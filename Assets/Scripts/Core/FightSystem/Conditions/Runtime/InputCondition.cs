using System;
using Orpheus.Core.FightSystem.Conditions.Data;
using Orpheus.Core.FightSystem.Conditions.Interface;
using Orpheus.Core.FightSystem.Skills.Runtime;
using UnityEngine.InputSystem;

namespace Orpheus.Core.FightSystem.Conditions
{
    public class InputCondition : ICondition<InputConditionData>
    {
        public bool IsReached { get;private set; }
        public InputConditionData ConditionData { get; private set; }
        public Skill Skill { get;private set;  }
        
        public InputCondition(Skill skill, InputConditionData data)
        {
            ConditionData = data;
            Skill = skill;
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
        
        public void ResetCondition()
        {
            IsReached = false;
        }

        public void OnConditionTriggered(InputAction.CallbackContext obj)
        {
            if (obj.phase == InputActionPhase.Started)
            {
                IsReached = true;
                Skill.OnConditionReached();
            }

            if (obj.phase == InputActionPhase.Canceled || obj.action.WasReleasedThisFrame())
            {
                IsReached = false;
            }
        }
    }
}