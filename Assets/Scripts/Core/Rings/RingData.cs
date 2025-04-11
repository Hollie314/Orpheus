using UnityEngine;

[CreateAssetMenu(fileName = "RingData", menuName = "Scriptable Objects/RingData")]
public class RingData : ScriptableObject
{
    public RingSize Size { get; private set; }
    public float Radius { get; private set; }
    public GameObject avatar { get; private set; }
}
