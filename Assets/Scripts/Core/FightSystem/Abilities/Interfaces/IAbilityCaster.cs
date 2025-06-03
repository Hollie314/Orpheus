using System;
using System.Collections.Generic;
using Orpheus.Core.FightSystem.Conditions.Interface;
using Orpheus.Core.FightSystem.Skills.Runtime;
using Orpheus.Core.Rings;
using UnityEngine;

namespace Orpheus.Core.FightSystem
{
    public interface IAbilityCaster : IConditionUser
    {
        public Vector3 CastPoint { get; }
        public Vector3 CastDirection { get; }
        public float Direction { get; }
        public TargetTeam Team { get; }
        public Ring CurrentRing { get; }
        public OrbitalStats Stats { get; }
        public List<Skill> skills { get; }
        public Animator Animator { get; }

        public void AddSkill(Skill skill, int index);
        public void RemoveSkill(Skill skill);
        public Vector3 GetAim();
        public IAbilityTarget GetTarget();
        public void SetAnimatorTrigger(String triggerName );
    }
}