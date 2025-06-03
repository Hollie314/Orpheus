using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Orpheus.Core.FightSystem;
using Orpheus.Core.FightSystem.Conditions.Interface;
using Orpheus.Core.FightSystem.Skills.Runtime;
using Orpheus.Core.Orbital.Player.States.MovementState;
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
        private bool IsDead;
        private bool IsJumping;
        
        //Caster and Target
        public Vector3 CastPoint { get; private set; }
        public Vector3 CastDirection { get; private set; }
        public TargetTeam Team { get; private set;}
        public IMovement CurrentMovement { get; private set;}
        public List<Skill> skills { get;private set; }
        [field:SerializeField, BoxGroup("Player")]
        public Animator Animator { get; set; }
        
        //Event for conditions
        public event Action<bool> Death;
        public event Action<bool> Chase;
        public event Action<bool, int> Attaque;

        protected override void Awake()
        {
            base.Awake();
            PlayerInput = GetComponent<PlayerInput>();
            SetRing(ring);
            Team = TargetTeam.Player;
            skills = new List<Skill>();
        }

        private void Start()
        {
            for (int i = 0; i < defaultStates.Length; i++)
            {
                AddState(defaultStates[i]);
            }
        }

        public void Init()
        {
            Stats.setStat(FloatStats.Hp, Stats.getStat(FloatStats.HpMax));
            IsDead = false;
            Animator.SetBool("IsDead",false);
        }

        protected override void FixedUpdate()
        {
          base.FixedUpdate();
          Transform transform1 = this.transform;
          CastPoint = transform1.position;
          CastDirection = transform1.forward;
          
          //apply movement from abilities (dunk, knock back... those are tween and can affect all orbital entities)
          if (CurrentMovement != null)
          {
              CurrentMovement.ApplyMovement(this.transform,Time.deltaTime, this);
              if (CurrentMovement.IsFinished)
              {
                  CurrentMovement = null;
              }
          }
          SetAnimatorTrigger();
        }


        private void SetAnimatorTrigger()
        {
            // idle or running
            if (CurrentVelocity.x != 0)
            {
                Animator.SetBool("IsRunning",true);
            }
            else
            {
                Animator.SetBool("IsRunning",false); 
            }
            Animator.SetBool("IsGrounded",IsGrounded); 
        }

        public void SetWeapon(int index)
        {
            Animator.SetInteger("weapon",index);
        }

        public void ApplyStatus()
        {
           
        }

        public void ApplyMovement(IMovement movement, float duration)
        {
            if (CurrentMovement == null)
            {
                movement.Initialize(this.transform, this, Direction, duration);
                CurrentMovement = movement;
            }
        }

        public void OnDeath(IAbilityCaster caster, DamageType damageType)
        {
            if (!IsDead)
            {
                IsDead = true;
                Animator.SetBool("IsDead",true);
                Death?.Invoke(true);
                StartCoroutine(CallAfterDelay(1,caster,damageType));
            }
        }
        
        IEnumerator CallAfterDelay(float delay,IAbilityCaster caster, DamageType damageType )
        {
            yield return new WaitForSeconds(delay);
            GameManager.Instance.OnPlayerDeath(damageType,caster.Team);
        }
        
        public void AddSkill(Skill skill, int index)
        {
            skills.Add(skill);
            skill.Initialize(index);
        }

        public void RemoveSkill(Skill skill)
        {
            skills.Remove(skill);
            skill.Dispose();
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

        public void SetAnimatorTrigger(string triggerName)
        {
            Animator.SetTrigger(triggerName);
        }

        public void OnSkill1(InputAction.CallbackContext obj)
        {
            Animator.SetInteger("abilityIndex",0);
            switch (obj.phase)
            {
                case InputActionPhase.Performed : 
                    Attaque?.Invoke(true, 0);
                    break;
                case InputActionPhase.Canceled : 
                    Attaque?.Invoke(false, 0);
                    break;
                default:
                    break;
            }
        }
        
        public void OnSkill2(InputAction.CallbackContext obj)
        {
            Animator.SetInteger("abilityIndex",1);
            switch (obj.phase)
            {
                case InputActionPhase.Performed :
                    Attaque?.Invoke(true, 1);
                    break;
                case InputActionPhase.Canceled : 
                    Attaque?.Invoke(false, 1);
                    break;
                default:
                    break;
            }
        }
        public Transform GetTransform()
        {
            return this.transform;
        }


        public int AttaqueIndex { get; }

        public void OnConditionReached()
        {
            
        }

        public void SetAnimator(string action)
        {
            
        }
    }
}