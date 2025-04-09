using UnityEngine;

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
        return GetPositionFromAngle(center , radius,GetAngleFromPosition(center, position) );
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
}

