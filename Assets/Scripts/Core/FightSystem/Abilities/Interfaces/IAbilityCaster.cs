using System;
using Orpheus.Core.FightSystem.Skills.Runtime;
using Orpheus.Core.Rings;
using UnityEngine;

namespace Orpheus.Core.FightSystem
{
    public interface IAbilityCaster
    {
        public Vector3 CastPoint { get; }
        public Vector3 CastDirection { get; }
        public TargetTeam Team { get; }
        public Ring CurrentRing { get; }
        public OrbitalStats Stats { get; }
        public event Action<bool> Skill1;
        public event Action<bool> Skill2;

        public void AddSkill(Skill skill);
        public void RemoveSkill(Skill skill);
        public Vector3 GetAim();
        public IAbilityTarget GetTarget();
    }
}