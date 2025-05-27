using Orpheus.Core.Orbital.Player;
using UnityEngine;

namespace Orpheus.Core.Orbital
{
    public abstract class MovementState<T> : ScriptableObject, IOrbitalMovementState<T> where T : OrbitalController<T>
        {
            public int StatePriority { get; private set; }
            public abstract void Initialize(T orbitalController);
            public abstract void Dispose(T orbitalController);
            public abstract void OnEnter(T orbitalController);
            public abstract void OnExit(T orbitalController);
            public virtual void PreUpdate(T orbitalController) { }
            public abstract int GetStatePriority(T orbitalController);
            public abstract Vector2 GetVelocity(T orbitalController, float deltaTime);
        }
    
}