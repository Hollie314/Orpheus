using System;
using System.Collections;
using System.Collections.Generic;

using NaughtyAttributes;
using Orpheus.Core.FightSystem;
using Orpheus.Core.FightSystem.Conditions;

using Orpheus.Core.FightSystem.Skills.Data;
using Orpheus.Core.FightSystem.Skills.Runtime;
using Orpheus.Core.Orbital.Player;
using Orpheus.Core.Orbital.Player.States.MovementState;

using UnityEngine;

namespace Orpheus.Core.Orbital.Entities
{
    public class AI_Entities : OrbitalController<AI_Entities>, IAbilityTarget, IAbilityCaster
    {

        [SerializeField, BoxGroup("Enemy")]
        private RangeTrigger rangeTrigger;
        [SerializeField, BoxGroup("Enemy")] private EntitiesMovementState[] defaultStates;
        [SerializeField, BoxGroup("Enemy")] private SkillData[] skillDatas;
        
        public Vector3 CastPoint { get;private set; }
        public Vector3 CastDirection { get;private set; }
        public TargetTeam Team { get; private set; }
        public IMovement CurrentMovement { get; private set;}
        public List<Skill> skills { get;private set; }
        public PlayerOrbitalController player { get;private set; }
        [field:SerializeField, BoxGroup("Enemy")]
        public Animator Animator { get; set; }
        public int AttaqueIndex { get;private set; }
        
        //Condition
        public event Action<bool> Death;
        public event Action<bool> Chase;
        public event Action<bool, int> Attaque;


        private bool IsDead;


        protected override void Awake()
        {
            base.Awake();
            Team = TargetTeam.Enemy;
            Direction = 1;
            skills = new List<Skill>();
            
        }
        private void Start()
        {
            for (int i = 0; i < defaultStates.Length; i++)
            {
                AddState(defaultStates[i]);
            }
            for (int i = 0; i < skillDatas.Length; i++)
            {
                AddSkill(skillDatas[i].GenerateAbility(this),i);
            }
        }

        private void OnEnable()
        {
            rangeTrigger.OnEnterRange += OnEnter;
            rangeTrigger.OnExitRange += OnExit;
            IsDead = false;
        }

        private void OnDisable()
        {
            rangeTrigger.OnEnterRange -= OnEnter;
            rangeTrigger.OnExitRange -= OnExit;
            
        }
        
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            CastPoint = transform.position;
            CastDirection = transform.forward;
            
            if (CurrentMovement != null)
            {
                CurrentMovement.ApplyMovement(this.transform,Time.deltaTime, this);
                if (CurrentMovement.IsFinished)
                {
                    CurrentMovement = null;
                }
            }

            //change direction if blocked by wall
            if (IsBlocked)
            {
                ChangeDirection();
            }
            
            SetAnimatorBool();

            if (player != null)
            {
                if (player.CurrentRing == CurrentRing)
                {
                    Chase?.Invoke(true);
                }
                else
                {
                    Chase?.Invoke(false); 
                }
            }
        }
        
        public void SetAnimatorTrigger(string triggerName)
        {
            Animator.SetTrigger(triggerName);
        }
        
        private void SetAnimatorBool()
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
        
        public void AddSkill(Skill skill, int index)
        {
            skills.Add(skill);
            skill.Initialize(index);
        }

        public void RemoveSkill(Skill skill)
        {
            skill.Dispose();
            skills.Remove(skill);
        }

        public Vector3 GetAim()
        {
            if (player)
            {
                return player.transform.position;
            }

            return this.transform.forward;
        }

        public float GetPlayerDirection()
        {
            if (player != null)
            {
                Vector3 center = CurrentRing.transform.position;
                float playerAngle =
                    OrbitalMath.GetAngleFromPosition(center, GetAim());
                float entitiAngle =
                    OrbitalMath.GetAngleFromPosition(center, transform.position);
                Direction = Mathf.Sign(playerAngle - entitiAngle);
            }
            return Direction;
        }

        public IAbilityTarget GetTarget()
        {
            if (player)
            {
                return player;
            }
            return null;
        }

        public void ApplyStatus()
        {
           
        }

        public void ApplyMovement(IMovement movement, float duration)
        {
            if (CurrentMovement == null)
            {
                IMovement movementClone = (IMovement)Instantiate((ScriptableObject)movement);
                movementClone.Initialize(this.transform, this, Direction*-1, duration);
                CurrentMovement = movementClone;
            }
        }

        private void Dispose()
        {
            for (int i = 0; i < skills.Count; i++)
            {
                RemoveSkill(skills[i]);
            }
            skills.Clear();
        }

        public void OnDeath(IAbilityCaster caster, DamageType damageType)
        {
            if (!IsDead)
            {
                Animator.SetTrigger("mort");
                IsDead = true;
                Death?.Invoke(true);
                StartCoroutine(CallAfterDelay(1));
            }
        }
        
        IEnumerator CallAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            GameManager.Instance.OnEnemyKilled(this);
        }

        private void OnEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerOrbitalController enteredTarget) && enteredTarget.CurrentRing == CurrentRing)
            {
                player = enteredTarget;
            }
        }

        private void OnExit(Collider other)
        {
            if (other.TryGetComponent(out PlayerOrbitalController exitedTarget) && exitedTarget.Equals(player))
            {
                player = null;
            }
        }

        public void ChangeDirection()=> Direction *= -1;

        public void Chasing()
        {
            float direction = 0;
            GetAim();
        }

        public void OnDestroy()
        {
            Dispose();
        }
        public Transform GetTransform()
        {
            return this.transform;
        }

        

        public void OnConditionReached()
        {
           
        }

        public void SetAnimator(string action)
        {
            
        }
    }
    
}