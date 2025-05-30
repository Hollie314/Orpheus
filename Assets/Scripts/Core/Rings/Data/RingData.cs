using Orpheus.Core.LevelGeneration;
using UnityEngine;

namespace Orpheus.Core.Rings
{
    [CreateAssetMenu(fileName = "RingData", menuName = "Orpheus/Ring/Ring")]
    public class RingData : ScriptableObject
    {
        [field : SerializeField] public RingSize Size { get; private set; }
        [field : SerializeField]public float Radius { get; private set; }
        [field : SerializeField]public RingVariantData[] Avatar { get; private set; }
    }
}
