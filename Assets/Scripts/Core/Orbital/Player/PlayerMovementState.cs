using UnityEngine;

namespace Core.Player
{
    
    public abstract class PlayerMovementState : ScriptableObject, IOrbitalMovementState<PlayerOrbitalController>
    {
        public abstract int GetStatePriority(PlayerOrbitalController orbitalController);
        public abstract Vector2 GetVelocity(PlayerOrbitalController orbitalController);
        public abstract void OnEnter(PlayerOrbitalController orbitalController);
        public abstract void OnExit(PlayerOrbitalController orbitalController);
        public abstract void Initialize(PlayerOrbitalController orbitalController);
        public abstract void Dispose(PlayerOrbitalController orbitalController);
    }
}