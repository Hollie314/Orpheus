using DG.Tweening;
using Orpheus.Core.Rings;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Orpheus.Core.Orbital.Player
{
    [CreateAssetMenu(fileName = "swapRingState", menuName = "Orpheus/Player/SwapRing", order = 0)]
    
    public class PlayerSwapRingState : PlayerMovementState
    {
        private InputAction swapInput;
        private float swipeDirection;
        private Ring previousRing;
        private Ring nextRing;
        
        private bool isSwapingRing;
        private bool isSwapComplet;
        private Tween jump;

        public override void Initialize(PlayerOrbitalController orbitalController)
        {
            swapInput = orbitalController.PlayerInput.actions.FindActionMap("Player").FindAction("Move");
            swapInput.performed += OnSwapPerfomed;
        }

        public override void Dispose(PlayerOrbitalController orbitalController)
        {
            swapInput = null;
        }


        public override int GetStatePriority(PlayerOrbitalController orbitalController)
        {
            if (isSwapingRing)
            {
                return 20;
            }
            return -1;
        }

        public override void OnEnter(PlayerOrbitalController orbitalController)
        {
            nextRing = GetNewRing(orbitalController);
            previousRing = orbitalController.CurrentRing;
            if (!nextRing)
            {
                isSwapingRing = false;
                return;
            }
            orbitalController.SetRing(nextRing);
        }

        public override void OnExit(PlayerOrbitalController orbitalController)
        {
            
        }

        public override Vector2 GetVelocity(PlayerOrbitalController orbitalController, float deltaTime)
        {
            if (isSwapingRing&& nextRing!=null)
            {
                if (orbitalController.IsBlocked)
                {
                    nextRing = previousRing;
                    orbitalController.SetRing(nextRing);
                }
                if (GetDistance(orbitalController)>0)
                {
                    //calculate position on next ring
                    Vector3 target = OrbitalMath.ClampToRing(orbitalController.GroundPosition, nextRing.transform.position, nextRing.RingData.Radius); 
                    jump = orbitalController.transform.DOJump(target, 0.3f, 1, 0.3f, false)
                        .OnComplete(() =>
                        {
                            isSwapingRing = false;
                        });
                }
                else
                {
                    //isSwapingRing = false;
                    return Vector2.zero;
                }

                float distanceToNextRing;
            }
            if (orbitalController.IsGrounded)
            {
                return Vector2.zero;
            }
            
            return Vector2.zero;
        }

        private void OnSwapPerfomed(InputAction.CallbackContext obj)
        {
            if (!isSwapingRing)
            {
                swipeDirection = swapInput.ReadValue<Vector2>().y;
                if (swipeDirection != 0)
                {
                    isSwapingRing = true;
                    isSwapComplet = false;
                }
            }
        }

        private Ring GetNewRing(PlayerOrbitalController orbitalController)
        {
            if (swipeDirection > 0)
            {
                return orbitalController.CurrentRing.GetNextSmaller();
            }
            return orbitalController.CurrentRing.GetNextLarger();
        }

        private float GetDistance(PlayerOrbitalController orbitalController)
        {
            return (orbitalController.GroundPosition - nextRing.ClampToRing(orbitalController.GroundPosition))
                .magnitude;
        }
    }
}