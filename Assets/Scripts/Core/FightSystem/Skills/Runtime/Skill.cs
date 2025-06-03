using System;
using System.Collections.Generic;
using Orpheus.Core.FightSystem.Conditions;
using Orpheus.Core.FightSystem.Conditions.Data;
using Orpheus.Core.FightSystem.Conditions.Interface;
using Orpheus.Core.FightSystem.Skills.Data;
using Orpheus.Core.Rings;
using UnityEditor;
using UnityEngine;

namespace Orpheus.Core.FightSystem.Skills.Runtime
{
    public class Skill : IConditionUser
    {
        public IAbilityCaster Caster { get; private set; }
        public readonly SkillData Data;
        public readonly List<IAbility> abilities;
        private IAbility longestAbility;
        public readonly List<ICondition> conditions;
        private bool AbilitiesRunning;
        public TimerCondition Cooldown { get; private set; }
        
        public event Action<bool> Skill1;
        public event Action<bool> Skill2;
        public event Action<bool> Death;
        public event Action<bool> Chase;
        public event Action<bool, int> Attaque;
        
        public int AttaqueIndex { get; private set; }


        public Skill(IAbilityCaster caster, SkillData data)
        {
            Caster = caster;
            Data = data;
            abilities = new List<IAbility>();
            conditions = new List<ICondition>();
            //event relay
            Caster.Death += OnDeath;
            Caster.Chase += OnChase;
            Caster.Attaque += OnAttaque;
        }

        public void Initialize(int attaqueIndex)
        {
            AttaqueIndex = attaqueIndex;
            //Init all abilities and conditions
            foreach (var abilityData in Data.AbilityDatas)
            {
                IAbility ability = abilityData.GenerateAbility(Caster);
                abilities.Add(ability);
            }
            foreach (var conditionData in Data.ConditionDatas)
            {
                ICondition condition = conditionData.GenerateCondition(this);
                conditions.Add(condition);
                condition.Initialize();
            }
            Cooldown = (TimerCondition)Data.CoolDown.GenerateCondition(this);
            Cooldown.Initialize();
            
            //set cd to 0
            Cooldown.LowerCurrentTime(Cooldown.CurrentTime); 
            AbilitiesRunning = false;
            
            //set skill global duration to the longest ability duration
            longestAbility = GetLongestAbility();
            longestAbility.OnEnd += OnAbilityEnd;
        }
        

        public void Dispose()
        {
            //event clear
            longestAbility.OnEnd -= OnAbilityEnd;
            Caster.Death -= OnDeath;
            Caster.Chase -= OnChase;
            Caster.Attaque -= OnAttaque;
            
            foreach (var condition in conditions)
            {
                condition.Dispose();
            }

            foreach (var ability in abilities)
            {
                ability.Dispose();
            }

            if (Cooldown != null)
            {
                Cooldown.Dispose();
            }
            abilities.Clear();
            conditions.Clear();
        }
        
        private void OnSkill1(bool value)
        {
            Skill1?.Invoke(value);
        }
        
        private void OnSkill2(bool value)
        {
            Skill2?.Invoke(value);
        }
        
        private void OnDeath(bool value)
        {
            Death?.Invoke(value);
        }
        
        private void OnChase(bool value)
        {
            Chase?.Invoke(value);
        }
        
        private void OnAttaque(bool value, int index)
        {
            Attaque?.Invoke(value, index);
        }

        public IAbility GetLongestAbility()
        {
            IAbility longestAbility = null;
            foreach (var ability in abilities)
            {
                if (longestAbility != null)
                {
                    if (longestAbility.GetLifeTime() < ability.GetLifeTime())
                    {
                        longestAbility = ability;
                    }
                }
                else
                {
                    longestAbility = ability;
                }
            }
            return longestAbility;
        }

       

        public void OnConditionReached()
        {
            //Use Ability if all Condition are met
            if (Caster != null && AllConditionMeet()&& !AbilitiesRunning)
            {
                ResetCondition();
                foreach (var ability in abilities)
                {
                    AbilityManager.Instance.AddAbility(ability);
                }
                AbilitiesRunning = true;
                Caster.SetAnimatorTrigger("attaque");
            }
        }

        public void SetAnimator(string action)
        {
            
        }

        public Transform GetTransform()
        {
            return Caster.GetTransform();
        }

        public bool AllConditionMeet()
        {
            foreach (var condition in conditions)
            {
                if (!condition.IsReached)
                {
                    return false;
                }
            }
            if (!Cooldown.IsReached)
            {
                return false;
            }
            
            return true;
        }

        public void ResetCondition()
        {
            foreach (var condition in conditions)
            {
                condition.ResetCondition();
            }
        }
        
        public void CoolDownReduction(float reduction)
        {
            //apply cooldownreduction
        }

        public void OnAbilityEnd()
        {
            Cooldown.ResetCondition();
            AbilitiesRunning = false;
        }
    }
}