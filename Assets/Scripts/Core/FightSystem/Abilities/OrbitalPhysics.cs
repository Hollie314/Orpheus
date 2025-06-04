using System;
using Orpheus.Core.Orbital;
using UnityEngine;

namespace Orpheus.Core.FightSystem
{
    public static class OrbitalPhysics
    {
        private static Collider[] ColliderBuffer = new Collider[64];
        private static RaycastHit[] RaycastHitBuffer = new RaycastHit[64];

        public static int RaycastNonAlloc(Vector3 center, Vector3 direction, float distance, 
            float ringRadius,
            Vector3 ringCenter,
            RaycastHit[] results,
            float stepDistance = .2f,
            int layerMask = -1)
        {
            return CastNonAlloc(center, direction, distance, ringRadius, 
                ringCenter, results,
                (Vector3 pos, Vector3 dir) =>
                {
                    return Physics.RaycastNonAlloc(pos, dir, RaycastHitBuffer, stepDistance, layerMask);
                },
                stepDistance);
        }
        
        public static int SphereCastNonAlloc(Vector3 center, Vector3 direction, float radius, float distance, 
            float ringRadius,
            Vector3 ringCenter,
            RaycastHit[] results,
            float stepDistance = .2f,
            int layerMask = -1)
        {
            return CastNonAlloc(center, direction, distance, 
                ringRadius, ringCenter, 
                results,
                (Vector3 pos, Vector3 dir) =>
                {
                    return Physics.SphereCastNonAlloc(pos, radius, dir, RaycastHitBuffer, stepDistance, layerMask);
                },
                stepDistance);
        }
        public static int BoxCastNonAlloc(Vector3 center, Vector3 direction, Vector3 size, Quaternion rot, float distance, 
            float ringRadius,
            Vector3 ringCenter,
            RaycastHit[] results,
            float stepDistance = .2f,
            int layerMask = -1)
        {
            return CastNonAlloc(center, direction, distance, 
                ringRadius, ringCenter, 
                results,
                (Vector3 pos, Vector3 dir) =>
                {
                    return Physics.BoxCastNonAlloc(pos, size / 2, dir, RaycastHitBuffer, rot, stepDistance, layerMask);
                },
                stepDistance);
        }
        
        public static int CastNonAlloc(Vector3 center, Vector3 direction, float distance, 
            float ringRadius,
            Vector3 ringCenter,
            RaycastHit[] results,
            Func<Vector3, Vector3, int> castMethod,
            float stepDistance)
        {
            int currentCount = 0;

            int stepCount = Mathf.CeilToInt(distance / stepDistance);
            Vector3 pos = center;
            Vector3 dir = direction;
            
            for (int i = 0; i < stepCount; i++)
            {
                Vector3 nextPoint = OrbitalMath.ClampToRing( pos + dir, ringCenter, ringRadius);
                Vector3 nextDirection = nextPoint - center;

                int count = castMethod(nextPoint, nextDirection);
                for (int j = 0; j < count; j++)
                {
                    results[currentCount] = RaycastHitBuffer[j];
                    currentCount++;

                    if (currentCount >= results.Length)
                        return currentCount;
                }

                pos = nextPoint;
                dir = nextDirection;
            }

            return currentCount;
        }
    }
}