using NaughtyAttributes;
using Orpheus.Core.FightSystem.Conditions.Interface;
using Orpheus.Core.FightSystem.Skills.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Orpheus.Core.FightSystem.Conditions.Data
{
    [CreateAssetMenu(fileName = "ActionConditionData", menuName = "Orpheus/FightSystem/Conditions/ActionCondition", order = 1)]
    public class ActionConditionData : ConditionData
    {
        [field: SerializeField, Min(0), MaxValue(2)]
        public ActionConditionName ActionConditionName { get; private set; }
        public override ICondition GenerateCondition(IConditionUser conditionUser)=> new ActionCondition(conditionUser, this);
    }
}