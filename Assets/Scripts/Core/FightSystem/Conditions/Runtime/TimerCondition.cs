
using Orpheus.Core.FightSystem.Conditions.Data;
using Orpheus.Core.FightSystem.Conditions.Interface;

using UnityEngine;

namespace Orpheus.Core.FightSystem.Conditions
{
    public class TimerCondition : Condition<TimerConditionData>
    {
        public float CurrentTime{ get; private set; }
        public float Duration { get; private set; }
        
        
        public TimerCondition(IConditionUser conditionUser, TimerConditionData data) : base(conditionUser, data)
        {
        }
        
        public override void Initialize()
        {
            GlobalTimer.OnTick += OnConditionTriggered;
            Duration = ConditionData.Duration;
            ResetCondition();
        }

        public override void Dispose()
        {
            GlobalTimer.OnTick -= OnConditionTriggered;
        }

        public override void ResetCondition()
        {
            CurrentTime = Duration;
            IsReached = false;
        }
        
        public void OnConditionTriggered(float deltaTime)
        {
            float lastTime = CurrentTime;
            LowerCurrentTime(deltaTime);
            if (CurrentTime <= 0)
            {
               IsReached = true;
               if (lastTime > 0)
               {
                   ConditionUser.OnConditionReached();
               }
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