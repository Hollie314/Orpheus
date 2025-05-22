using Orpheus.Core.Rings;
using UnityEngine;

namespace Orpheus.Core.FightSystem
{
    public interface IAbilityCaster
    {
        public Vector3 CastPoint { get; }
        public Vector3 CastDirection { get; }
        public TargetTeam Team { get; }
        public Ring Ring { get; }
        public OrbitalStats Stats { get; }
    }
}