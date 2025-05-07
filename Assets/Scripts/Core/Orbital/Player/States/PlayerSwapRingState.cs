using UnityEngine;
using UnityEngine.InputSystem;

namespace Orpheus.Core.Orbital.Player
{
    [CreateAssetMenu(fileName = "swapRingState", menuName = "Orpheus/Player/SwapRing", order = 0)]
    
    public class PlayerSwapRingState : PlayerControlledMovementState
    {
        private InputAction swapInput;
        private float swipe;

        public override void Initialize(PlayerOrbitalController orbitalController)
        {
            base.Initialize(orbitalController);
            swapInput = orbitalController.PlayerInput.actions.FindActionMap("Player").FindAction("Jump");
            swapInput.performed += OnSwapPerfomed;
        }
        
        
        public override int GetStatePriority(PlayerOrbitalController orbitalController)
        {
            return 20;
        }

        public override void OnEnter(PlayerOrbitalController orbitalController)
        {
           
        }

        public override void OnExit(PlayerOrbitalController orbitalController)
        {
            
        }

        public override Vector2 GetVelocity(PlayerOrbitalController orbitalController, float deltaTime)
        {
            return Vector2.zero;
        }

        private void OnSwapPerfomed(InputAction.CallbackContext obj)
        {
            swipe = Mathf.Sign(swapInput.ReadValue<Vector2>().y);
        }
        
        protected void JumpOnRing(PlayerOrbitalController orbitalController)
        {
           
        }
        
        
    }
}