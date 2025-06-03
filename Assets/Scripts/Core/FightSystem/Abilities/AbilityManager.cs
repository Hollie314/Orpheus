using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Orpheus.Core.FightSystem
{
    public class AbilityManager : MonoBehaviour
    {
        public static AbilityManager _instance;
        private List<IAbility> runningAbilities;
        
        
        public static AbilityManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject(nameof(AbilityManager)).AddComponent<AbilityManager>();
                    DontDestroyOnLoad(_instance);
                }

                return _instance;
            }
        }

        private void Awake()
        {
            runningAbilities = new();
            
        }

        public void AddAbility(IAbility ability)
        {
            ability.Init();
            runningAbilities.Add(ability);
            
        }
        public void RemoveAbility(IAbility ability)
        {
            runningAbilities.Remove(ability);
        }


        private void Update()
        {
            float deltaTime = Time.deltaTime;
            using (ListPool<IAbility>.Get(out List<IAbility> abilities))
            {
                abilities.AddRange(runningAbilities);
                foreach (var ability in abilities)
                {
                    if (ability.AbilityUpdate(deltaTime))
                    {
                        ability.EndAbility();
                       
                    }
                }
                abilities.Clear();
            }
        }

        public void OnDisable()
        {
            for (int i = 0; i < runningAbilities.Count; i++)
            {
                runningAbilities.Remove(runningAbilities[i]);
            }
            runningAbilities.Clear();
        }
        
        public GameObject SpawnVFX(GameObject prefab, Transform transform)
        {
            return Instantiate(prefab, transform);
        }
        
        public void DestroyVfx(GameObject vfx)
        {
            Destroy(vfx);
        }
        
        public GameObject SpawnProjectiles(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return Instantiate(prefab, position, rotation);
        }
        
    }
}