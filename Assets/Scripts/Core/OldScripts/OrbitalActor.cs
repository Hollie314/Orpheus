using System;
using DG.Tweening;
using UnityEngine;

namespace Orpheus.Core.Orbital
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class OrbitalActor : MonoBehaviour
    {
        [Header("Ring Settings")]
        [field:SerializeField] public Transform[] center {get; private set; }   // Transform of the ring we're on so we get the center
        [field:SerializeField] public float[] radius {get; private set; }   // Radius of the ring
        public (Transform, float) ring {get; private set; } 
        protected int ring_index;
        
        [Header("Orbital Movement Settings")]
        protected float angle = 0f;   // Current angle on the ring
        protected float direction = 0;  //current direction (left/right)
        protected bool isMoving;    
        protected float radiusOffset = 0f;
        
        [Header("Character Stats")]
        [SerializeField] private float speed = 10f; // linear speed (unit per second)
        protected float jumpHeight = 10;
        protected bool canMove;
        
        protected Rigidbody rb;
        protected Tween jump;

        protected virtual void Start()
        {
            rb = GetComponent<Rigidbody>();
            canMove = true;
            
            //changed how it's set later with the ring system
            ring_index = 0;
            SetRing(center[ring_index], radius[ring_index]);
        }

        protected virtual void FixedUpdate()
        {
            if (isMoving)
            {
                MoveOnRing();
            }
            Lookforward(Time.fixedTime);
        }

        protected void SetRing(Transform ring, float radius)
        {
            this.ring = (ring, radius);
        }

        protected void JumpOnRing()
        {
            canMove = false;
            Vector3 target = OrbitalMath.ClampToRing(rb.position, ring.Item1.position, ring.Item2); //calculate position on next ring
            jump = rb.transform.DOJump(target, 0.3f, 1, 0.3f, false)
                .OnComplete(() =>
                {
                    canMove = true;
                });
        }

        protected void RingSwap(int ring_index)
        {
            bool canSwap = false;
            if (ring_index >= 0 && ring_index< center.Length)
            {
                Debug.Log(this.ring_index);
                //faire un raycast pour voir s'il n'y a pas d'obstacle
                this.ring_index = ring_index;
                SetRing(center[ring_index], radius[ring_index]);
                JumpOnRing();
            }
        }

        public void SetAngle(float angle)
        {
            this.angle = angle;
        }
        

        protected void MoveOnRing()
        {
            //move on the ring
            angle = GetAngle(); //get actual angle
            angle += GetAngularSpeed(speed) * direction * Time.deltaTime; //calculate new angle based on speed
            Vector3 newposition = OrbitalMath.GetPositionFromAngle(ring.Item1.position, ring.Item2, angle); //calculate new position
            rb.MovePosition(new Vector3(newposition.x, rb.position.y,newposition.z)); //move to position
        }
        
        public void KnockBack(Vector3 force)
        {
            float forwardforce = Vector2.Distance(Vector2.zero, new Vector2(force.x, force.z));
            float direction = Math.Sign(force.x);
            float upforce = force.y;
            //Vector3 direction = (force-Vector3.zero).normalized;
            angle = GetAngle(); //get actual angle
            angle += GetAngularSpeed(forwardforce) * direction * Time.deltaTime; //calculate new angle based force and direction
            Vector3 newposition = OrbitalMath.GetPositionFromAngle(ring.Item1.position, ring.Item2, angle); //calculate new position
            rb.MovePosition(new Vector3(newposition.x, upforce,newposition.z)); //move to position
        }

        protected void Lookforward(float time)
        {
            //tangent of the ring
            Vector3 currentposition = rb.position;
            Vector3 tangentDir = OrbitalMath.GetTangent(currentposition, ring.Item1.position, direction);
            Vector3 lookTarget = currentposition + tangentDir;
            //transform.LookAt(lookTarget);
            
            Quaternion targetRotation = Quaternion.LookRotation(lookTarget - currentposition);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, time);
        }

        public float GetAngle()
        {
            return OrbitalMath.GetAngleFromPosition(ring.Item1.position, rb.position);
        }
        
        public float GetAngularSpeed(float force)
        {
            return force / ring.Item2;
        }
    }
}
