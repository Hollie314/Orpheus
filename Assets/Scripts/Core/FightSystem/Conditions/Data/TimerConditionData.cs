using System;
using Orpheus.Core.FightSystem.Conditions.Interface;
using Orpheus.Core.FightSystem.Skills.Runtime;
using UnityEngine;

namespace Orpheus.Core.FightSystem.Conditions.Data
{
    [CreateAssetMenu(fileName = "TimerConditionData", menuName = "Orpheus/FightSystem/Conditions/TimerCondition", order = 0)]
    public class TimerConditionData : ConditionData
    {
        [SerializeField]
        public float Duration { get; private set; }
        public override ICondition<ConditionData> GenerateCondition(Skill skill) => (ICondition<ConditionData>)new TimerCondition(skill, this);
    }
}