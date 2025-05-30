using UnityEngine;

namespace Orpheus.Core.Orbital.Entities.States.MovementStates
{
    [CreateAssetMenu(fileName = "EntitieIdleState", menuName = "Orpheus/Entitie/Idle", order = 0)]
    public class EntitieIdleState : EntitiesMovementState
    {
        public override void OnExit(AI_Entities orbitalController)
        {
            
        }

        public override int GetStatePriority(AI_Entities orbitalController)
        {
            return 20;
        }

        public override Vector2 GetVelocity(AI_Entities orbitalController, float deltaTime)
        {
            return Vector2.zero;
        }

        public override void Initialize(AI_Entities orbitalController)
        {
            
        }

        public override void Dispose(AI_Entities orbitalController)
        {
            
        }

        public override void OnEnter(AI_Entities orbitalController)
        {
            
        }
    }
}