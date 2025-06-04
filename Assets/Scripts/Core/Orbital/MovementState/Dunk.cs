using DG.Tweening;
using NaughtyAttributes;
using Orpheus.Core.FightSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Orpheus.Core.Orbital.Player.States.MovementState
{
    [CreateAssetMenu(fileName = "Dunk", menuName = "Orpheus/Movement/Dunk", order = 1)]
    public class Dunk : Movement
    {
        [SerializeField, BoxGroup("Dunk")] private float distance;
        [SerializeField, BoxGroup("Dunk")] private float jumpPower;
        private Tween dunk;
        
        public override void Initialize(Transform transform, IAbilityTarget target, float direction, float duration)
        {
            Duration = duration;
            Vector2 angularVelocity = new Vector2(target.CurrentRing.GetAngularSpeed(direction*distance),0) * Time.deltaTime;
            nextpoint = target.CurrentRing.GetPositionOnRing(transform.position, angularVelocity);
            nextpoint = OrbitalMath.ClampToRing(nextpoint, target.CurrentRing.transform.position, target.CurrentRing.RingData.Radius);
        }

        public override void ApplyMovement(Transform transform, float deltaTime, IAbilityTarget target)
        {
            if (GetDistance(transform) > 0)
            {
                dunk = transform.DOJump(nextpoint, jumpPower, 1, Duration/2, false).OnComplete(() =>
                {
                    IsFinished = true;
                }).SetEase(Ease.Linear).SetUpdate(true);
            }
        }
    }
}