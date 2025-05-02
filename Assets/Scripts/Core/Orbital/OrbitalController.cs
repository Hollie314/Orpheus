using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public abstract class OrbitalController<T> : MonoBehaviour where T : OrbitalController<T>
{
    [Header("Collision")]
    [SerializeField, Range(1,64)] private int maxPenetrationCount;
    [Header("Physics")]
    [SerializeField, Range(0,10)] private float gravityScale;
    [Header("Ground Collision")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField, Range(0,1)] private float groundDetectionRange;
    [SerializeField, Range(0,90)] private float groundMaxAngle;

    public Vector2 CurrentVelocity { get; private set; }
    public Ring CurrentRing { get; private set; }
    public bool IsGrounded { get; private set;}
    public Vector3 GroundNormal { get; private set; }
    public Vector3 GroundPosition { get; private set; }

    private List<IOrbitalMovementState<T>> _movementStates;
    private IOrbitalMovementState<T> currentMovementState;
    private Rigidbody rb;
    private CapsuleCollider cc;
    private static readonly Collider[] colliders = new Collider[16];
    private static readonly RaycastHit[] raycastHits = new RaycastHit[16];
    

    protected virtual void Awake()
    {
        this.rb = GetComponent<Rigidbody>();
        this.cc = GetComponent<CapsuleCollider>();
        _movementStates = new List<IOrbitalMovementState<T>>();
    }

    private void FixedUpdate()
    {
        CheckGround();
        SelectNextState();
        ComputeVelocity();
        ApplyGravity();
        Move();
    }

    private T GetController() => this as T;

    public void SetRing(Ring ring)
    {
        CurrentRing = ring;
    }

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
        int maxPriority = 0;
        foreach (var state in _movementStates)
        {
            int priority = state.GetStatePriority(GetController());
            if (priority > maxPriority)
            {
                maxPriority = priority;
                nextMovementState = state;
            }
        }

        if (currentMovementState != nextMovementState)
        {
            currentMovementState?.OnExit(GetController());
            nextMovementState?.OnEnter(GetController());
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

        CurrentVelocity = currentMovementState.GetVelocity(GetController());
    }

    private void ApplyGravity()
    {
        
        if (IsGrounded)
        {
            CurrentVelocity = new Vector2(CurrentVelocity.x, 0);
        }
        else
        {
            CurrentVelocity += Vector2.down * (gravityScale * Time.deltaTime * 9.81f);
        }
        Debug.Log(CurrentVelocity);
    }
    private void Move()
    {
        
        Vector3 newPosition = CurrentRing.GetPositionOnRing(rb.position, CurrentVelocity);
        var lastPosition = rb.position;
        Vector3 finalVelocity = newPosition - lastPosition;

     

        float deltaTime = Time.deltaTime;
        Vector3 p1 = lastPosition + cc.center + transform.up * (-cc.height * 0.25f);
        Vector3 p2 = p1 + transform.up * cc.height;


        Vector3 collisionOffset = Vector3.zero;

        for (int i = 0; i < maxPenetrationCount; i++)
        {
            Vector3 nextPosition = rb.position + collisionOffset + finalVelocity * deltaTime;
            Vector3 nextP1 = p1 + collisionOffset;
            Vector3 nextP2 = p2 + collisionOffset;
            int count = Physics.OverlapCapsuleNonAlloc(nextP1, nextP2, cc.radius - 0.01f, colliders);
            
            if (count > 0)
            {
                for (int j = 0; j < count; j++)
                {
                    Collider collider = colliders[j];

                    if (collider.attachedRigidbody == rb)
                        continue;

                    if (Physics.GetIgnoreLayerCollision(collider.gameObject.layer, cc.gameObject.layer))
                        continue;

                    if (Physics.GetIgnoreCollision(collider, cc))
                        continue;

                    Vector3 otherPosition = collider.transform.position;
                    Quaternion otherRotation = collider.transform.rotation;

                    if (Physics.ComputePenetration(cc, nextPosition, rb.rotation, collider, otherPosition,
                            otherRotation, out Vector3 direction, out float distance))
                    {
                        Vector3 offset = direction * distance;
                        Debug.DrawLine(rb.position + collisionOffset, rb.position + collisionOffset + offset);
                        collisionOffset += offset;
                    }
                }
            }
            else
            {
                break;
            }
        }
        Debug.Log(finalVelocity);
        Vector3 nonOrbitalNewPosition = rb.position  + finalVelocity * deltaTime;//+ collisionOffset
        //Vector3 orbitalNewPosition = CurrentRing.ClampToRing(nonOrbitalNewPosition);
        rb.MovePosition(nonOrbitalNewPosition);
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
    }
    
    
    
}