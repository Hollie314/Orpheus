using System;
using Orpheus.Core.Orbital.Player;
using Orpheus.Core.Rings;
using UnityEngine;

namespace Orpheus.Core
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] 
        private float distance;
        [SerializeField] 
        private float yOffset;

        [SerializeField] 
        private float damping;
        
        [SerializeField] 
        private PlayerOrbitalController playerOrbitalController;

        private Vector3 currentVel;

        private void LateUpdate()
        {
            Ring ring = playerOrbitalController.CurrentRing;

            if (ring != null)
            {
                Vector3 playerPos = playerOrbitalController.GroundPosition;
                Vector3 dir = playerPos - ring.transform.position;


                Vector3 newPos = playerPos + dir.normalized * distance + Vector3.up * yOffset;
                transform.position = Vector3.SmoothDamp(transform.position, newPos, ref currentVel, damping * Time.deltaTime);
            }
        }
    }
}
