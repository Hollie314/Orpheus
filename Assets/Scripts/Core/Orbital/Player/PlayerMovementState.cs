using UnityEngine;

namespace Orpheus.Core.Orbital.Player
{
    
    public abstract class PlayerMovementState : ScriptableObject, IOrbitalMovementState<PlayerOrbitalController>
    {
        public int StatePriority { get; private set; }
        public abstract void Initialize(PlayerOrbitalController orbitalController);
        public abstract void Dispose(PlayerOrbitalController orbitalController);
        public abstract void OnEnter(PlayerOrbitalController orbitalController);
        public abstract void OnExit(PlayerOrbitalController orbitalController);
        public virtual void PreUpdate(PlayerOrbitalController orbitalController) { }
        public abstract int GetStatePriority(PlayerOrbitalController orbitalController);
        public abstract Vector2 GetVelocity(PlayerOrbitalController orbitalController, float deltaTime);
    }
}