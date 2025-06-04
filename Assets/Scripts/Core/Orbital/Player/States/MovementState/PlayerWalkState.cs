using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Orpheus.Core.Orbital.Player
{
    [CreateAssetMenu(fileName = "walkState", menuName = "Orpheus/Player/Walk", order = 0)]
    public class PlayerWalkState : PlayerControlledMovementState
    {
        public override int GetStatePriority(PlayerOrbitalController orbitalController)
        {
            return orbitalController.IsGrounded ? 1 : -1;
        }

        public override void OnEnter(PlayerOrbitalController orbitalController)
        {
           
        }

        public override void OnExit(PlayerOrbitalController orbitalController)
        {
            
        }
    }
}