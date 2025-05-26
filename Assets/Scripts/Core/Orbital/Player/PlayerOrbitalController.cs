using System;
using NaughtyAttributes;
using Orpheus.Core.FightSystem;
using Orpheus.Core.Rings;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Orpheus.Core.Orbital.Player
{
    public class PlayerOrbitalController : OrbitalController<PlayerOrbitalController>, IAbilityTarget, IAbilityCaster
    {
        public PlayerInput PlayerInput { get; private set; }
        [SerializeField, BoxGroup("Player")] private PlayerMovementState[] defaultStates;
        [SerializeField, BoxGroup("Player")] private Ring ring;
        
        //event 
        public static event Action OnPlayerDeath;
        
        protected override void Awake()
        {
            base.Awake();
            PlayerInput = GetComponent<PlayerInput>();
            SetRing(ring);
        }

        private void Start()
        {
            for (int i = 0; i < defaultStates.Length; i++)
            {
                AddState(defaultStates[i]);
            }
        }

        public Vector3 CastPoint { get; }
        public Vector3 CastDirection { get; }
        public TargetTeam Team { get; }
        public OrbitalStats Stats { get; }
        public Ring Ring { get; }
        
        public void ApplyStatus()
        {
            throw new System.NotImplementedException();
        }

        public void ApplyMovement()
        {
            throw new System.NotImplementedException();
        }

        public void OnDeath()
        {
            
        }
    }
}