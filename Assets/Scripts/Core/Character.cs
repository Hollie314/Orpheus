using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class Character : MonoBehaviour, IMovable
    {
        [field:SerializeField] public Transform center {get; private set; }   // Transform of the ring we're on so we get the center
        [field:SerializeField] public float radius {get; private set; } = 5f ;   // Radius of the ring
        
        public (Transform, float) ring {get; private set; }
        private float angle = 0f;   // Current angle on the ring
        private float direction = 0;  //current direction (left/right)
        private bool isMoving;    
        private float radiusOffset = 0.2f;
        
        [SerializeField] private float speed = 10f; // linear speed (unit per second)
        private Rigidbody rb;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            
            //changed how it's set later with the ring system
            SetRing(center, radius);
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
        }
        public void OnMove(InputAction.CallbackContext context)
        {
            isMoving = true;
            if (context.started)
            {
                direction = context.ReadValue<Vector2>().x;
            }
            if (context.canceled)
            {
                isMoving = false;
            }
        }
        
        public void OnSwapRing(InputAction.CallbackContext context)
        {
            
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
            if (Vector3.Distance(rb.position, center.position) - radius > radiusOffset)
            {
                Debug.Log("toofar");
                Vector3 newposition = OrbitalMath.ClampToRing(rb.position, ring.Item1.position, ring.Item2); //calculate clamped position
                rb.MovePosition(new Vector3(newposition.x, rb.position.y, newposition.z)); //clamp to ring
            }
        }
    }
}
