using UnityEngine;

namespace Core
{
    public abstract class OrbitalMovementState : ScriptableObject
    {
        public abstract int GetStatePriority(OrbitalController orbitalController);
        public abstract Vector2 GetVelocity(OrbitalController orbitalController);
        public abstract void OnEnter(OrbitalController orbitalController);
        public abstract void OnExit(OrbitalController orbitalController);
    }
}