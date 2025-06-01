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
        public ActionCondition(IConditionUser conditionUser, ActionConditionData data) : base(conditionUser, data)
        {
        }
        
        public override void Initialize()
        {
            switch (ConditionData.ActionConditionName)
            {
                case ActionConditionName.Skill1 :  ConditionUser.Skill1 += OnCondition;
                    break;
                case ActionConditionName.Skill2 :  ConditionUser.Skill2 += OnCondition;
                    break;
                case ActionConditionName.Death :  ConditionUser.Death += OnCondition;
                    break;
                case ActionConditionName.Chase :  ConditionUser.Chase += OnCondition;
                    break;
            }
            IsReached = false;
        }

        public override void Dispose()
        {
            ConditionUser.Skill1 -= OnCondition;
            ConditionUser.Skill2 -= OnCondition;
            ConditionUser.Death -= OnCondition;
            ConditionUser.Chase -= OnCondition;
        }

        public override void ResetCondition()
        {
            IsReached = false;
        }

        public void OnCondition(bool reached)
        {
            ConditionUser.SetAnimator(ConditionData.ActionConditionName.ToString());
            IsReached = reached;
            if (reached)
            {
                ConditionUser.OnConditionReached();
            }
        }
    }
}