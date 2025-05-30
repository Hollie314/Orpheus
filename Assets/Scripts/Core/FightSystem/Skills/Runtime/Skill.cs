using System;
using System.Collections.Generic;
using Orpheus.Core.FightSystem.Conditions;
using Orpheus.Core.FightSystem.Conditions.Data;
using Orpheus.Core.FightSystem.Conditions.Interface;
using Orpheus.Core.FightSystem.Skills.Data;
using UnityEditor;
using UnityEngine;

namespace Orpheus.Core.FightSystem.Skills.Runtime
{
    public class Skill
    {
        public IAbilityCaster Caster { get; private set; }
        public readonly SkillData Data;
        public readonly List<IAbility> abilities;
        public readonly List<ICondition> conditions;
        private bool AbilitiesRunning;
        public TimerCondition Cooldown { get; private set; }
        
        public Skill(IAbilityCaster caster, SkillData data)
        {
            Caster = caster;
            Data = data;
            abilities = new List<IAbility>();
            conditions = new List<ICondition>();
            Initialize();
        }

        public void Initialize()
        {
            foreach (var abilityData in Data.AbilityDatas)
            {
                IAbility ability = abilityData.GenerateAbility(Caster);
                abilities.Add(ability);
                //abilities.Init();
            }
            foreach (var conditionData in Data.ConditionDatas)
            {
                ICondition condition = conditionData.GenerateCondition(this);
                conditions.Add(condition);
                condition.Initialize();
            }
            Cooldown = (TimerCondition)Data.CoolDown.GenerateCondition(this);
            Cooldown.Initialize();
            Cooldown.LowerCurrentTime(Cooldown.CurrentTime); //set cd to 0 
            GetLongestAbility().OnEnd += OnAbilityEnd;
        }

        public void Dispose()
        {
            GetLongestAbility().OnEnd -= OnAbilityEnd;
            foreach (var condition in conditions)
            {
                condition.Dispose();
            }

            foreach (var ability in abilities)
            {
                ability.Dispose();
            }
            Cooldown.Dispose();
            abilities.Clear();
            conditions.Clear();
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
                foreach (var ability in abilities)
                {
                    AbilityManager.Instance.AddAbility(ability);
                }
                AbilitiesRunning = true;
            }
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
            //If all condition are met it reset them 
            ResetCondition();
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