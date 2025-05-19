

using UnityEngine;

namespace Orpheus.Core.Orbital.Player.States
{
    [CreateAssetMenu(menuName = "Create PlayerMobileAbility", fileName = "PlayerMobileAbility", order = 0)]
    public abstract class PlayerMobileAbility : PlayerMovementState
    {
        public override void OnExit(PlayerOrbitalController orbitalController)
        {
            
        }

        public override int GetStatePriority(PlayerOrbitalController orbitalController)
        {
            return -1;
        }

        public override Vector2 GetVelocity(PlayerOrbitalController orbitalController, float deltaTime)
        {
            return Vector2.zero;
        }

        public override void Initialize(PlayerOrbitalController orbitalController)
        {
            ;
        }

        public override void Dispose(PlayerOrbitalController orbitalController)
        {
            
        }

        public override void OnEnter(PlayerOrbitalController orbitalController)
        {
            
        }
    }
}