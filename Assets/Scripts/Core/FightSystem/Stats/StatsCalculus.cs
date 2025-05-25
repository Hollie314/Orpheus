using UnityEngine;

namespace Orpheus.Core.FightSystem
{
    public static class StatsCalculus
    {
        public static void Heal(OrbitalStats caster, OrbitalStats target, int flatHeal, int percentageOfMissingHp, int percentageOfMaxHp)
        {
           // Debug.Log("healing");
        }
        
        public static void Damage(OrbitalStats caster, OrbitalStats target, int flatDamages, int percentage)
        {
            // Debug.Log("healing");
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