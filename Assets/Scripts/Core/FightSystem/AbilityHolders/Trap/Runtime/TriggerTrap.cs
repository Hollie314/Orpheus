using System;
using Orpheus.Core.FightSystem.Runtime;
using UnityEngine;

namespace Orpheus.Core.FightSystem.Trap
{
    public class TriggerTrap : Trap
    {
        private Vector3 castDirection;
        public override Vector3 CastDirection => castDirection;

        private void Awake()
        {
            castDirection = transform.up;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<IAbilityTarget>() != null)
            {
                IsActive = true;
            }
        }
    }
}