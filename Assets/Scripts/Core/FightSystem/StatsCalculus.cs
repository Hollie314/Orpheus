using UnityEngine;

namespace Orpheus.Core.FightSystem
{
    public static class StatsCalculus
    {
        public static void Heal(OrbitalStats caster, OrbitalStats target, int flatHeal, int percentage, FloatStats percentageType)
        {
           // Debug.Log("healing");
        }
        
        
        private static float getResistanceValue(FloatStats stat, OrbitalStats stats)
        {
            float mitigatedStat = 0f;
            switch (stat)
            {
                case FloatStats.PhysicalDamages:
                    mitigatedStat = Mathf.Clamp(stats.getStat(FloatStats.PhysicalResistance)*0.005f, 0, 100) ;
                    break;
                case FloatStats.MagicalDamages:
                    mitigatedStat = Mathf.Clamp(stats.getStat(FloatStats.MagicalResistance)*0.005f, 0, 100);
                    break;
                default: mitigatedStat = stats.getStat(FloatStats.DebuffResistance);
                    break;
            }
            return mitigatedStat;
        }
    }
}