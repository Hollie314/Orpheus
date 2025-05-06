using Orpheus.Core.Orbital;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Orpheus.Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class Character : OrbitalActor
    {
        protected override void Start()
        {
            base.Start();
            JumpOnRing();
        }
        
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
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

        public void OnJump()
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
    }
}
