using System;
using System.Collections.Generic;
using Orpheus.Core.FightSystem.AbilityHolders.Projectile;
using Orpheus.Core.Orbital.Player.States.MovementState;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace Orpheus.Core.FightSystem.Runtime
{
    public abstract class Ability<T> :MonoBehaviour, IAbility  where T : AbilityData
    {
        protected static Collider[] ColliderBuffer = new Collider[64];
        protected static RaycastHit[] HitsBuffer = new RaycastHit[64];

        public readonly IAbilityCaster Caster;
        public readonly T Data;


        public float CurrentLifetime { get; private set; }
        public int CurrentFireCount { get; private set; }

        private List<GameObject> castVFXGameObjects;
        private bool isCastVFXInstantiated;
        private List<GameObject> fireVFXGameObjects;
        private bool isFireVFXInstantiated;
        private List<GameObject> recoilVFXGameObjects;
        private bool isRecoilVFXInstantiated;
        
        //event
        public event Action OnEnd;


        public Ability(IAbilityCaster caster, T data)
        {
            Data = data;
            Caster = caster;
            CurrentFireCount = 0;
            CurrentLifetime = 0;
            castVFXGameObjects = new List<GameObject>();
            fireVFXGameObjects = new List<GameObject>();
            recoilVFXGameObjects = new List<GameObject>();
        }
        
        public bool AbilityUpdate(float deltaTime)
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
            if (!isCastVFXInstantiated)
            {
                isCastVFXInstantiated = true;
                if (Data.Cast_VFX != null)
                {
                    foreach (var vfx in Data.Cast_VFX)
                    {
                        castVFXGameObjects.Add(Instantiate(vfx, Caster.GetTransform()));
                    }
                }
            }
            
            if (Data.Castmovement)
            {
                IAbilityTarget target = (IAbilityTarget)Caster;
                if (target != null)
                {
                    if (target.CurrentMovement == null)
                    {
                        target.ApplyMovement(Data.Castmovement,Data.CastDuration);
                    }
                }
            }
        }

        protected virtual void ProcessFirePhase(float deltaTime)
        {
            if (!isFireVFXInstantiated)
            {
                isFireVFXInstantiated = true;
                if (Data.Fire_VFX != null)
                {
                    foreach (var vfx in Data.Fire_VFX)
                    {
                        fireVFXGameObjects.Add(Instantiate(vfx, Caster.GetTransform()));
                    }
                }
            }
            
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
            if (!isRecoilVFXInstantiated)
            {
                isRecoilVFXInstantiated = true;
                if (Data.Recoil_VFX != null)
                {
                    foreach (var vfx in Data.Recoil_VFX)
                    {
                        recoilVFXGameObjects.Add(Instantiate(vfx, Caster.GetTransform()));
                    }
                }
            }
        }

        protected virtual void Fire()
        {
            using (ListPool<IAbilityTarget>.Get(out List<IAbilityTarget> targets))
            {
                GetTouchedTargets(targets);
                foreach (var target in targets)
                {
                    if (CanDamageTarget(target))
                    {
                        target.ApplyDamage(Caster, Data.FlatDamage, Data.PercentDamage, Data.DamageStat, Data.DamageType);
                        if (target.Stats.getStat(FloatStats.Hp) <= 0)
                        {
                            target.OnDeath(Caster, Data.DamageType);
                            continue;
                        }
                    }
                    
                    if(CanHealTarget(target))
                        target.Heal(Caster, Data.FlatHeal, Data.PercentHealCurrentHp, Data.PercentHealMaxHp);
                    //Apply status
                    //apply movement
                    if (Data.Firemovement && target != Caster)
                    {
                        target.ApplyMovement(Data.Firemovement, Data.FiremovementDuration);
                    }
                }
            }
            if (Data.projectile != null)
            {
                Vector3 position = new Vector3(Caster.CastPoint.x+ (Caster.CastDirection.x*Data.xoffset), Caster.CastPoint.y+ Data.yoffset,
                    Caster.CastPoint.z + (Caster.CastDirection.z*Data.zoffset));
                Quaternion quaternion = Quaternion.LookRotation(Caster.CastDirection);
                GameObject projectile = Instantiate(Data.projectile, position,quaternion);
                Projectile projectileScript = projectile.GetComponent<Projectile>();
                projectileScript.Initialize(Caster, Data.DamageType, Data.DamageStat, Data.FlatDamage, Data.PercentDamage);
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

        private void ClearCastVFX()
        {
            for (int i = 0; i < castVFXGameObjects.Count; i++)
            {
                Destroy(castVFXGameObjects[i]);
            }
            castVFXGameObjects.Clear();
        }

        private void ClearFireVFX()
        {
            for (int i = 0; i < fireVFXGameObjects.Count; i++)
            {
                Destroy(fireVFXGameObjects[i]);
            }
            fireVFXGameObjects.Clear();
        }

        private void ClearRecoilVFX()
        {
            for (int i = 0; i < recoilVFXGameObjects.Count; i++)
            {
                Destroy(recoilVFXGameObjects[i]);
            }
            recoilVFXGameObjects.Clear();
        }

        public virtual void Dispose()
        {
            AbilityManager.Instance.RemoveAbility(this);
        }

        public void Reset()
        {
            CurrentFireCount = 0;
            CurrentLifetime = 0;
            isFireVFXInstantiated = false;
            isCastVFXInstantiated = false;
            isRecoilVFXInstantiated = false;
            OnEnd?.Invoke();
        }
        
        public float GetLifeTime()
        {
            return Data.TotalLifetime;
        }
    }
}