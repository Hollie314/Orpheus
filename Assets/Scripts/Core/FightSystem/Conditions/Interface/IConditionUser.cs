using System;
using Orpheus.Core.Rings;

namespace Orpheus.Core.FightSystem.Conditions.Interface
{
    public interface IConditionUser
    {
        public event Action<bool> Skill1;
        public event Action<bool> Skill2;
        public event Action<bool> Death;
        public event Action<bool> InRange;
        public Ring CurrentRing { get; }
        
        public void OnConditionReached();
        public void SetAnimator(String action);
    }
}