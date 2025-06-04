using System;
using Orpheus.Core.FightSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Orpheus.Core.Orbital.Player.States
{
    [CreateAssetMenu(menuName = "Create PlayerMobileAbility", fileName = "PlayerMobileAbility", order = 0)]
    public abstract class PlayerMobileAbility<T> : PlayerMovementState where T : AbilityData
    {
        [SerializeField] public PlayerMovementState PlayerMovementState { get; private set; }
        [SerializeField] public String AbilityInputName { get; private set; }
        [SerializeField] public T Data { get; private set; }
        
        private InputAction abilityInput;
        
        public override void OnEnter(PlayerOrbitalController orbitalController)
        {
            PlayerMovementState.OnEnter(orbitalController);
        }
        public override void OnExit(PlayerOrbitalController orbitalController)
        {
            PlayerMovementState.OnExit(orbitalController);
        }
        
        public override void PreUpdate(PlayerOrbitalController orbitalController)
        {
            base.PreUpdate(orbitalController);
            PlayerMovementState = (PlayerMovementState)orbitalController.currentMovementState;
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
            abilityInput = null;
        }

        private void OnAbilityPerformed(InputAction.CallbackContext obj)
        {
            
        }
    }
}