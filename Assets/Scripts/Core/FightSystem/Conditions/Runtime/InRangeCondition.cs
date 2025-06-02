using Orpheus.Core.FightSystem.Conditions.Data;
using Orpheus.Core.FightSystem.Conditions.Interface;
using Orpheus.Core.FightSystem.Skills.Runtime;
using Orpheus.Core.Orbital.Entities;
using UnityEngine;

namespace Orpheus.Core.FightSystem.Conditions
{
    public class InRangeCondition  : Condition<InRangeConditionData>
    {
        private IAbilityCaster caster;
        private TargetTeam target;
        
        private GameObject rangeObject;
        private RangeTrigger rangeTrigger;

        
        
        public InRangeCondition(IConditionUser conditionUser, InRangeConditionData data) : base(conditionUser, data)
        {
        }

        public override void Initialize()
        {
            target = ConditionData.TeamInRange;
            
            //Create the object that will serve as detector and stuck it to the caster
            rangeObject = new GameObject("RangeTrigger");
            rangeObject.transform.SetParent((ConditionUser).GetTransform());
            rangeObject.transform.localPosition = Vector3.zero;

            //Add the collider and set its range
            SphereCollider collider = rangeObject.AddComponent<SphereCollider>();
            collider.isTrigger = true;
            collider.radius = ConditionData.Range;

            //add a rigidbody for it to work
            Rigidbody rb = rangeObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;

            //Add the script with collision trigger event  
            rangeTrigger = rangeObject.AddComponent<RangeTrigger>();
            rangeTrigger.OnEnterRange += OnEnter;
            rangeTrigger.OnExitRange += OnExit;
            
            ResetCondition();
        }

        public override void Dispose()
        {
            if (rangeTrigger != null)
            {
                rangeTrigger.OnEnterRange -= OnEnter;
                rangeTrigger.OnExitRange -= OnExit;
            }

            if (rangeObject != null)
            {
                GameObject.Destroy(rangeObject);
            }
        }

        public override void ResetCondition()
        {
        }
        
        private void OnEnter(Collider other)
        {
            if (other.TryGetComponent(out IAbilityTarget enteredTarget) && enteredTarget.Team == target)
            {
                IsReached = true;
                ConditionUser.OnConditionReached();
                ConditionUser.SetAnimator("InRange");
            }
        }

        private void OnExit(Collider other)
        {
            if (other.TryGetComponent(out IAbilityTarget exitedTarget) && exitedTarget.Team == target)
            {
                IsReached = false;
            }
        }
    }
}