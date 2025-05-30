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

        private void OnEnable()
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
            ability.Reset();
        }


        private void Update()
        {
            float deltaTime = Time.deltaTime;
            using (ListPool<IAbility>.Get(out List<IAbility> abilities))
            {
                abilities.AddRange(runningAbilities);

                foreach (var ability in abilities)
                {
                    Debug.Log(ability.GetLifeTime());
                    if (ability.AbilityUpdate(deltaTime))
                    {
                        RemoveAbility(ability);
                    }
                }
            }
        }

        public void OnDisable()
        {
            for (int i = 0; i < runningAbilities.Count; i++)
            {
                runningAbilities[i].Dispose();
            }
            runningAbilities.Clear();
        }
    }
}