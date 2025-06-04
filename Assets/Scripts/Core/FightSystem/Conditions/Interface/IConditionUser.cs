using System;
using Orpheus.Core.Rings;
using UnityEngine;

namespace Orpheus.Core.FightSystem.Conditions.Interface
{
    public interface IConditionUser
    {
        public event Action<bool> Death;
        public event Action<bool> Chase;
        public event Action<bool,int> Attaque;
        public int AttaqueIndex{ get; }
        public void OnConditionReached();
        public void SetAnimator(String action);
        public Transform GetTransform();
    }
}