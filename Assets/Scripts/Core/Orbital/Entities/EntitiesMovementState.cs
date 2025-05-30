using Orpheus.Core.Orbital.Player;
using UnityEngine;

namespace Orpheus.Core.Orbital.Entities
{
    public abstract class EntitiesMovementState : ScriptableObject, IOrbitalMovementState<AI_Entities>
    {
    public int StatePriority { get; private set; }
    public abstract void Initialize(AI_Entities orbitalController);
    public abstract void Dispose(AI_Entities orbitalController);
    public abstract void OnEnter(AI_Entities orbitalController);
    public abstract void OnExit(AI_Entities orbitalController);
    public virtual void PreUpdate(AI_Entities orbitalController) { }
    public abstract int GetStatePriority(AI_Entities orbitalController);
    public abstract Vector2 GetVelocity(AI_Entities orbitalController, float deltaTime);
    }
}