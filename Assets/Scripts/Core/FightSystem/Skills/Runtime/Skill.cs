using System;
using System.Collections.Generic;
using Orpheus.Core.FightSystem.Conditions;
using Orpheus.Core.FightSystem.Conditions.Data;
using Orpheus.Core.FightSystem.Conditions.Interface;
using Orpheus.Core.FightSystem.Skills.Data;

namespace Orpheus.Core.FightSystem.Skills.Runtime
{
    public class Skill
    {
        public IAbilityCaster Caster { get; private set; }
        public readonly SkillData Data;
        public readonly IAbility ability;
        public readonly List<ICondition<ConditionData>> conditions;
        public readonly TimerCondition cooldown;
        
        public Skill(IAbilityCaster caster, SkillData data)
        {
            Caster = caster;
            Data = data;
            ability = data.AbilityData.GenerateAbility(caster);
            conditions = new List<ICondition<ConditionData>>();
            cooldown = (TimerCondition)data.CoolDown.GenerateCondition(this);
            Initialize();
        }

        public void Initialize()
        {
            foreach (var conditionData in Data.ConditionDatas)
            {
                ICondition<ConditionData> condition = conditionData.GenerateCondition(this);
                conditions.Add(condition);
                condition.Initialize();
            }
            cooldown.Initialize();
            CurrentCoolDownReduction(cooldown.CurrentTime); //set cd to 0 
        }

        public void Dispose()
        {
            foreach (var conditionData in conditions)
            {
                conditionData.Dispose();
            }
            cooldown.Dispose();
        }

        public void UpdateSkill()
        {
            //Use Ability if all Condition are met
            if (Caster != null && AllConditionMeet())
            {
                AbilityManager.Instance.AddAbility(ability);
            }
        }

        public void OnConditionReached()
        {
            if (Caster != null && AllConditionMeet())
            {
                AbilityManager.Instance.AddAbility(ability);
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

        public void CurrentCoolDownReduction(float reduction)
        {
            cooldown.LowerCurrentTime(reduction);
        }
        
        public void CoolDownReduction(float reduction)
        {
            //apply cooldownreduction
        }
    }
}