using UnityEngine;

namespace Orpheus.Core.Orbital
{
    public static class OrbitalMath
    {
        public static Vector3 GetPositionFromAngle(Vector3 center, float radius, float angle)
        {
            return center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
        }

        public static float GetAngleFromPosition(Vector3 center, Vector3 position)
        {
            Vector3 dir = (position - center).normalized;
            return Mathf.Atan2(dir.z, dir.x);
        }
    
        public static Vector3 ClampToRing(Vector3 position, Vector3 center, float radius)
        {
            return GetPositionFromAngle(center , radius,GetAngleFromPosition(new Vector3(center.x,position.y,center.z), position) );
        }

        public static Vector3 GetTangent(Vector3 position, Vector3 center, float direction)
        {
            Vector3 radiusDir = (position - center).normalized ;
            if (direction > 0)
            {
                return new Vector3(-radiusDir.z, 0, radiusDir.x);
            }
            return new Vector3(radiusDir.z, 0, -radiusDir.x);
        }

        public static Vector3 PositionAfterRotation(Vector3 ringCenter, Vector3 actorPosition, float velocity, float radius)
        {
            float angle = GetAngleFromPosition(actorPosition,ringCenter);
            angle += GetAngularVelocity(velocity, radius);
            return GetPositionFromAngle(ringCenter, radius, angle);

        }
    
        public static float GetAngularVelocity(float velocity, float radius)
        {
            return (velocity / radius);
        }
    }
}

