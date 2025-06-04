
using Orpheus.Core.Orbital.Player.States.MovementState;
using Orpheus.Core.Rings;

using UnityEngine;

namespace Orpheus.Core.FightSystem
{
    public interface IAbilityTarget
    {
        TargetTeam Team { get; }
        public OrbitalStats Stats { get; }
        public Ring CurrentRing { get; }
        public IMovement CurrentMovement { get; }
        public Animator Animator { get; }

        void ApplyDamage(IAbilityCaster caster, float flatValue, float percentageOfStat, FloatStats stat, DamageType damageType)
        {
            //Calculate damages
            float damages = StatsCalculus.Damage(caster.Stats, flatValue, percentageOfStat, stat);
            //Calculate mitigated damages
            damages = StatsCalculus.MitigatedDamages(damages, caster.Team, damageType, Stats);
            
            //Apply damages, it does not implement shield for now.
            Stats.setStat(FloatStats.Hp, Mathf.Clamp(Stats.getStat(FloatStats.Hp)- damages,0,Stats.getStat(FloatStats.HpMax)));
            Animator.SetTrigger("damages");
            
            if (Stats.getStat(FloatStats.Hp) <= 0)
            {
                OnDeath(caster, damageType);
            }
        }

        void Heal(IAbilityCaster caster, float flatValue,  float percentageOffMissingHp, float percentageOfMaxHp)
        {
            //Calculate heal
            float heal = StatsCalculus.Heal(caster.Stats, flatValue, percentageOffMissingHp, percentageOfMaxHp);
            //Apply Heal clamped at max HP
            Stats.setStat(FloatStats.Hp,Mathf.Clamp(Stats.getStat(FloatStats.Hp) +heal, 0, Stats.getStat(FloatStats.HpMax)));
        }
        void ApplyStatus();
        void ApplyMovement(IMovement movement, float duration);
        void OnDeath(IAbilityCaster caster, DamageType damageType);
    }
}