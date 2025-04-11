using System;
using Core;
using UnityEngine;

public class OrbitalCamera : MonoBehaviour, IMovable
{
    public Character mainCharacter; //the script
    [field : SerializeField] private float offset = 5f; //offset from the character
    private float radius; 
    private float angle;
    private Transform center;

    void LateUpdate()
    {
        UpdateRing();
        MoveOnRing();
        transform.LookAt(mainCharacter.transform.position);
    }

    public float GetAngle()
    {
        return mainCharacter.GetAngle();
    }

    public void SetAngle(float angle)
    {
        this.angle = angle;
    }

    public void MoveOnRing()
    {
        this.angle = GetAngle();
        Vector3 newposition = OrbitalMath.GetPositionFromAngle(center.position, radius, angle); //calculate new position
        transform.position = new Vector3(newposition.x, transform.position.y, newposition.z);
    }

    public void UpdateRing()
    {
        radius = mainCharacter.ring.Item2 + offset;
        center = mainCharacter.ring.Item1;
    }
}
