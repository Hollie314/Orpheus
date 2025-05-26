using NaughtyAttributes;
using NUnit.Framework;
using Orpheus.Core.FightSystem.Runtime;
using Orpheus.Core.Orbital;
using UnityEngine;

namespace Orpheus.Core.FightSystem
{
    public abstract class AbilityData : ScriptableObject
    {
        [field: Header("Durations")] 
        
        [field: Space]
        [field: SerializeField] 
        public float CastDuration { get; private set; }
        
        [field: Space]
        [field: SerializeField] 
        public float FireCount { get; private set; }
        [field: SerializeField] 
        public float FireDuration { get; private set; }
        
        [field: Space]
        [field: SerializeField] 
        public float HitInterval { get; private set; }
        
        [field: Space]
        [field: SerializeField] 
        public float RecoilDuration { get; private set; }
        
        [field: Space]
        [field: SerializeField] 
        public float Cooldown { get; private set; }


        [field: Header("Damage")]
        [field: SerializeField]
        public bool DamageOtherTeam { get; private set; } = true;
        [field: SerializeField]
        public bool DamageSameTeam { get; private set; } = false;
        
        [field: SerializeField, Min(0)] 
        public float FlatDamage { get; private set; }
        [field: SerializeField, Min(0)] 
        public float PercentDamage { get; private set; }
        [field: SerializeField]
        public FloatStats DamageStat { get; private set; }
        [field: SerializeField]
        public DamageType DamageType { get; private set; }
        
        [field: Header("Heal")]
        [field: SerializeField]
        public bool HealOtherTeam { get; private set; } = false;
        [field: SerializeField]
        public bool HealSameTeam { get; private set; } = true;
        [field: SerializeField, Min(0)] 
        public float FlatHeal { get; private set; }
        [field: SerializeField, MinMaxSlider(0,1)] 
        public float PercentHealCurrentHp { get; private set; }
        [field: SerializeField, MinMaxSlider(0,1)]
        public float PercentHealMaxHp { get; private set; }
        
        [field: Header("Movement")]
        //public IOrbitalMovementState<T> MovementState { get; private set; }
        
        public float CastTiming => CastDuration;
        public float FireTiming => CastDuration + FireDuration;
        public float RecoilTiming => CastDuration + FireDuration + RecoilDuration;
        
        public float TotalLifetime => RecoilTiming;
        
        public abstract IAbility GenerateAbility(IAbilityCaster caster);
    }
}