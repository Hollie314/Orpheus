using System;
using NaughtyAttributes;
using Orpheus.Core.FightSystem;
using Orpheus.Core.FightSystem.Skills.Runtime;
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
           
        }

        public void ApplyMovement()
        {
            
        }

        public void OnDeath()
        {
            
        }
        
        public void AddSkill(Skill skill)
        {
            
        }

        public void RemoveSkill(Skill skill)
        {
           
        }

        public Vector3 GetAim()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = PlayerCamera.distance;
            return Camera.main.ScreenToWorldPoint(mousePos);
        }
    }
}