using System;
using Orpheus.Core.FightSystem.Conditions.Data;
using Orpheus.Core.FightSystem.Conditions.Interface;


namespace Orpheus.Core.FightSystem.Conditions
{
    public abstract class Condition<T> : ICondition where T : ConditionData
    {
        public T ConditionData { get; protected set;}
        public IConditionUser ConditionUser { get;protected set; }
        public bool IsReached { get; protected set; }
        

        public Condition(IConditionUser conditionUser, T data)
        {
            ConditionUser = conditionUser;
            ConditionData = data;
        }

        public virtual void ResetCondition()
        {
        }
        public virtual void Initialize()
        {
        }

        public virtual void Dispose()
        {
        }
    }
}