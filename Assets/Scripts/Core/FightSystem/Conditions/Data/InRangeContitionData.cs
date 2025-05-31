using Orpheus.Core.FightSystem.Conditions.Interface;
using Orpheus.Core.FightSystem.Skills.Runtime;
using UnityEngine;

namespace Orpheus.Core.FightSystem.Conditions.Data
{
    [CreateAssetMenu(fileName = "InRangeConditionData", menuName = "Orpheus/FightSystem/Conditions/InRangeCondition", order = 3)]
    public class InRangeConditionData : ConditionData
    {
        [field: SerializeField]
        public float Range { get; private set; }
        [field: SerializeField]
        public TargetTeam TeamInRange { get; private set; }
        public override ICondition GenerateCondition(IConditionUser conditionUser) => new InRangeCondition(conditionUser, this);
    }
}