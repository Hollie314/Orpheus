using UnityEngine;

namespace Orpheus.Core.Orbital.Player.States.MovementState
{
    public class KnockBackState<T> : MovementState<T> where T : OrbitalController<T>
    {
        public override void OnExit(T orbitalController)
        {
            throw new System.NotImplementedException();
        }

        public override int GetStatePriority(T orbitalController)
        {
            return 50;
        }

        public override Vector2 GetVelocity(T orbitalController, float deltaTime)
        {
            throw new System.NotImplementedException();
        }

        public override void Initialize(T orbitalController)
        {
            throw new System.NotImplementedException();
        }

        public override void Dispose(T orbitalController)
        {
            throw new System.NotImplementedException();
        }

        public override void OnEnter(T orbitalController)
        {
            throw new System.NotImplementedException();
        }
    }
}