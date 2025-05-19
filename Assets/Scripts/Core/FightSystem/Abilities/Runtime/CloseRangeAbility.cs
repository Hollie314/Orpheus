using System.Collections.Generic;
using UnityEngine;

namespace Orpheus.Core.FightSystem.Runtime
{
    public class CloseRangeAbility : Ability<CloseRangeAbilityData>
    {
        public CloseRangeAbility(IAbilityCaster caster, CloseRangeAbilityData data) : base(caster, data)
        {
        }

        protected override void GetTouchedTargets(List<IAbilityTarget> targets)
        {
            Vector3 p1 = Caster.CastPoint + Vector3.up * Data.HitBoxHeight / 2;
            Vector3 p2 = Caster.CastPoint - Vector3.up * Data.HitBoxHeight / 2;

            int count = Physics.OverlapCapsuleNonAlloc(p1, p2, Data.HitBoxRadius, ColliderBuffer);
            TryAddTargets(ColliderBuffer, count, targets);
        }
    }
}