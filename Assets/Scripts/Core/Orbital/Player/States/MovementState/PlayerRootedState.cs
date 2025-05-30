using UnityEngine;

namespace Orpheus.Core.Orbital.Player.States.MovementState
{
    public class PlayerRootedState : PlayerMovementState
    {
        private bool IsRooted;
        public override void OnExit(PlayerOrbitalController orbitalController)
        {
            
        }

        public override int GetStatePriority(PlayerOrbitalController orbitalController)
        {
            if (IsRooted)
            {
                return 50;
            }
            return -1;
        }

        public override Vector2 GetVelocity(PlayerOrbitalController orbitalController, float deltaTime)
        {
           return Vector2.zero;
        }

        public override void Initialize(PlayerOrbitalController orbitalController)
        {
            
        }

        public override void Dispose(PlayerOrbitalController orbitalController)
        {
            
        }

        public override void OnEnter(PlayerOrbitalController orbitalController)
        {
           
        }
    }
}