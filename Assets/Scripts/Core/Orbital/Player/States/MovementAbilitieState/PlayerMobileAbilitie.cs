

using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Orpheus.Core.Orbital.Player.States
{
    [CreateAssetMenu(menuName = "Create PlayerMobileAbility", fileName = "PlayerMobileAbility", order = 0)]
    public abstract class PlayerMobileAbility : PlayerMovementState
    {
        
        [SerializeField] public PlayerMovementState PlayerMovementState { get; private set; }
        [SerializeField] public String AbilityInputName { get; private set; }
        
        private InputAction abilityInput;
        
        public override void OnEnter(PlayerOrbitalController orbitalController)
        {
            PlayerMovementState.OnEnter(orbitalController);
        }
        public override void OnExit(PlayerOrbitalController orbitalController)
        {
            PlayerMovementState.OnExit(orbitalController);
        }

        public override int GetStatePriority(PlayerOrbitalController orbitalController)
        {
            return -1;
        }

        public override Vector2 GetVelocity(PlayerOrbitalController orbitalController, float deltaTime)
        {
            return PlayerMovementState.GetVelocity(orbitalController, deltaTime);
        }

        public override void Initialize(PlayerOrbitalController orbitalController)
        {
            PlayerMovementState.Initialize(orbitalController);
            abilityInput = orbitalController.PlayerInput.actions.FindActionMap("Player").FindAction(AbilityInputName);
            abilityInput.performed += OnAbilityPerformed;
        }

        public override void Dispose(PlayerOrbitalController orbitalController)
        {
            PlayerMovementState.Dispose(orbitalController);
        }

        private void OnAbilityPerformed(InputAction.CallbackContext obj)
        {
            
        }

       
    }
}