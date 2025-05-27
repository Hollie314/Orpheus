using Orpheus.Core.FightSystem.Runtime;
using UnityEngine;

namespace Orpheus.Core.FightSystem
{
    [CreateAssetMenu(fileName = "SelfAbilityData", menuName = "Orpheus/FightSystem/AbilityData/SelfAbility", order = 2)]
    public class SelfAbilityData : AbilityData
    {
        public override IAbility GenerateAbility(IAbilityCaster caster)=> new SelfAbility(caster, this);
    }
}