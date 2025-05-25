using System;
using Orpheus.Core.FightSystem.Conditions.Data;
using Orpheus.Core.FightSystem.Conditions.Interface;
using Orpheus.Core.FightSystem.Runtime;
using Orpheus.Core.FightSystem.Skills.Runtime;
using PlasticPipe.PlasticProtocol.Messages.Serialization;

namespace Orpheus.Core.FightSystem.Conditions
{
    public abstract class Condition<T> : ICondition<T> where T : ConditionData
    {
        public readonly IAbilityCaster Caster;
        public T ConditionData { get; protected set;}
        public Skill Skill { get; }
        

        public bool IsReached { get; protected set; }
        

        public Condition(IAbilityCaster caster, T data)
        {
            Caster = caster;
            ConditionData = data;
        }

        public void ResetCondition()
        {
            InitializeCondition();
        }
        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        

        protected abstract void InitializeCondition();

    }
}