using System;
using Orpheus.Core.Rings;
using UnityEngine;

namespace Orpheus.Core.FightSystem.Conditions.Interface
{
    public interface IConditionUser
    {
        public event Action<bool> Skill1;
        public event Action<bool> Skill2;
        public event Action<bool> Death;
        public event Action<bool> Chase;
        public Ring CurrentRing { get; }
        
        public void OnConditionReached();
        public void SetAnimator(String action);
        public Transform GetTransform();
    }
}