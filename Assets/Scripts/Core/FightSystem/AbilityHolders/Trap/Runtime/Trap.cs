using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NaughtyAttributes;
using Orpheus.Core.FightSystem.Conditions;
using Orpheus.Core.FightSystem.Skills.Data;
using Orpheus.Core.FightSystem.Skills.Runtime;
using Orpheus.Core.Rings;
using UnityEngine;
using UnityEngine.UIElements;

namespace Orpheus.Core.FightSystem.Trap
{
    public abstract class Trap : MonoBehaviour, IAbilityCaster
    {

        [SerializeField, BoxGroup("Trap")] private OrbitalStatsData statsData;
        [SerializeField, BoxGroup("Trap")] private SkillData[] skillDatas;
        [SerializeField, BoxGroup("Trap")] private RangeTrigger rangeTrigger;
        
        public Vector3 CastPoint { get;private set; }
        public Vector3 CastDirection { get; protected set; }
        public float Direction { get;private set; }
        public TargetTeam Team { get; private set; } = TargetTeam.Trap;
        public event Action<bool> Chase;
        public event Action<bool, int> Attaque;
        public Ring CurrentRing { get; private set; }
        

        
        public OrbitalStats Stats { get;private set; }
        public List<Skill> skills { get;private set; }
        public event Action<bool> Skill1;
        public event Action<bool> Skill2;
        public event Action<bool> Death;

        [field:SerializeField, BoxGroup("Trap")]
        public Animator Animator { get; set; }
        public bool IsActive { get; set; }

        private List<IAbilityTarget> allTarget;
        

        public void Initialize(Ring ring)
        {
            this.CurrentRing = ring;
            IsActive = false;
            skills = new List<Skill>();
            allTarget = new List<IAbilityTarget>();
            Stats = new OrbitalStats();
            Stats.Initialize(statsData, 1);
        }

        protected virtual void Start()
        {
            for (int i = 0; i < skillDatas.Length; i++)
            {
                AddSkill(skillDatas[i].GenerateAbility(this),i);
            }
            
            rangeTrigger.OnEnterRange += OnEnter;
            rangeTrigger.OnExitRange += OnExit;

            CastPoint = transform.position;
            SetCastDirection();
        }

        public void Update()
        {
           
        }

        private void OnDestroy()
        {
            rangeTrigger.OnEnterRange -= OnEnter;
            rangeTrigger.OnExitRange -= OnExit;
        }

        protected virtual void SetCastDirection()
        {
            CastDirection = transform.forward;
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

        public int AttaqueIndex { get; }

        public void OnConditionReached()
        {
            
        }

        public void SetAnimator(string action)
        {
            
        }
        
        public void SetAnimatorTrigger(string triggerName)
        {
            
        }
        
        private void OnEnter(Collider other)
        {
            if (other.TryGetComponent(out IAbilityTarget enteredTarget) && enteredTarget.CurrentRing == CurrentRing)
            {
                allTarget.Add(enteredTarget);
                Attaque?.Invoke(true,0);
            }
        }

        private void OnExit(Collider other)
        {
            if (other.TryGetComponent(out IAbilityTarget exitedTarget) && allTarget.Contains(exitedTarget))
            {
                allTarget.Remove(exitedTarget);
                if (allTarget.Count == 0)
                {
                    Attaque?.Invoke(false,0);
                }
            }
        }
    }
}