using Orpheus.Core.FightSystem;
using UnityEngine;

namespace Orpheus.Core.Orbital.Player.States.MovementState
{
    public interface IMovement
    {
        public void Initialize(Transform transform, IAbilityTarget target, Vector3 direction);
        public void ApplyMovement(Transform transform, float deltaTime);
        public void Dispose();

    }
}