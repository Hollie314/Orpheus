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
                case ActionConditionName.Death :  ConditionUser.Death += OnCondition;
                    break;
                case ActionConditionName.Chase :  ConditionUser.Chase += OnCondition;
                    break;
                case ActionConditionName.Attaque : ConditionUser.Attaque += OnCondition;
                    break;
            }
            IsReached = false;
        }

        public override void Dispose()
        {
            ConditionUser.Death -= OnCondition;
            ConditionUser.Chase -= OnCondition;
            ConditionUser.Attaque -= OnCondition;
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
                ConditionUser.SetAnimator(ConditionData.ActionConditionName.ToString());
                ConditionUser.OnConditionReached();
            }
        }
        
        public void OnCondition(bool reached, int index)
        {
            if (index == ConditionUser.AttaqueIndex)
            {
                IsReached = reached;
                if (reached)
                {
                    ConditionUser.SetAnimator(ConditionData.ActionConditionName.ToString());
                    ConditionUser.OnConditionReached();
                } 
            }
            
        }
    }
}