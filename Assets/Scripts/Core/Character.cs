using UnityEngine;
using UnityEngine.InputSystem;

namespace Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class Character : MonoBehaviour, IMovable
    {
        [field:SerializeField] public Transform center {get; private set; }   // Transform of the ring we're on so we get the center
        [field:SerializeField] public float radius {get; private set; } = 5f ;   // Radius of the ring
        [SerializeField] private float speed = 10f;       // linear speed (unit per second)
        private float angle = 0f;        // Current angle around the ring
        private float direction = 0;
       
        [SerializeField]
        private Rigidbody rb;

        private Vector2 movementInput = new Vector2(1,1);

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void SetRing(Transform ring, float radius)
        {
            center = ring;
            this.radius = radius;
        }
        public void OnMove(InputAction.CallbackContext context)
        {
            movementInput = context.ReadValue<Vector2>();
            direction = context.ReadValue<Vector2>().x;
            if (context.canceled)
            {
                direction = 0;
                movementInput = Vector2.zero;
            }
        }
        
        public void OnSwapRing(InputAction.CallbackContext context)
        {
            
        }
        void FixedUpdate()
        {
            if (direction!= 0)
            {
                MoveOnRing();
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                Debug.Log("Hit an enemy!");
            }

            if (other.CompareTag("Hazard"))
            {
                Debug.Log("Touched a hazard!");
            }
        }

        public void SetAngle(float angle)
        {
            this.angle = angle;
        }

        public void MoveOnRing()
        {
            angle = GetAngle(); //get actual angle
            angle += GetAngularSpeed() * direction * Time.deltaTime; //calculate new angle based on speed
            Vector3 newposition = OrbitalMath.GetPositionFromAngle(center.position, radius, angle); //calculate new position
            rb.MovePosition(new Vector3(newposition.x, rb.position.y,newposition.z)); //move to position
            newposition = OrbitalMath.ClampToRing(rb.position, center.position, radius); //calculate clamped position
           // rb.MovePosition(new Vector3(newposition.x, rb.position.y, newposition.z)); //clamp to ring
            
            Vector3 tangentDir = OrbitalMath.GetTangent(rb.position, center.position, direction);
            Vector3 lookTarget = rb.position + tangentDir;
            transform.LookAt(lookTarget);
        }

        public float GetAngle()
        {
            return OrbitalMath.GetAngleFromPosition(center.position, rb.position);
        }
        
        public float GetAngularSpeed()
        {
            return speed / radius;
        }
    }
}
