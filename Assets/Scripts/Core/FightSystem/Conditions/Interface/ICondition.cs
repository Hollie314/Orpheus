using Orpheus.Core.FightSystem.Conditions.Data;
using Orpheus.Core.FightSystem.Skills.Runtime;

namespace Orpheus.Core.FightSystem.Conditions.Interface
{
    public interface ICondition
    {
        public bool IsReached { get; }
        public void Initialize();
        public void Dispose();
        public void ResetCondition();
        
    }
}