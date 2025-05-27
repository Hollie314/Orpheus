using Orpheus.Core.FightSystem.Conditions;
using Orpheus.Core.FightSystem.Conditions.Data;
using Orpheus.Core.FightSystem.Skills.Runtime;
using UnityEngine;

namespace Orpheus.Core.FightSystem.Skills.Data
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "Orpheus/FightSystem/SkillData", order = 0)]
    public class SkillData : ScriptableObject
    {
        [field: SerializeField] public AbilityData[] AbilityDatas { get; private set; }
        [field: SerializeField] public ConditionData[] ConditionDatas { get; private set; }
        [field: SerializeField] public TimerConditionData CoolDown { get; private set; }
        
        public Skill GenerateAbility(IAbilityCaster caster) => new Skill(caster, this);
    }
}