using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Orpheus.Core.Orbital.Player
{
    public abstract class PlayerControlledMovementState : PlayerMovementState
    {
        [SerializeField, BoxGroup("Base")] private float maxSpeed;
        [SerializeField, BoxGroup("Base")] protected float acceleration;
        [SerializeField, BoxGroup("Base")] protected float deceleration;
        [SerializeField, BoxGroup("Base")] protected float stopForce;

        private InputAction moveAction;
        
        public override void Initialize(PlayerOrbitalController orbitalController)
        {
            moveAction = orbitalController.PlayerInput.actions.FindActionMap("Player").FindAction("Move");
        }

        public override void Dispose(PlayerOrbitalController orbitalController)
        {
            moveAction = null;
        }

        public override Vector2 GetVelocity(PlayerOrbitalController orbitalController, float deltaTime)
        {
            Vector2 direction = GetTargetDirection(orbitalController);
            if (direction.x != 0)
            {
                orbitalController.SetDirection((int)Mathf.Sign(direction.x));
            }
            Vector2 targetVelocity = direction * maxSpeed;
            Vector2 currentVelocity = orbitalController.CurrentVelocity;
            
            float accelerationForce = 0;

            if (Vector2.Dot(targetVelocity, currentVelocity) < 0)
                accelerationForce = GetStopForce(orbitalController);
            else
            {
                float currentSpeed = currentVelocity.sqrMagnitude;
                float maxSpeedSqr = maxSpeed * maxSpeed;
                
                if (currentSpeed < maxSpeedSqr)
                    accelerationForce = acceleration;
                else
                    accelerationForce = deceleration;
            }

            Vector2 newVelocity = Vector2.MoveTowards(currentVelocity, targetVelocity, accelerationForce * deltaTime);
            
            return newVelocity;
        }


        protected virtual Vector2 GetTargetDirection(PlayerOrbitalController controller) => new Vector2()
        {
            x = moveAction.ReadValue<Vector2>().x,
            y = 0,
        };
        
        protected virtual float GetStopForce(PlayerOrbitalController orbitalController) => stopForce;
    }
}