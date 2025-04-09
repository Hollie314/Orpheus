using System;
using Core;
using UnityEngine;

public class OrbitalCamera : MonoBehaviour, IMovable
{
    public Transform target; // theplayer
    public Character mainCharacter; //the script
    [field : SerializeField] private float offset = 5f; //offset from the character
    private float radius; 
    public float followSpeed = 10f; //it should

    private float angle;

    private void Start()
    {
        UpdateRadius();
        MoveOnRing();
    }

    void LateUpdate()
    {
        UpdateRadius();
        MoveOnRing();
        transform.LookAt(target.position);
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
        Vector3 newposition = OrbitalMath.GetPositionFromAngle(mainCharacter.center.position, radius, angle); //calculate new position
        transform.position = new Vector3(newposition.x, transform.position.y, newposition.z);
    }

    public void UpdateRadius()
    {
        radius = mainCharacter.radius + offset;
    }
}
