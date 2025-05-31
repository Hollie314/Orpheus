using DG.Tweening;
using NaughtyAttributes;
using Orpheus.Core.FightSystem;
using UnityEngine;

namespace Orpheus.Core.Orbital.Player.States.MovementState
{
    [CreateAssetMenu(fileName = "KnockBack", menuName = "Orpheus/Movement/KnockBack", order = 0)]
    public class KnockBack : Movement 
    {
        [SerializeField, BoxGroup("Jump")] 
        private Ease knockease;
        [SerializeField, BoxGroup("Jump")] 
        private float knockForce;
        private Vector3 knockbackDir;
        private Tween knockback;
        
        public override void Initialize(Transform transform, IAbilityTarget target, float direction, float duration)
        {
            knockbackDir = transform.forward * direction;
            Duration = duration;
        }

        public override void ApplyMovement(Transform transform, float deltaTime, IAbilityTarget target)
        {
            knockback = transform.DOMove(transform.position + knockbackDir * knockForce, Duration).OnComplete(() =>
                {
                    IsFinished = true;
                })
                .SetEase(knockease);
        }
    }
}