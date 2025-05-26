using System;
using Orpheus.Core.FightSystem.Conditions.Data;
using Orpheus.Core.FightSystem.Conditions.Interface;
using Orpheus.Core.FightSystem.Skills.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Orpheus.Core.FightSystem.Conditions
{
    public class InputCondition : Condition<InputConditionData>
    {
        public InputCondition(Skill skill, InputConditionData data) : base(skill, data)
        {
        }
        
        public void Initialize()
        {
            ConditionData.InputAction.performed += OnConditionTriggered;
            IsReached = false;
        }

        public void Dispose()
        {
            ConditionData.InputAction.performed -= OnConditionTriggered;
        }

        protected override void InitializeCondition()
        {
            
        }

        public void ResetCondition()
        {
            IsReached = false;
        }

        public void OnConditionTriggered(InputAction.CallbackContext obj)
        {
            Debug.Log("we are triggerring some condition with trigger");
            if (obj.phase == InputActionPhase.Started)
            {
                Debug.Log("start input");
                IsReached = true;
                Skill.OnConditionReached();
                return;
            }

            if (obj.phase == InputActionPhase.Canceled || obj.action.WasReleasedThisFrame())
            {
                IsReached = false;
            }
        }
    }
}