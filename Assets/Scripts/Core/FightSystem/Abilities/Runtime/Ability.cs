using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Orpheus.Core.FightSystem.Runtime
{
    public abstract class Ability<T> : IAbility where T : AbilityData
    {
        protected static Collider[] ColliderBuffer = new Collider[64];
        protected static RaycastHit[] HitsBuffer = new RaycastHit[64];

        public readonly IAbilityCaster Caster;
        public readonly T Data;


        public float CurrentLifetime { get; private set; }
        public int CurrentFireCount { get; private set; }
        
        
        public Ability(IAbilityCaster caster, T data)
        {
            Data = data;
            Caster = caster;
            CurrentFireCount = 0;
            CurrentLifetime = 0;
        }
        
        public bool Update(float deltaTime)
        {
            if (IsInCastPhase())
                ProcessCastPhase(deltaTime);

            if (IsInFirePhase())
                ProcessFirePhase(deltaTime);

            if (IsInRecoilPhase())
                ProcessRecoilPhase(deltaTime);

            CurrentLifetime += deltaTime;
            return CurrentLifetime >= Data.TotalLifetime;
        }

        //Delay before fire, serve for animation as well
        protected virtual void ProcessCastPhase(float deltaTime)
        {
            
        }

        protected virtual void ProcessFirePhase(float deltaTime)
        {
            //Time between fire
            float interval = Data.FireDuration / Data.FireCount;
            //Cmb de temps dans la phase de tir
            float currentFireDuration = CurrentLifetime - Data.CastDuration;

            //How many time it should have fire
            int targetFireCount = Mathf.FloorToInt(currentFireDuration / interval);
            //Delay fire has
            int missingFires = targetFireCount - CurrentFireCount;

            //Fire as many time needed to catch up 
            for (int i = 0; i < missingFires; i++)
                Fire();

            CurrentFireCount = targetFireCount;
        }
        
        protected virtual void ProcessRecoilPhase(float deltaTime)
        {
            
        }

        protected virtual void Fire()
        {
            using (ListPool<IAbilityTarget>.Get(out List<IAbilityTarget> targets))
            {
                GetTouchedTargets(targets);
                foreach (var target in targets)
                {
                    if(CanDamageTarget(target))
                        target.ApplyDamage(Data.FlatDamage);
                    
                    if(CanHealTarget(target))
                        target.Heal(Caster.Stats, Data.FlatHeal, Data.PercentHeal, 0, 0);
                    
                    //Apply status
                }
            }
        }

        protected abstract void GetTouchedTargets(List<IAbilityTarget> targets);

        private bool CanDamageTarget(IAbilityTarget target)
        {
            if (target.Team != Caster.Team && Data.DamageOtherTeam)
                return true;


            if (target.Team == Caster.Team && Data.DamageSameTeam)
                return true;

            return false;
        }
        private bool CanHealTarget(IAbilityTarget target)
        {
            if (target.Team != Caster.Team && Data.HealOtherTeam)
                return true;


            if (target.Team == Caster.Team && Data.HealSameTeam)
                return true;

            return false;
        }
        protected void TryAddTargets(Collider[] colliders, int count, List<IAbilityTarget> targets)
        {
            for (int i = 0; i < count; i++)
            {
                Collider col =  colliders[i];
                if (col.TryGetComponent(out IAbilityTarget target) && !targets.Contains(target))
                    targets.Add(target);
            }
        }
        
        protected void TryAddHitTargets(RaycastHit[] hits, int count, List<IAbilityTarget> targets)
        {
            for (int i = 0; i < count; i++)
            {
                Collider col =  hits[i].collider;
                if (col.TryGetComponent(out IAbilityTarget target) && !targets.Contains(target))
                    targets.Add(target);
            }
        }
        
        public bool IsInCastPhase() => CurrentLifetime < Data.CastTiming;
        public bool IsInFirePhase() => CurrentLifetime >= Data.CastTiming && CurrentLifetime < Data.FireTiming;
        public bool IsInRecoilPhase() => CurrentLifetime >= Data.FireTiming && CurrentLifetime < Data.RecoilTiming;

        public virtual void Init()
        {
            CurrentFireCount = 0;
            CurrentLifetime = 0;
        }

        public virtual void Dispose()
        {
        
        }
    }
}