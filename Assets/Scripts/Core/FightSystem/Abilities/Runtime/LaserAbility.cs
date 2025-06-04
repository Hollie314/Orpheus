using System.Collections.Generic;
using UnityEngine;

namespace Orpheus.Core.FightSystem.Runtime
{
    public class LaserAbility : Ability<LaserAbilityData>
    {
        public LaserAbility(IAbilityCaster caster, LaserAbilityData data) : base(caster, data)
        {
        }

        protected override void GetTouchedTargets(List<IAbilityTarget> targets)
        {
            int count = OrbitalPhysics.BoxCastNonAlloc(Caster.CastPoint, Caster.CastDirection, 
                new Vector3(1, Data.BoxHeight, 1), Quaternion.identity, Data.RayDistance,
                Caster.CurrentRing.RingData.Radius, Caster.CurrentRing.transform.position,
                HitsBuffer);
            TryAddHitTargets(HitsBuffer, count, targets); 
            //TryAddTargets(ColliderBuffer, count, targets);
        }
    }
}