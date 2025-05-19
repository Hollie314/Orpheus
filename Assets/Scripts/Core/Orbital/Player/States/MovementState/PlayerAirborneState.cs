using UnityEngine;

namespace Orpheus.Core.Orbital.Player.States.MovementState
{
    [CreateAssetMenu(fileName = "AirborneState", menuName = "Orpheus/Player/Airborne", order = 0)]
    public class PlayerAirborneState : PlayerControlledMovementState
    {
        public override void OnEnter(PlayerOrbitalController orbitalController)
        {
            
        }

        public override void OnExit(PlayerOrbitalController orbitalController)
        {
            
        }

        public override Vector2 GetVelocity(PlayerOrbitalController orbitalController, float deltaTime)
        {
            var velocity = base.GetVelocity(orbitalController, deltaTime);
            
            return new Vector2()
            {
                x = velocity.x,
                y = orbitalController.CurrentVelocity.y
            };
        }

        public override int GetStatePriority(PlayerOrbitalController orbitalController)
        {
            return orbitalController.IsGrounded ? -1 : 1;
        }
    }
}