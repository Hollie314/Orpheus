using Orpheus.Core.FightSystem.Conditions.Interface;
using Orpheus.Core.FightSystem.Skills.Runtime;
using UnityEngine;

namespace Orpheus.Core.FightSystem.Conditions.Data
{
    public abstract class ConditionData : ScriptableObject
    {
        public abstract ICondition GenerateCondition(IConditionUser conditionUser);
    }
}