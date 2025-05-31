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
        }

        public override void ApplyMovement(Transform transform, float deltaTime, IAbilityTarget target)
        {
            dash = transform.DOMove(transform.position + dashDir * dashForce, Duration).OnComplete(() =>
                {
                    IsFinished = true;
                })
                .SetEase(deashease);
        }
    }
}