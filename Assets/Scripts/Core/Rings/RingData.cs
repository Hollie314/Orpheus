using UnityEngine;

namespace Orpheus.Core.Rings
{
    [CreateAssetMenu(fileName = "RingData", menuName = "Scriptable Objects/RingData")]
    public class RingData : ScriptableObject
    {
        [field : SerializeField] public RingSize Size { get; private set; }
        [field : SerializeField]public float Radius { get; private set; }
        [field : SerializeField]public GameObject Avatar { get; private set; }
    }
}
