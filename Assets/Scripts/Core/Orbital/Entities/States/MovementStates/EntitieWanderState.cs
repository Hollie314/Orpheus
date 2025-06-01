using UnityEngine;

namespace Orpheus.Core.Orbital.Entities.States.MovementStates
{
    [CreateAssetMenu(fileName = "EntitieWanderState", menuName = "Orpheus/Entitie/Wander", order = 2)]
    public class EntitieWanderState: EntitiesMovementState
    {
        
        public override void OnExit(AI_Entities orbitalController)
        {
            
        }

        public override int GetStatePriority(AI_Entities orbitalController)
        {
            return 10;
        }

        public override Vector2 GetVelocity(AI_Entities orbitalController, float deltaTime)
        {
            return new Vector2(orbitalController.Direction * orbitalController.Stats.getStat(FloatStats.Speed) *deltaTime, 0);
        }

        public override void Initialize(AI_Entities orbitalController)
        {
            
        }

        public override void Dispose(AI_Entities orbitalController)
        {
            
        }

        public override void OnEnter(AI_Entities orbitalController)
        {
            orbitalController.Animator.SetBool("IsRunning", true);
        }
    }
}