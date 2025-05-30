using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Player_Detected", story: "Agent is in proximity of Player", category: "Conditions", id: "6f49f00b8f7251288ca5906c7c3e3f87")]
public partial class PlayerDetectedCondition : Condition
{

    public override bool IsTrue()
    {
        return true;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
