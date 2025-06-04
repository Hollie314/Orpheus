using System;
using UnityEngine;

namespace Orpheus.Core
{
    public class GlobalTimer : MonoBehaviour
    {
        public static GlobalTimer _instance;
        public static event Action<float> OnTick;

        public static GlobalTimer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject(nameof(GlobalTimer)).AddComponent<GlobalTimer>();
                    DontDestroyOnLoad(_instance);
                }

                return _instance;
            }
        }

        private void Update()
        {
            OnTick?.Invoke(Time.deltaTime);
        }
    }
}