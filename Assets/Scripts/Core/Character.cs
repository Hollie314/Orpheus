using UnityEngine;
using UnityEngine.InputSystem;

namespace Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class Character : MonoBehaviour, IMovable
    {
        [SerializeField]
        public Transform center;         // Center of the ring
        public float radius = 5f;        // Radius of the ring
        public float speed = 1f;         // Angular speed (radians per second)

        private float angle = 0f;        // Current angle around the ring
       [SerializeField]
        private Rigidbody rb;

        private Vector2 movementInput = new Vector2(1,1);

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }
        
        public void OnMove(InputAction.CallbackContext context)
        {
            Debug.Log(context.ReadValue<Vector2>());
            movementInput = context.ReadValue<Vector2>();

            if (context.canceled)
            {
                movementInput = Vector2.zero;
            }
        }
        
        public void OnSwapRing(InputAction.CallbackContext context)
        {
            
        }
        void Update()
        {
            if (movementInput.x != 0)
            {
                MoveOnRing(movementInput.x);
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

        public float GetAngle()
        {
            return OrbitalMath.GetAngleFromPosition(center.position, rb.position);
        }

        public void SetAngle(float angle)
        {
            angle = angle;
        }

        public void MoveOnRing(float direction)
        {
            angle = GetAngle();
            angle += speed/radius * direction * Time.deltaTime;
            Vector3 newposition = OrbitalMath.GetPositionOnRing(center.position, radius, angle);
            rb.MovePosition(new Vector3(newposition.x, rb.position.y,newposition.z));
            newposition = OrbitalMath.ClampToRing(rb.position, center.position, radius);
            rb.position = new Vector3(newposition.x, rb.position.y, newposition.z);
           // rb.rotation.SetLookRotation(OrbitalMath.GetTangent(rb.position, center.position, direction));
            this.transform.LookAt(OrbitalMath.GetTangent(rb.position, center.position, direction));
        }
    }
}
