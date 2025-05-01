using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Ring : MonoBehaviour
{
    private int floorIndex;

   [field :SerializeField] public RingData RingData { get; private set; }
    

    public RingSize GetNextLarger(RingSize size)
    {
        if ((int)size < RingSize.GetValues(typeof(RingSize)).Length - 1)
            return (RingSize)((int)size + 1);
        return size;
    }

    public RingSize GetNextSmaller(RingSize size)
    {
        if ((int)size > 0)
            return (RingSize)((int)size - 1);
        return size;
    }

    private float GetAngle(Vector3 position)
    {
        return OrbitalMath.GetAngleFromPosition(transform.position, position);
    }

    private float GetAngularSpeed(float force)
    {
        return force / RingData.Radius;
    }

    public Vector3 GetPositionOnRing(Vector3 position, Vector2 speed)
    {
        //move on the ring
        float angle = GetAngle(position); //get actual angle
        angle += GetAngularSpeed(speed.x) * Time.deltaTime; //calculate new angle based on speed
        Vector3 positionFromAngle = OrbitalMath.GetPositionFromAngle(position, RingData.Radius, angle);
        Vector3 verticalPosition = new Vector3(0,position.y + speed.y * Time.deltaTime,0); // Vertical position
        return positionFromAngle+verticalPosition; //calculate new position
    }

    public Vector3 ClampToRing(Vector3 position)
    {
        return OrbitalMath.ClampToRing(position, transform.position, RingData.Radius);
    }
}
