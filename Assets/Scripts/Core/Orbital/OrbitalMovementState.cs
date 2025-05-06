using UnityEngine;

namespace Orpheus.Core.Orbital
{
    public interface IOrbitalMovementState<in T> where T : OrbitalController<T>
    {
        public int GetStatePriority(T orbitalController);
        public Vector2 GetVelocity(T orbitalController, float deltaTime);
        public void OnEnter(T orbitalController);
        public void OnExit(T orbitalController);
        public void Initialize(T orbitalController);
        public void Dispose(T orbitalController);

        void PreUpdate(T orbitalController);
    }
}