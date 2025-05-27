using UnityEngine;

namespace Orpheus.Core.Orbital.Player.States.MovementState
{
    [CreateAssetMenu(fileName = "KnockBackState", menuName = "Orpheus/Player/KnockBack", order = 3)]
    public class KnockBackState<T> : MovementState<T> where T : OrbitalController<T>
    {
        public override void OnExit(T orbitalController)
        {
            
        }

        public override int GetStatePriority(T orbitalController)
        {
            return 50;
        }

        public override Vector2 GetVelocity(T orbitalController, float deltaTime)
        {
            return Vector2.zero;
        }

        public override void Initialize(T orbitalController)
        {
            
        }

        public override void Dispose(T orbitalController)
        {
            
        }

        public override void OnEnter(T orbitalController)
        {
           
        }
    }
}