using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Orpheus.Core.FightSystem;
using Orpheus.Core.FightSystem.Conditions;
using Orpheus.Core.FightSystem.Conditions.Data;
using Orpheus.Core.FightSystem.Skills.Data;
using Orpheus.Core.FightSystem.Skills.Runtime;
using Orpheus.Core.Orbital.Player;
using Orpheus.Core.Orbital.Player.States.MovementState;
using Orpheus.Core.Rings;
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
        public event Action<bool> Skill1;
        public event Action<bool> Skill2;
        public IMovement CurrentMovement { get; private set;}
        public List<Skill> skills { get;private set; }
        public PlayerOrbitalController player { get;private set; }


        private void Start()
        {
            for (int i = 0; i < defaultStates.Length; i++)
            {
                AddState(defaultStates[i]);
            }
            for (int i = 0; i < skillDatas.Length; i++)
            {
                AddSkill(skillDatas[i].GenerateAbility(this));
            }
        }

        protected override void Awake()
        {
            base.Awake();
            Team = TargetTeam.Enemy;
            Direction = 1;
            skills = new List<Skill>();
            rangeTrigger.OnEnterRange += OnEnter;
            rangeTrigger.OnExitRange += OnExit;
        }
        
        private void OnDisable()
        {
            rangeTrigger.OnEnterRange -= OnEnter;
            rangeTrigger.OnExitRange -= OnExit;
        }
        
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            Transform transform1 = this.transform;
            CastPoint = transform1.position;
            CastDirection = transform1.forward;
            
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
            
        }
        
        public void AddSkill(Skill skill)
        {
            skills.Add(skill);
        }

        public void RemoveSkill(Skill skill)
        {
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
                movement.Initialize(this.transform, this, Direction*-1, duration);
                CurrentMovement = movement;
            }
        }

        private void Dispose()
        {
            foreach (var skill in skills)
            {
                skill.Dispose();
            }
            skills.Clear();
        }

        public void OnDeath(IAbilityCaster caster, DamageType damageType)
        {
            Dispose();
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
    }
    
}