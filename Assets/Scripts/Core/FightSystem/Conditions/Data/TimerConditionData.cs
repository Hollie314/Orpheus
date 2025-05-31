using System;
using Orpheus.Core.FightSystem.Conditions.Interface;
using Orpheus.Core.FightSystem.Skills.Runtime;
using UnityEngine;

namespace Orpheus.Core.FightSystem.Conditions.Data
{
    [CreateAssetMenu(fileName = "TimerConditionData", menuName = "Orpheus/FightSystem/Conditions/TimerCondition", order = 0)]
    public class TimerConditionData : ConditionData
    {
        [field: SerializeField]
        public float Duration { get; private set; }
        public override ICondition GenerateCondition(IConditionUser conditionUser) => new TimerCondition(conditionUser, this);
    }
}