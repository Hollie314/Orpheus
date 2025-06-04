using System;
using Orpheus.Core.Orbital.Player;
using Orpheus.Core.Rings;
using UnityEngine;

namespace Orpheus.Core
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] 
        public static float distance = 1.36f;
        [SerializeField] 
        private float yOffset;

        [SerializeField] 
        private float damping;
        
        [SerializeField] 
        private PlayerOrbitalController playerOrbitalController;
        [SerializeField] 
        private Transform followTarget;
        

        private void LateUpdate()
        {
            Ring ring = playerOrbitalController.CurrentRing;

            if (ring != null)
            {
                Vector3 playerPos = playerOrbitalController.transform.position;
                Vector3 dir = playerPos - ring.transform.position;
                dir.y = 0;
                
                Vector3 newPos = playerPos + dir.normalized * distance + Vector3.up * yOffset;
                followTarget.position = Vector3.Lerp(followTarget.position, newPos, damping * Time.deltaTime);
            }
        }
    }
}
