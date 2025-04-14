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
        private float direction = 0;    //current direction (left/right)
        
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
            if (direction!= 0)
            {
                MoveOnRing();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                KnockBack(-10f);
            }
            // ClampToRing();
        }
        

        public void SetRing(Transform ring, float radius)
        {
            this.ring = (ring, radius);
        }
        public void OnMove(InputAction.CallbackContext context)
        {
            direction = context.ReadValue<Vector2>().x;
            if (context.canceled)
            {
                direction = 0;
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

        public void KnockBack(float force)
        {
            //c'est de la merde
            angle = GetAngle(); //get actual angle
            angle += GetAngularSpeed(force) * Time.deltaTime; //calculate new angle based on speed
            Vector3 newposition = OrbitalMath.GetPositionFromAngle(ring.Item1.position, ring.Item2, angle); //calculate new position
            rb.MovePosition(new Vector3(newposition.x, rb.position.y,newposition.z)); //move to position
            Lookforward();

        }

        public void Lookforward()
        {
            //lookat direction
            Vector3 tangentDir = OrbitalMath.GetTangent(rb.position, ring.Item1.position, rb.transform.forward.x);
            Vector3 lookTarget = rb.position + tangentDir;
            transform.LookAt(lookTarget);
        }
        
        public void MoveOnRing()
        {
            //move on the ring
            angle = GetAngle(); //get actual angle
            angle += GetAngularSpeed(speed) * direction * Time.deltaTime; //calculate new angle based on speed
            Vector3 newposition = OrbitalMath.GetPositionFromAngle(ring.Item1.position, ring.Item2, angle); //calculate new position
            rb.MovePosition(new Vector3(newposition.x, rb.position.y,newposition.z)); //move to position
            
            
            //lookat direction
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
            Vector3 newposition = OrbitalMath.ClampToRing(rb.position, ring.Item1.position, ring.Item2); //calculate clamped position
            rb.MovePosition(new Vector3(newposition.x, rb.position.y, newposition.z)); //clamp to ring
        }
    }
}
