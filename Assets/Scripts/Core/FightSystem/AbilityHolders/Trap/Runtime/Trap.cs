using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Orpheus.Core.FightSystem.Skills.Runtime;
using Orpheus.Core.Rings;
using UnityEngine;
using UnityEngine.UIElements;

namespace Orpheus.Core.FightSystem.Trap
{
    [RequireComponent(typeof(CapsuleCollider))]
    public abstract class Trap : MonoBehaviour, IAbilityCaster
    {
        public Vector3 CastPoint { get;private set; }
        public virtual Vector3 CastDirection { get; private set; }
        public float Direction { get; }
        public TargetTeam Team { get; private set; } = TargetTeam.Trap;
        public Ring CurrentRing { get; private set; }
        public OrbitalStats Stats { get; }
        public List<Skill> skills { get;private set; }
        public event Action<bool> Skill1;
        public event Action<bool> Skill2;


        public AbilityData AbilityData { get; private set; }
        
        public bool IsActive { get; set; }
        [field: SerializeField] public float ActivationRate { get; private set; }
        private float currentTime;

        public void Initialize(Ring ring)
        {
            this.CurrentRing = ring;
            currentTime = 0;
            IsActive = false;
            skills = new List<Skill>();
        }

        public void Update()
        {
            //apply 
            if (IsActive)
            {
                if (currentTime >= ActivationRate || currentTime == 0)
                {
                    ActivateAbility(AbilityData.TotalLifetime);
                    currentTime = 0;
                }
                currentTime+= Time.deltaTime;
            }
            else
            {
                currentTime = 0;
            }
        }

        public IEnumerator ActivateAbility(float duration)
        {
            IAbility ability = this.AbilityData.GenerateAbility(this);
            AbilityManager.Instance.AddAbility(ability);
            yield return duration;
            AbilityManager.Instance.RemoveAbility(ability);
            
        }
        
        public void AddSkill(Skill skill)
        {
            throw new NotImplementedException();
        }

        public void RemoveSkill(Skill skill)
        {
            throw new NotImplementedException();
        }

        public Vector3 GetAim()
        {
            throw new NotImplementedException();
        }

        public IAbilityTarget GetTarget()
        {
            throw new NotImplementedException();
        }
    }
}