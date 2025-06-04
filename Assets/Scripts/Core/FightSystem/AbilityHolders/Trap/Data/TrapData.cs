using Orpheus.Core.Rings;
using UnityEngine;

namespace Orpheus.Core.FightSystem.Trap
{
    public class TrapData : ScriptableObject
    {
        public Vector3 CastPoint { get; private set; }
        public Vector3 CastDirection { get; private set; }
        public AbilityData AbilityData { get; private set; }
    }
}