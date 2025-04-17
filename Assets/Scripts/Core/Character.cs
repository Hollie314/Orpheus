using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class Character : MonoBehaviour, IMovable
    {
        [Header("Ring Settings")]
        [field:SerializeField] public Transform[] center {get; private set; }   // Transform of the ring we're on so we get the center
        [field:SerializeField] public float[] radius {get; private set; }   // Radius of the ring
        public (Transform, float) ring {get; private set; } 
        private int ring_index;
        
        [Header("Orbital Movement Settings")]
        
        private float angle = 0f;   // Current angle on the ring
        private float direction = 0;  //current direction (left/right)
        private bool isMoving;    
        private float radiusOffset = 0.2f;
        
        [Header("Character Stats")]
        [SerializeField] private float speed = 10f; // linear speed (unit per second)
        private float jumpHeight = 10;
        private bool canMove;
        private Rigidbody rb;

        private Tween jump;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            canMove = true;
            //changed how it's set later with the ring system
            ring_index = 0;
            SetRing(center[ring_index], radius[ring_index]);
        }
        
        void FixedUpdate()
        {
            if (isMoving)
            {
                MoveOnRing();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                KnockBack(new Vector3(5,5,5));
                ClampToRing();
            }
        }

        public void SetRing(Transform ring, float radius)
        {
            this.ring = (ring, radius);
            canMove = false;
            Vector3 target = OrbitalMath.ClampToRing(rb.position, ring.position, radius); //calculate position on next ring
            jump = rb.transform.DOJump(target, 0.3f, 1, 0.3f, false)
                .OnComplete(() =>
                {
                    canMove = true;
                });
        }
        
        public void OnMove(InputAction.CallbackContext context)
        {
            if (canMove)
            {
                if (context.ReadValue<Vector2>().x != 0)
                {
                    isMoving = true;
                    if (context.started)
                    {
                        direction = context.ReadValue<Vector2>().x;
                    }
                }
            }
            if (context.canceled)
            {
                isMoving = false;
            }
        }
        
        public void OnSwapRing(InputAction.CallbackContext context)
        {
            if (canMove)
            {
                switch (context.ReadValue<Vector2>().y)
                    {
                        case > 0 :
                            RingSwap(ring_index - 1);
                            break;
                        case < 0 :RingSwap(ring_index + 1);
                            break;
                        default:
                            break;
                    }
            }
        }

        private void RingSwap(int ring_index)
        {
            bool canSwap = false;
            if (ring_index >= 0 && ring_index< center.Length)
            {
                Debug.Log(this.ring_index);
                //faire un raycast pour voir s'il n'y a pas d'obstacle
                this.ring_index = ring_index;
                SetRing(center[ring_index], radius[ring_index]);
            }
        }
        
        
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                //todolater
                Debug.Log("Hit an enemy!");
            }
        }

        public void SetAngle(float angle)
        {
            this.angle = angle;
        }
        

        public void MoveOnRing()
        {
            //move on the ring
            angle = GetAngle(); //get actual angle
            angle += GetAngularSpeed(speed) * direction * Time.deltaTime; //calculate new angle based on speed
            Vector3 newposition = OrbitalMath.GetPositionFromAngle(ring.Item1.position, ring.Item2, angle); //calculate new position
            rb.MovePosition(new Vector3(newposition.x, rb.position.y,newposition.z)); //move to position
            Lookforward();
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
           
            Lookforward();
        }

        public void Lookforward()
        {
            //tangent of the ring
            Vector3 tangentDir = OrbitalMath.GetTangent(rb.position, ring.Item1.position, direction);
            Vector3 lookTarget = rb.position + tangentDir;
            transform.LookAt(lookTarget);
        }

        public float GetAngle()
        {
            return OrbitalMath.GetAngleFromPosition(ring.Item1.position, rb.position);
        }
        
        public float GetAngularSpeed(float force)
        {
            return force / ring.Item2;
        }

        public void ClampToRing()
        {
            //check if too far from the center of the ring
            if (Vector3.Distance(rb.position, ring.Item1.position) - ring.Item2 > radiusOffset)
            {
                Debug.Log("toofar");
                Vector3 newposition = OrbitalMath.ClampToRing(rb.position, ring.Item1.position, ring.Item2); //calculate clamped position
                rb.MovePosition(new Vector3(newposition.x, rb.position.y, newposition.z)); //clamp to ring
            }
        }
    }
}
