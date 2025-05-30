using Orpheus.Core.FightSystem;
using UnityEngine;

namespace Orpheus.Core.Orbital.Player.States.MovementState
{
    public interface IMovement
    {
        public bool IsFinished { get; }
        public void Initialize(Transform transform, IAbilityTarget target, float direction, float duration);
        public void ApplyMovement(Transform transform, float deltaTime, IAbilityTarget target);
        public void Dispose();

    }
}