using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Player
{
    [CreateAssetMenu(fileName = "walkState", menuName = "Orphee/Player/Walk", order = 0)]
    public class PlayerWalkState : PlayerMovementState
    {
        [SerializeField] private float maxSpeed;
        [SerializeField] private float acceleration;
        [SerializeField] private float deceleration;

        private InputAction moveAction;
        
        public override int GetStatePriority(PlayerOrbitalController orbitalController)
        {
            return 1;
        }

        public override Vector2 GetVelocity(PlayerOrbitalController orbitalController)
        {
            float xVelocity = moveAction.ReadValue<Vector2>().x;
            if (xVelocity != null)
            {
                return new Vector2(Math.Sign(xVelocity) * maxSpeed,0) ;
            }
            return Vector2.zero;
        }

        public override void OnEnter(PlayerOrbitalController orbitalController)
        {
           
        }

        public override void OnExit(PlayerOrbitalController orbitalController)
        {
            
        }

        public override void Initialize(PlayerOrbitalController orbitalController)
        {
            moveAction = orbitalController.PlayerInput.actions.FindActionMap("Player").FindAction("Move");
        }

        public override void Dispose(PlayerOrbitalController orbitalController)
        {
            moveAction = null;
        }
    }
}