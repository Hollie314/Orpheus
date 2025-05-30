using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Orpheus.Core.FightSystem;
using Orpheus.Core.FightSystem.Conditions;
using Orpheus.Core.FightSystem.Conditions.Data;
using Orpheus.Core.FightSystem.Skills.Runtime;
using Orpheus.Core.Orbital.Player.States.MovementState;
using Orpheus.Core.Rings;
using UnityEngine;

namespace Orpheus.Core.Orbital.Entities
{
    public class AI_Entities : OrbitalController<AI_Entities>, IAbilityTarget, IAbilityCaster
    {

        [SerializeField, BoxGroup("Enemy")] private float rangePlayerDetection;
        private RangeTrigger rangeTrigger;
        
        
        public Vector3 CastPoint { get;private set; }
        public Vector3 CastDirection { get;private set; }
        public TargetTeam Team { get; private set; }
        public event Action<bool> Skill1;
        public event Action<bool> Skill2;
        public IMovement CurrentMovement { get; private set;}


        private void Start()
        {
          
        }

        protected override void Awake()
        {
            base.Awake();
            Team = TargetTeam.Enemy;
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
        }
        
        public void AddSkill(Skill skill)
        {
            
        }

        public void RemoveSkill(Skill skill)
        {
            
        }

        public Vector3 GetAim()
        {
            return Vector3.zero;
        }

        public IAbilityTarget GetTarget()
        {
            return null;
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

        private void Dispose()
        {
        }

        public void OnDeath(IAbilityCaster caster, DamageType damageType)
        {
            Dispose();
            GameManager.Instance.OnEnemyKilled(this);
        }
    }
}