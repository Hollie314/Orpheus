using System;
using UnityEngine;

namespace Orpheus.Core
{
    public class GlobalTimer : MonoBehaviour
    {
        public static GlobalTimer Instance { get; private set; }
        public static event Action<float> OnTick;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }

        private void Update()
        {
            OnTick?.Invoke(Time.deltaTime);
        }
    }
}