using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Orpheus.Core.Orbital.Player.States.MovementState
{
    [CreateAssetMenu(fileName = "PlayerDunkState", menuName = "Orpheus/Player/PlayerDunk", order = 1)]
    public class PlayerDunkState : PlayerMovementState
    {
        [SerializeField, BoxGroup("Dunk")] private float distance;
        [SerializeField, BoxGroup("Dunk")] private float jumpPower;
        [SerializeField, BoxGroup("Dunk")] private float duration;
        private Tween dunk;
        private Vector3 nextpoint;
        
        
        public override void OnExit(PlayerOrbitalController orbitalController)
        {
            
        }

        public override int GetStatePriority(PlayerOrbitalController orbitalController)
        {
            return 30;
        }

        public override Vector2 GetVelocity(PlayerOrbitalController orbitalController, float deltaTime)
        {
            dunk = orbitalController.transform.DOJump(nextpoint, jumpPower, 1, duration, false);
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
            float angularSpeed = distance / (duration * orbitalController.CurrentRing.RingData.Radius);
            Vector2 angularVelocity = new Vector2(angularSpeed, 0);
            nextpoint = orbitalController.CurrentRing.GetPositionOnRing(orbitalController.GroundPosition, angularVelocity);
        }
    }
}