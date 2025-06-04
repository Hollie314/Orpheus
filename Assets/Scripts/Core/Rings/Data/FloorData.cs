using System;
using UnityEngine;

namespace Orpheus.Core.Rings
{
    [CreateAssetMenu(fileName = "FloorData", menuName = "Orpheus/Ring/Floor")]
    public class FloorData : ScriptableObject
    {
        [field:SerializeField]
        public int FloorIndex { get; private set; }
        
        [field:SerializeField]
        public GameObject[] EnemiesToSpawn { get; private set; }
        
    }
}