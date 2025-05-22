using Orpheus.Core.FightSystem.Runtime;

namespace Orpheus.Core.FightSystem
{
    public class SelfAbilityData : AbilityData
    {
        public override IAbility GenerateAbility(IAbilityCaster caster)=> new SelfAbility(caster, this);
    }
}