using Orpheus.Core.FightSystem.Conditions.Data;
using Orpheus.Core.FightSystem.Skills.Runtime;

namespace Orpheus.Core.FightSystem.Conditions.Interface
{
    public interface ICondition<T> where T : ConditionData
    {
        public bool IsReached { get; }
        
        public  T ConditionData{ get; }
        public Skill Skill { get; }

        public void Initialize();
        public void Dispose();
        public void ResetCondition();
        
    }
}