using DG.Tweening;
using NaughtyAttributes;
using Orpheus.Core.FightSystem;
using UnityEngine;

namespace Orpheus.Core.Orbital.Player.States.MovementState
{
    [CreateAssetMenu(fileName = "DashState", menuName = "Orpheus/Movement/Dash", order = 0)]
    public class DashState: Movement 
    {
        [SerializeField, BoxGroup("dash")] 
        private Ease deashease;
        [SerializeField, BoxGroup("dash")] 
        private float dashForce;
        private Vector3 dashDir;
        private Tween dash;
        
        public override void Initialize(Transform transform, IAbilityTarget target, float direction, float duration)
        {
            dashDir = transform.forward;
            Duration = duration;
            Vector2 angularVelocity = new Vector2(target.CurrentRing.GetAngularSpeed(direction*dashForce),0) * Time.deltaTime;
            nextpoint = target.CurrentRing.GetPositionOnRing(transform.position, angularVelocity);
            nextpoint = OrbitalMath.ClampToRing(nextpoint, target.CurrentRing.transform.position, target.CurrentRing.RingData.Radius);
        }

        public override void ApplyMovement(Transform transform, float deltaTime, IAbilityTarget target)
        {
            dash = transform.DOMove(nextpoint, Duration).OnComplete(() =>
                {
                    IsFinished = true;
                })
                .SetEase(deashease).SetUpdate(true);
        }
    }
}