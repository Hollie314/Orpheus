using System;
using System.Collections.Generic;
using DG.Tweening;
using Orpheus.Core.FightSystem.Skills.Data;
using Orpheus.Core.FightSystem.Skills.Runtime;
using Orpheus.Core.Orbital;
using Orpheus.Core.Rings;
using UnityEngine;
using UnityEngine.UI;

namespace Orpheus.Core.FightSystem.AbilityHolders.Projectile
{
    public class Projectile : MonoBehaviour
    {
        [field :SerializeField]
        public float Speed { get; private set; }
        [field :SerializeField]
        public float LifeTime { get;private set; }
        
        public IAbilityCaster Caster { get;private set; }
        public DamageType DamageType { get;private set; }
        public FloatStats FloatStats { get;private set; }
        public float FlatDamages { get;private set; }
        public float PercentDamages { get;private set; }
        public float Direction { get; private set; }

        private Rigidbody rb;
        private float currentTime;

        private void Awake()
        {
            this.rb = GetComponent<Rigidbody>();
            currentTime = 0;
        }

        private void OnEnable()
        {
           
        }

        public void Initialize(IAbilityCaster caster, DamageType damageType, FloatStats floatStats, float flatdamages, float percentDamages)
        {
            Caster = caster;
            DamageType = damageType;
            FloatStats = floatStats;
            FlatDamages = flatdamages;
            PercentDamages = percentDamages;
            Direction = Caster.Direction;
        }

        private void Update()
        {
            Move();
            Lookforward();
            currentTime++;
            if (currentTime >= LifeTime)
            {
                Destroy(this.gameObject);
            }
        }
        public void OnTriggerEnter(Collider other)
        {
            IAbilityTarget target = other.gameObject.GetComponent<IAbilityTarget>();
            if (target!=null)
            {
                if (target.Team!=Caster.Team)
                {
                    target.ApplyDamage(Caster,FlatDamages,PercentDamages,FloatStats,DamageType);
                }
            }
        }
       

        private void Move()
        {
            Vector2 angularVelocity = new Vector2(Caster.CurrentRing.GetAngularSpeed(Speed*Direction),0) * Time.deltaTime;
            Vector3 orbitalNewPosition =  Caster.CurrentRing.GetPositionOnRing(rb.position, angularVelocity);
            rb.MovePosition(orbitalNewPosition);
        }
        
        protected void Lookforward()
        {
            //tangent of the ring
            Vector3 currentposition = rb.position;
            Vector3 tangentDir = OrbitalMath.GetTangent(currentposition, Caster.CurrentRing.transform.position, Math.Sign(Direction));
            Vector3 lookTarget = currentposition + tangentDir;
            transform.DOLookAt(lookTarget,0.2f);
        }

        public void OnDestroy()
        {
            
        }
    }
}