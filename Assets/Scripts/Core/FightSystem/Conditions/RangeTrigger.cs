using System;
using UnityEngine;

namespace Orpheus.Core.FightSystem.Conditions
{
    public class RangeTrigger : MonoBehaviour
    {
        public event Action<Collider> OnEnterRange;
        public event Action<Collider> OnExitRange;

        private void OnTriggerEnter(Collider other)
        {
            OnEnterRange?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            OnExitRange?.Invoke(other);
        }
    }
}