using Orpheus.Core.FightSystem.Conditions.Interface;
using Orpheus.Core.FightSystem.Skills.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Orpheus.Core.FightSystem.Conditions.Data
{
    [CreateAssetMenu(fileName = "InputConditionData", menuName = "Orpheus/FightSystem/Conditions/InputCondition", order = 1)]
    public class InputConditionData : ConditionData
    {

        [field: SerializeField]
        public InputAction InputAction { get; private set; }
        public override ICondition GenerateCondition(Skill skill)=> new InputCondition(skill, this);
    }
}