using System.Collections.Generic;
using Orpheus.Core.FightSystem;
using UnityEngine;

namespace Orpheus.Core
{
    [CreateAssetMenu(fileName = "OrbitalStatsData", menuName = "Scriptable Objects/OrbitalStatsData")]
    public class OrbitalStatsData : ScriptableObject
    {
        [System.Serializable]
        public class FloatStatEntry
        {
            public FloatStats floatStatName;
            public float value;
        }
        
        [System.Serializable]
        public class BoolStatEntry
        {
            public BoolStats boolStatName;
            public bool value;
        }

        [System.Serializable]
        public class TeamResistanceEntry
        {
            public TargetTeam teamName;
            public float resistanceValue;
        }

        [System.Serializable]
        public class DamageResistanceEntry
        {
            public DamageType damageTypeName;
            public float resistanceValue;
        }

        [field: SerializeField] public FloatStatEntry[] FloatStats { get; private set; }
        [field: SerializeField] public BoolStatEntry[] BoolStats { get; private set; }
        
        [field: Header("Resistance")]
        [field: SerializeField] public TeamResistanceEntry[] TeamResistances { get; private set; }
        [field: SerializeField] public DamageResistanceEntry[] DamageResistances { get; private set; }
        
    }
}
