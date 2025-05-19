using Orpheus.Core.FightSystem.Runtime;
using UnityEngine;

namespace Orpheus.Core.FightSystem
{
    public class LaserAbilityData : AbilityData
    {
        
        [field: Header("Metrics")]
        [field: SerializeField]
        public float BoxHeight { get; private set; }
        [field: SerializeField]
        public float RayDistance { get; private set; }
        
        public override IAbility GenerateAbility(IAbilityCaster caster) => new LaserAbility(caster, this);
    }
}