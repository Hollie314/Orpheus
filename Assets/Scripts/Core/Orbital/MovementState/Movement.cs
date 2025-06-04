using DG.Tweening;
using NaughtyAttributes;
using Orpheus.Core.FightSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Orpheus.Core.Orbital.Player.States.MovementState
{
    public abstract class Movement :  ScriptableObject, IMovement
    {
        protected Vector3 nextpoint;
        public bool IsFinished { get; protected set; }
        public float Duration { get; protected set; }
        
        public virtual void Initialize(Transform transform, IAbilityTarget target, float direction, float duration)
        {
            
        }
        
        public virtual void Dispose()
        {
            
        }
        public abstract void ApplyMovement(Transform transform, float deltaTime, IAbilityTarget target);
        
        protected float GetDistance(Transform transform)
        {
            return (transform.position - nextpoint).magnitude;
        }
    }
}