using System.Collections.Generic;
using UnityEngine;

namespace Orpheus.Core.FightSystem
{
    public class OrbitalStats
    {
        private OrbitalStatsData orbitalStatsData;
        private Dictionary<FloatStats, float> floatStats = new();
        private Dictionary<BoolStats, bool> boolStats = new();
        
        private Dictionary<TargetTeam, float> teamResistances = new();
        private Dictionary<DamageType, float> damageResistances = new();

        public void Initialize(OrbitalStatsData orbitalStatsData, int difficulty)
        {
            this.orbitalStatsData = orbitalStatsData;
            foreach (var floatStat in orbitalStatsData.FloatStats)
            {
                floatStats.Add(floatStat.floatStatName, floatStat.value * difficulty);
            }
            
            foreach (var boolStat in orbitalStatsData.BoolStats)
            {
                boolStats.Add(boolStat.boolStatName, boolStat.value);
            }

            foreach (var teamResistance in orbitalStatsData.TeamResistances)
            {
                teamResistances.Add(teamResistance.teamName, teamResistance.resistanceValue * difficulty);
            }
            
            foreach (var damageResistance in orbitalStatsData.DamageResistances)
            {
                damageResistances.Add(damageResistance.damageTypeName, damageResistance.resistanceValue * difficulty);
            }
        }

        public float getStat(FloatStats stat) => floatStats.TryGetValue(stat, out var val) ? val : 0f;
        public bool getStat(BoolStats stat) => boolStats.TryGetValue(stat, out var val) && val;
        public float getStat(TargetTeam stat) => teamResistances.TryGetValue(stat, out var val) ? val : 0f;
        public float getStat(DamageType stat) => damageResistances.TryGetValue(stat, out var val) ? val : 0f;


        public void setStat(FloatStats stat, float value)
        {
            if (floatStats.ContainsKey(stat))
            {
                floatStats[stat] = value;
            }
        }
        public void setStat(BoolStats stat, bool value)
        {
            if (boolStats.ContainsKey(stat))
            {
                boolStats[stat] = value;
            }
        }
        public void setStat(TargetTeam stat, float value)
        {
            if (teamResistances.ContainsKey(stat))
            {
                teamResistances[stat] = value;
            }
        }
        
        public void setStat(DamageType stat, float value)
        {
            if (damageResistances.ContainsKey(stat))
            {
                damageResistances[stat] = value;
            }
        }
        
        public void addtoStat(FloatStats stat, float value) => setStat(stat, getStat(stat) + value);
        public void addtoStat(TargetTeam stat, float value) => setStat(stat, getStat(stat) + value);
        public void addtoStat(DamageType stat, float value) => setStat(stat, getStat(stat) + value);
    }
}