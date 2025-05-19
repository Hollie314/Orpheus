using System.Collections.Generic;
using UnityEngine;

namespace Orpheus.Core.FightSystem
{
    public class OrbitalStats
    {
        private OrbitalStatsData orbitalStatsData;
        private Dictionary<FloatStats, float> floatStats = new();
        private Dictionary<BoolStats, bool> boolStats = new();

        public void Initialize(OrbitalStatsData orbitalStatsData)
        {
            this.orbitalStatsData = orbitalStatsData;
            foreach (var floatStat in orbitalStatsData.FloatStats)
            {
                floatStats.Add(floatStat.floatStatName, floatStat.value);
            }
            
            foreach (var boolStat in orbitalStatsData.BoolStats)
            {
                boolStats.Add(boolStat.boolStatName, boolStat.value);
            }
        }

        public float getStat(FloatStats stat) => floatStats.TryGetValue(stat, out var val) ? val : 0f;
        public bool getStat(BoolStats stat) => boolStats.TryGetValue(stat, out var val) && val;

        public void setStat(FloatStats stat, float value) => floatStats[stat] = value;
        public void setStat(BoolStats stat, bool value) => boolStats[stat] = value;
        public void addtoStat(FloatStats stat, float value) => setStat(stat, getStat(stat) + value);

        public float getResistanceValue(FloatStats stat)
        {
            float mitigatedStat = 0f;
            switch (stat)
            {
                case FloatStats.PhysicalDamages:
                    mitigatedStat = Mathf.Clamp(getStat(FloatStats.PhysicalResistance)*0.005f, 0, 100) ;
                    break;
                case FloatStats.MagicalDamages:
                    mitigatedStat = Mathf.Clamp(getStat(FloatStats.MagicalResistance)*0.005f, 0, 100);
                    break;
                default: mitigatedStat = getStat(FloatStats.DebuffResistance);
                    break;
            }

            return mitigatedStat;
        }

        public float getPenetration(FloatStats stat)
        {
            float penetration = 0f;
            switch (stat)
            {
                case FloatStats.PhysicalDamages :
                    penetration = getStat(FloatStats.ArmorPenetration);
                    break;
                case FloatStats.MagicalDamages :
                    penetration = getStat(FloatStats.MagicPenetration);
                    break;
            }
            return penetration;
        }

        public float getScaledStat(FloatStats stat, float percentValue)
        {
            float scaled = 0;
            switch (stat)
            {
                case FloatStats.PhysicalDamages:
                    scaled = getStat(FloatStats.PhysicalDamages)*percentValue;
                    break;
                case FloatStats.MagicalDamages:
                    scaled = getStat(FloatStats.MagicalDamages)*percentValue;
                    break;
                default: scaled = getStat(FloatStats.BuffAugmentation)*percentValue;
                    break;
            }
            return scaled;
        }
    }
    
    
}