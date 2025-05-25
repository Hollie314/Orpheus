using Orpheus.Core.FightSystem.Runtime;
using UnityEngine;

namespace Orpheus.Core.FightSystem
{
    [CreateAssetMenu(fileName = "CloseRangeAbilityData", menuName = "Orpheus/FightSystem/AbilityData/CloseRange", order = 0)]
    public class CloseRangeAbilityData : AbilityData
    {
        [field: Header("Metrics")]
        [field: SerializeField]
        public float HitBoxRadius { get; private set; }
        [field: SerializeField]
        public float HitBoxHeight{ get; private set; }
        
        public override IAbility GenerateAbility(IAbilityCaster caster) => new CloseRangeAbility(caster, this);
    }
}