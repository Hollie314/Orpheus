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
        [SerializeField, BoxGroup("Dunk")] private float duration;
        private Tween dunk;
        private Vector3 nextpoint;
        
        public override void Initialize(Transform transform, IAbilityTarget target, Vector3 direction)
        {
            Vector2 angularVelocity = new Vector2(target.CurrentRing.GetAngularSpeed(direction.x),0) * Time.deltaTime;
            nextpoint = target.CurrentRing.GetPositionOnRing(transform.position, angularVelocity);
            nextpoint = OrbitalMath.ClampToRing(nextpoint, target.CurrentRing.transform.position, target.CurrentRing.RingData.Radius);
        }

        public override void ApplyMovement(Transform transform, float deltaTime, IAbilityTarget target)
        {
            Debug.Log(nextpoint);
            if (GetDistance(transform) > 0)
            {
                dunk = transform.DOJump(nextpoint, jumpPower, 1, duration, false) .OnComplete(() =>
                {
                    Debug.Log("we have finished it i guess");
                });
            }
        }
    }
}