using System;
using Orpheus.Core.FightSystem.Runtime;
using UnityEngine;

namespace Orpheus.Core.FightSystem.Trap
{
    public class BasicTrap : Trap
    {
        protected override void SetCastDirection()
        {
            CastDirection = transform.up;
        }
    }
}