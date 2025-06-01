using System;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using Orpheus.Core.FightSystem;
using Orpheus.Core.Rings;
using UnityEngine;

namespace Orpheus.Core.Orbital
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public abstract class OrbitalController<T> : MonoBehaviour where T : OrbitalController<T>
    {
        [BoxGroup("Collision")]
        [SerializeField, Range(1,64)] private int maxPenetrationCount;
        [BoxGroup("Physics")]
        [SerializeField, Range(0,10)] private float gravityScale;
        [BoxGroup("Ground Collision")]
        [SerializeField] private LayerMask groundMask;
        [BoxGroup("Ground Collision")]
        [SerializeField, Range(0,1)] private float groundDetectionRange;
        [BoxGroup("Ground Collision")]
        [SerializeField, Range(0,90)] private float groundMaxAngle;
        [BoxGroup("State")] 
        [SerializeField] private bool IsFlying;

        [field: ShowNonSerializedField]
        public Vector2 CurrentVelocity { get; private set; }
        [field: ShowNonSerializedField, ShowIf("@Application.isPlaying")]
        public Ring CurrentRing { get; protected set; }
        [field: ShowNonSerializedField]
        public bool IsGrounded { get; private set;}
        [field: ShowNonSerializedField]
        public bool IsBlocked { get; private set;}
        [field: ShowNonSerializedField]
        public Vector3 GroundNormal { get; private set; }
        [field: ShowNonSerializedField]
        public Vector3 GroundPosition { get; private set; }
        
        
        [field: ShowNonSerializedField]
        public float Direction { get; protected set; }
    

        private List<IOrbitalMovementState<T>> _movementStates;
        public IOrbitalMovementState<T> currentMovementState { get; private set; }
        private Rigidbody rb;
        private CapsuleCollider cc;
        private static readonly Collider[] colliders = new Collider[16];
        private static readonly RaycastHit[] raycastHits = new RaycastHit[16];
        
        [BoxGroup("Stats")]
        [SerializeField] private OrbitalStatsData orbitalStatsData;
        public OrbitalStats Stats { get; private set;}

        protected virtual void Awake()
        {
            this.rb = GetComponent<Rigidbody>();
            this.cc = GetComponent<CapsuleCollider>();
            _movementStates = new List<IOrbitalMovementState<T>>();
            Stats = new OrbitalStats();
            Stats.Initialize(orbitalStatsData,1);
        }

        protected virtual void FixedUpdate()
        {
            CheckGround();
            SelectNextState();
            ComputeVelocity();
            if (!IsFlying)
            {
                ApplyGravity();
            }
            Move();
            Lookforward();
        }

        private T GetController() => this as T;

        public void SetRing(Ring ring)
        {
            if (ring != null)
            {
                CurrentRing = ring;
            }
        }

        //might need to instantiate SO here to avoid issue with shared datas
        public void AddState(IOrbitalMovementState<T> orbitalMovementState)
        {
            if (_movementStates.Contains(orbitalMovementState))
            {
                return;
            }
            _movementStates.Add(orbitalMovementState);
            orbitalMovementState.Initialize(GetController());
        }

        public void RemoveState(IOrbitalMovementState<T> orbitalMovementState)
        {
            if (_movementStates.Remove(orbitalMovementState))
            {
                orbitalMovementState.Dispose(GetController());
            }
        }

        private void SelectNextState()
        {
            IOrbitalMovementState<T> nextMovementState = null;
            T controller = GetController();
            int maxPriority = 0;
            
            foreach (var state in _movementStates)
            {
                state.PreUpdate(controller);
                int priority = state.GetStatePriority(controller);
                if (priority > maxPriority)
                {
                    maxPriority = priority;
                    nextMovementState = state;
                }
            }

            if (currentMovementState != nextMovementState)
            {
                currentMovementState?.OnExit(controller);
                nextMovementState?.OnEnter(controller);
                currentMovementState = nextMovementState;
            }
        }

        private void ComputeVelocity()
        {
            if (currentMovementState==null)
            {
                CurrentVelocity = Vector2.zero;
                return;
            }

            CurrentVelocity = currentMovementState.GetVelocity(GetController(), Time.deltaTime);
        }

        private void ApplyGravity()
        {
            if (!IsGrounded)
            {
                CurrentVelocity += Vector2.down * (gravityScale * Time.deltaTime * 9.81f);
            }
            else if(CurrentVelocity.y <= 0)
            {
                CurrentVelocity = new Vector2(CurrentVelocity.x, 0);
            }
            //Debug.Log("gravity : "+CurrentVelocity);
        }
        private void Move()
        {
            float deltaTime = Time.deltaTime;
        
            Vector3 newPosition = rb.position + new Vector3(CurrentVelocity.x, CurrentVelocity.y, 0);
            var lastPosition = rb.position;
            Vector3 finalVelocity = (newPosition - lastPosition);
            //Debug.Log("current velocity on y : "+ CurrentVelocity.y+" final velocity on y : "+ finalVelocity.y);
            //Debug.Log("current velocity on x : "+ CurrentVelocity.x+" final velocity on x : "+ finalVelocity.x);
        
            Vector2 angularVelocity = new Vector2(CurrentRing.GetAngularSpeed(CurrentVelocity.x),CurrentVelocity.y) * Time.deltaTime;
            
            Vector3 p1 = lastPosition + cc.center + transform.up * (-cc.height * 0.25f);
            Vector3 p2 = p1 + transform.up * cc.height;


            Vector3 collisionOffset = Vector3.zero;

            for (int i = 0; i < maxPenetrationCount; i++)
            {
                Vector3 nextPosition = rb.position + collisionOffset + finalVelocity * deltaTime;
                nextPosition = CurrentRing.GetPositionOnRing(rb.position+  collisionOffset, angularVelocity);
                //nextPosition = CurrentRing.ClampToRing(nextPosition);
                Vector3 nextP1 = p1 + collisionOffset;
                Vector3 nextP2 = p2 + collisionOffset;
                int count = Physics.OverlapCapsuleNonAlloc(nextP1, nextP2, cc.radius - 0.01f, colliders);

                int numberOfObstacle = 0;
                
                if (count > 0)
                {
                    for (int j = 0; j < count; j++)
                    {
                        Collider c = colliders[j];

                        if (c.attachedRigidbody == rb)
                            continue;

                        if (Physics.GetIgnoreLayerCollision(c.gameObject.layer, cc.gameObject.layer))
                            continue;

                        if (Physics.GetIgnoreCollision(c, cc))
                            continue;
                        if (c.isTrigger)
                            continue;

                        numberOfObstacle++;
                        Vector3 otherPosition = c.transform.position;
                        Quaternion otherRotation = c.transform.rotation;

                        if (Physics.ComputePenetration(cc, nextPosition, rb.rotation, c, otherPosition,
                                otherRotation, out Vector3 direction, out float distance))
                        {
                            Vector3 offset = direction * distance;
                            Debug.DrawLine(rb.position + collisionOffset, rb.position + collisionOffset + offset);
                            collisionOffset += offset;
                        }
                    }

                    if (numberOfObstacle > 0 && !IsGrounded)
                    {
                        IsBlocked = true;
                    }
                    else
                    {
                        IsBlocked = false;
                    }
                }
                else
                {
                    IsBlocked = false;
                    break;
                }
            }
            //Debug.Log(" final vel : "+angularVelocity);
            
            Vector3 orbitalNewPosition =  CurrentRing.GetPositionOnRing(rb.position + collisionOffset, angularVelocity);
            rb.MovePosition(orbitalNewPosition);
        }

        private void CheckGround()
        {
            var up = transform.up;
            Vector3 p1 = rb.position + cc.center + up * (-cc.height * 0.25f);
            Vector3 p2 = p1 + up * cc.height;

            float shrink = 0.02f;
            int count = Physics.CapsuleCastNonAlloc(p1, p2, cc.radius - shrink, Vector3.down, raycastHits,
                groundDetectionRange + shrink, groundMask);
            IsGrounded = false;
            GroundNormal = Vector3.up;
            GroundPosition = rb.position;

            for (int i = 0; i < count; i++)
            {
                RaycastHit hit = raycastHits[i];
                float angle = Vector3.Angle(Vector3.up, hit.normal);
                if (angle < groundMaxAngle)
                {
                    IsGrounded = true;
                    GroundNormal = hit.normal;
                    GroundPosition = hit.point;
                    return;
                }
            }
        
            //Debug.Log("we are on the ground : "+IsGrounded);
        }
    
        protected void Lookforward()
        {
            //tangent of the ring
            Vector3 currentposition = rb.position;
            Vector3 tangentDir = OrbitalMath.GetTangent(currentposition, CurrentRing.transform.position, Math.Sign(Direction));
            Vector3 lookTarget = currentposition + tangentDir;
        
            transform.DOLookAt(lookTarget,0.2f);
        }

        public void SetDirection(int dir)
        {
            Direction = dir;
        }
        
    }
}