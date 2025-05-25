using System.Collections.Generic;
using Orpheus.Core.FightSystem.Skills.Runtime;
using UnityEngine;
using UnityEngine.Pool;

namespace Orpheus.Core.FightSystem.Skills
{
    public class SkillManager : MonoBehaviour
    {
        public static SkillManager _instance;
        
        public static SkillManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject(nameof(AbilityManager)).AddComponent<SkillManager>();
                    DontDestroyOnLoad(_instance);
                }

                return _instance;
            }
        }

        private List<Skill> runningSkills;

        private void Awake()
        {
            runningSkills = new();
        }

        public void AddAbility(Skill skill)
        {
            runningSkills.Add(skill);
        }
        public void RemoveAbility(Skill skill)
        {
            runningSkills.Remove(skill);
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            foreach (var skill in runningSkills)
            {
              
            }
        }
    }
}