using UnityEngine;

public static class OrbitalMath
{
    public static Vector3 GetPositionOnRing(Vector3 center, float radius, float angle)
    {
        return center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
    }

    public static float GetAngleFromPosition(Vector3 center, Vector3 position)
    {
        Vector3 dir = (position - center).normalized;
        return Mathf.Atan2(dir.z, dir.x);
    }
    
    public static Vector3 GetPositionFromAngle(float angle, float radius, Vector3 center)
    {
        float x = center.x + Mathf.Cos(angle) * radius;
        float z = center.z + Mathf.Sin(angle) * radius;
        return new Vector3(x, 0, z);
    }

    public static Vector3 ClampToRing(Vector3 position, Vector3 center, float radius)
    {
        return GetPositionFromAngle(GetAngleFromPosition(center, position), radius, center);
    }

    public static Vector3 GetTangent(Vector3 position, Vector3 center, float direction)
    {
        Debug.Log(Mathf.Sign(direction));
        Vector3 radiusDir = (position - center).normalized ;
        switch (direction)
        {
            case 1: return new Vector3(-radiusDir.z, 0f, radiusDir.x);
                break;
            case -1 : return new Vector3(radiusDir.z, 0f, -radiusDir.x);    
                break;
            default:
                break;
        }
        return new Vector3(-radiusDir.z, 0f, radiusDir.x);
    }
}

