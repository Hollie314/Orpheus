using System;
using System.Collections.Generic;
using Orpheus.Core.FightSystem;
using Orpheus.Core.FightSystem.Conditions;
using Orpheus.Core.FightSystem.Skills.Runtime;
using Orpheus.Core.Rings;
using UnityEngine;

namespace Orpheus.Core
{
    public class Hole : MonoBehaviour, IAbilityCaster
    {
        [field:SerializeField]
        private RangeTrigger rangeTrigger;
        [field:SerializeField]
        public Animator animator { get;private set; }

        public Vector3 CastPoint { get; private set; }
        public Vector3 CastDirection { get;private set; }
        public float Direction { get;private set; }
        public TargetTeam Team { get;private set; }
        public event Action<bool> InRange;
        public Ring CurrentRing { get;private set; }
        public OrbitalStats Stats { get;private set; }
        public List<Skill> skills { get;private set; }
        
        public event Action<bool> Skill1;
        public event Action<bool> Skill2;
        public event Action<bool> Death;
       
        public void OnConditionReached()
        {
            
        }

        public void SetAnimator(string action)
        {
           
        }

      

        private void Awake()
        {
            Team = TargetTeam.Trap;
            Direction = 1;
            skills = new List<Skill>();
            rangeTrigger.OnEnterRange += OnEnter;
        }
        
        private void OnDisable()
        {
            rangeTrigger.OnEnterRange -= OnEnter;
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

        public Transform GetTransform()
        {
            return this.transform;
        }

        private void OnEnter(Collider other)
        {
            IAbilityTarget target = other.GetComponent<IAbilityTarget>();
            Debug.Log(other.gameObject.name);
            if (target!=null)
            {
                Debug.Log("whahaha");
                target.ApplyDamage(this,target.Stats.getStat(FloatStats.Hp),0,FloatStats.Power,DamageType.Fall);
            }
        }
    }
}
