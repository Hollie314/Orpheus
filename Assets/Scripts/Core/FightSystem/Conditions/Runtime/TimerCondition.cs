using Codice.Client.Common.EventTracking;
using Orpheus.Core.FightSystem.Conditions.Data;
using Orpheus.Core.FightSystem.Conditions.Interface;
using Orpheus.Core.FightSystem.Skills.Runtime;
using PlasticPipe.PlasticProtocol.Messages.Serialization;
using UnityEngine;

namespace Orpheus.Core.FightSystem.Conditions
{
    public class TimerCondition : ICondition<TimerConditionData>
    {
        public float CurrentTime{ get; private set; }
        public float Duration { get; private set; }
        public TimerConditionData ConditionData { get;private set; }
        public Skill Skill { get; private set;}
        public bool IsReached { get; private set; }
        
        
        public TimerCondition(Skill skill, TimerConditionData data)
        {
            ConditionData = data;
            Skill = skill;
        }
        
        public void Initialize()
        {
            GlobalTimer.OnTick += OnConditionTriggered;
            Duration = ConditionData.Duration;
            ResetCondition();
        }

        public void Dispose()
        {
            GlobalTimer.OnTick -= OnConditionTriggered;
        }
        
        public void ResetCondition()
        {
            CurrentTime = Duration;
            IsReached = false;
        }
        
        public void OnConditionTriggered(float deltaTime)
        {
            LowerCurrentTime(deltaTime);
            if (CurrentTime == 0)
            {
               IsReached = true;
               Skill.OnConditionReached();
            }
        }

        public void SetDuration(float duration)
        {
            this.Duration = duration;
        }

        public void LowerCurrentTime(float time)
        {
            CurrentTime = Mathf.Clamp(CurrentTime - time, 0, CurrentTime);
        }
    }
}