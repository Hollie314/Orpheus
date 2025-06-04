using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Orpheus.Core.LevelGeneration
{
    [CreateAssetMenu(fileName = "RingVariantData", menuName = "Orpheus/Ring/RingVariant")]
    public class RingVariantData : ScriptableObject
    {
        [field:SerializeField]
        public List<BiomePrefabEntry> biomeVariants;

        public GameObject GetVariant(BiomeName biome)
        {
            return biomeVariants.FirstOrDefault(x => x.biomeName == biome)?.ringVariantPrefab;
        }
    }
    
    [System.Serializable]
    public class BiomePrefabEntry
    {
        public BiomeName biomeName;
        public GameObject ringVariantPrefab;
    }
}