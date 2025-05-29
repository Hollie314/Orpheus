using UnityEngine;

namespace Orpheus.Core.FightSystem
{
    public static class StatsCalculus
    {
        public static float Heal(OrbitalStats caster, float flatHeal, float percentageOfMissingHp, float percentageOfMaxHp)
        {
            float missingHPHeal = (caster.getStat(FloatStats.HpMax) - caster.getStat(FloatStats.Hp)) * percentageOfMissingHp * 0.01f;
            float maxHpHeal = caster.getStat(FloatStats.HpMax) * percentageOfMaxHp * 0.01f;
            float heal = flatHeal + missingHPHeal + maxHpHeal;
            return heal;
        }
        
        public static float Damage(OrbitalStats caster, float flatDamages, float percentage, FloatStats stat)
        {
            float damage = flatDamages + caster.getStat(stat) * percentage * 0.01f;
            return damage;
        }

        public static float MitigatedDamages(float baseDamages,TargetTeam team, DamageType damageType, OrbitalStats target)
        {
            float damage = baseDamages - baseDamages * target.getStat(damageType)* 0.01f;
            damage -= damage * target.getStat(team)* 0.01f;
            damage -= damage * target.getStat(FloatStats.Resistance)* 0.01f;
            return damage;
        }
        
        public static void Debuff(OrbitalStats caster, OrbitalStats target, int flatDamages, int percentage)
        {
            
        }
        
        public static void Buff(OrbitalStats caster, OrbitalStats target, int flatDamages, int percentage)
        {
            
        }
        
        
        private static float GetTeamResistanceValue(TargetTeam team, OrbitalStats targetStats)
        {
            return targetStats.getStat(team);
        }
        
        private static float GetDamageTypeResistance(DamageType damageType, OrbitalStats targetStats)
        {
            return targetStats.getStat(damageType);
        }
        

        public static float CalculateStatValue(OrbitalStats target, float flatValue, float percentValue, FloatStats statType)
        {
            float finalValue = flatValue + percentValue * target.getStat(statType);
            return finalValue;
        }
        
    }
}