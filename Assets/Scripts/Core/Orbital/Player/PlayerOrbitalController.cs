using System;
using NaughtyAttributes;
using Orpheus.Core.FightSystem;
using Orpheus.Core.FightSystem.Skills.Runtime;
using Orpheus.Core.Rings;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Orpheus.Core.Orbital.Player
{
    public class PlayerOrbitalController : OrbitalController<PlayerOrbitalController>, IAbilityTarget, IAbilityCaster
    {
        public PlayerInput PlayerInput { get; private set; }
        [SerializeField, BoxGroup("Player")] private PlayerMovementState[] defaultStates;
        [SerializeField, BoxGroup("Player")] private Ring ring;
        
        //event 
        public static event Action OnPlayerDeath;
        
        //Caster and Target
        public Vector3 CastPoint { get; private set; }
        public Vector3 CastDirection { get; private set; }
        public TargetTeam Team { get; private set;}
        public event Action<bool> Skill1;
        public event Action<bool> Skill2;
        
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

        protected override void FixedUpdate()
        {
          base.FixedUpdate();
          CastPoint = this.transform.position;
        }


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

        public IAbilityTarget GetTarget()
        {
            //make a raycast
            return null;
        }

        public void OnSkill1(InputAction.CallbackContext obj)
        {
            switch (obj.phase)
            {
                case InputActionPhase.Performed : Skill1?.Invoke(true);
                    break;
                case InputActionPhase.Canceled : Skill1?.Invoke(false);
                    break;
                default:
                    break;
            }
        }
        
        public void OnSkill2(InputAction.CallbackContext obj)
        {
            switch (obj.phase)
            {
                case InputActionPhase.Performed : Skill2?.Invoke(true);
                    break;
                case InputActionPhase.Canceled : Skill2?.Invoke(false);
                    break;
                default:
                    break;
            }
        }
    }
}