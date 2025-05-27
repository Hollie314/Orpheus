using System;
using NaughtyAttributes;
using Orpheus.Core.FightSystem;
using Orpheus.Core.FightSystem.Skills.Runtime;
using Orpheus.Core.Rings;
using UnityEngine;

namespace Orpheus.Core.Orbital.Entities
{
    public class AI_Entities : OrbitalController<AI_Entities>, IAbilityTarget, IAbilityCaster
    {
        [SerializeField, BoxGroup("Entity")] private Ring ring;
        public Vector3 CastPoint { get;private set; }
        public Vector3 CastDirection { get;private set; }
        public TargetTeam Team { get; private set; }
        public event Action<bool> Skill1;
        public event Action<bool> Skill2;
        
        
        protected override void Awake()
        {
            base.Awake();
            SetRing(ring);
            Team = TargetTeam.Enemy;
        }
        
        public void AddSkill(Skill skill)
        {
            
        }

        public void RemoveSkill(Skill skill)
        {
            
        }

        public Vector3 GetAim()
        {
            return Vector3.zero;
        }

        public IAbilityTarget GetTarget()
        {
            return null;
        }

        public void ApplyStatus()
        {
           
        }

        public void ApplyMovement()
        {
            
        }

        public void OnDeath()
        {
            
        }
    }
}