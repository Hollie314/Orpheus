using System.Collections.Generic;

namespace Orpheus.Core.FightSystem.Runtime
{
    public class SelfAbility : Ability<SelfAbilityData>
    {
        public SelfAbility(IAbilityCaster caster, SelfAbilityData data) : base(caster, data)
        {
        }

        protected override void GetTouchedTargets(List<IAbilityTarget> targets)
        {
            if (Caster is IAbilityTarget target)
                targets.Add(target);
        }
    }
}