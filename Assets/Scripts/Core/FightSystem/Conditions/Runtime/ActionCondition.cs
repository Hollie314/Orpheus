using System;
using Orpheus.Core.FightSystem.Conditions.Data;
using Orpheus.Core.FightSystem.Conditions.Interface;
using Orpheus.Core.FightSystem.Skills.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Orpheus.Core.FightSystem.Conditions
{
    public class ActionCondition : Condition<ActionConditionData>
    {
        public ActionCondition(Skill skill, ActionConditionData data) : base(skill, data)
        {
        }
        
        public override void Initialize()
        {
            switch (ConditionData.SkillIndex)
            {
                case 0 :  Skill.Caster.Skill1 += OnCondition;
                    break;
                case 1 :  Skill.Caster.Skill2 += OnCondition;
                    break;
            }
            IsReached = false;
        }

        public override void Dispose()
        {
            Skill.Caster.Skill1 -= OnCondition;
            Skill.Caster.Skill2 -= OnCondition;
        }

        public override void ResetCondition()
        {
            IsReached = false;
        }

        public void OnCondition(bool reached)
        {
            IsReached = reached;
            if (reached)
            {
                Skill.OnConditionReached();
            }
        }
    }
}