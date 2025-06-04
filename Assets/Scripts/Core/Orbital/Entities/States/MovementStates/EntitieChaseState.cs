using UnityEngine;

namespace Orpheus.Core.Orbital.Entities.States.MovementStates
{
    [CreateAssetMenu(fileName = "EntitieChaseState", menuName = "Orpheus/Entitie/Chase", order = 0)]
    public class EntitieChaseState : EntitiesMovementState
    {
        public override void OnExit(AI_Entities orbitalController)
        {
            
        }

        public override int GetStatePriority(AI_Entities orbitalController)
        {
            if (orbitalController.player != null)
            {
                return 20;
            }
            return -1;
        }

        public override Vector2 GetVelocity(AI_Entities orbitalController, float deltaTime)
        {
            return new Vector2(orbitalController.GetPlayerDirection() * orbitalController.Stats.getStat(FloatStats.Speed)*deltaTime, 0);
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

        public override void PreUpdate(AI_Entities orbitalController)
        {
            base.PreUpdate(orbitalController);
            
        }
    }
}