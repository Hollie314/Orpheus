using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Orpheus.Core.Rings;
using UnityEngine;
using UnityEngine.UIElements;

namespace Orpheus.Core.FightSystem.Trap
{
    [RequireComponent(typeof(CapsuleCollider))]
    public abstract class Trap : MonoBehaviour, IAbilityCaster
    {
        public Vector3 CastPoint { get;private set; }
        public virtual Vector3 CastDirection { get; private set; }
        public TargetTeam Team { get; private set; } = TargetTeam.Trap;
        public Ring Ring { get; private set; }
        public OrbitalStats Stats { get; }

        public AbilityData AbilityData { get; private set; }
        
        public bool IsActive { get; set; }
        [field: SerializeField] public float ActivationRate { get; private set; }
        private float currentTime;

        public void Initialize(Ring ring)
        {
            this.Ring = ring;
            currentTime = 0;
            IsActive = false;
        }

        public void Update()
        {
            //apply 
            if (IsActive)
            {
                if (currentTime >= ActivationRate || currentTime == 0)
                {
                    ActivateAbility(AbilityData.TotalLifetime);
                    currentTime = 0;
                }
                currentTime+= Time.deltaTime;
            }
            else
            {
                currentTime = 0;
            }
        }

        public IEnumerator ActivateAbility(float duration)
        {
            IAbility ability = this.AbilityData.GenerateAbility(this);
            AbilityManager.Instance.AddAbility(ability);
            yield return duration;
            AbilityManager.Instance.RemoveAbility(ability);
            
        }
    }
}