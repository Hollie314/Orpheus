using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Orpheus.Core.FightSystem
{
    public class AbilityManager : MonoBehaviour
    {
        public static AbilityManager _instance;
        
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

        private List<IAbility> runningAbilities;

        private void Awake()
        {
            runningAbilities = new();
        }

        public void AddAbility(IAbility ability)
        {
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
                    Debug.Log("we are using ability !" +ability);
                    if (ability.Update(deltaTime))
                    {
                        RemoveAbility(ability);
                    }
                }
            }
        }
    }
}