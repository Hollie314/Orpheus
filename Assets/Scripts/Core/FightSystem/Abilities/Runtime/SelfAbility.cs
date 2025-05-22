using System.Collections.Generic;

namespace Orpheus.Core.FightSystem.Runtime
{
    public class SelfAbility : Ability<LaserAbilityData>
    {
        public SelfAbility(IAbilityCaster caster, LaserAbilityData data) : base(caster, data)
        {
        }

        protected override void GetTouchedTargets(List<IAbilityTarget> targets)
        {
            if (Caster is IAbilityTarget target)
                targets.Add(target);
        }
    }
}