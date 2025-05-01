using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public abstract class OrbitalController : MonoBehaviour
{
    [SerializeField] private List<OrbitalMovementState> _movementStates;
    [SerializeField, Range(1,64)] private int maxPenetrationCount;

    public Vector2 CurrentVelocity { get; private set; }
    public Ring CurrentRing { get; private set; }

    private OrbitalMovementState currentMovementState;
    private Rigidbody rb;
    private CapsuleCollider cc;

    private void Awake()
    {
        this.rb = GetComponent<Rigidbody>();
        this.cc = GetComponent<CapsuleCollider>();
    }

    private void FixedUpdate()
    {
        SelectNextState();
        ComputeVelocity();
    }

    private void SelectNextState()
    {
        OrbitalMovementState nextMovementState = null;
        int maxPriority = 0;
        foreach (var state in _movementStates)
        {
            int priority = state.GetStatePriority(this);
            if (priority > maxPriority)
            {
                maxPriority = priority;
                nextMovementState = state;
            }
        }

        if (currentMovementState != nextMovementState)
        {
            if (currentMovementState)
            {
                currentMovementState.OnExit(this);
            }

            if (nextMovementState)
            {
                nextMovementState.OnEnter(this);
            }
        }
    }

    private void ComputeVelocity()
    {
        if (!currentMovementState)
        {
            CurrentVelocity = Vector2.zero;
            return;
        }

        CurrentVelocity = currentMovementState.GetVelocity(this);
    }

    private void Move()
    {
        Vector3 newPosition = CurrentRing.GetPositionOnRing(rb.position, CurrentVelocity);
        var lastPosition = rb.position;
        Vector3 finalVelocity = newPosition - lastPosition;

     

        float deltaTime = Time.deltaTime;
        Vector3 p1 = lastPosition + cc.center + transform.up * (-cc.height * 0.25f);
        Vector3 p2 = p1 + transform.up * cc.height;

        Collider[] colliders = new Collider[16];

        Vector3 collisionOffset = Vector3.zero;

        for (int i = 0; i < maxPenetrationCount; i++)
        {
            Vector3 nextPosition = rb.position + collisionOffset + finalVelocity * deltaTime;
            Vector3 nextP1 = p1 + collisionOffset;
            Vector3 nextP2 = p2 + collisionOffset;
            int count = Physics.OverlapCapsuleNonAlloc(nextP1, nextP2, cc.radius - 0.01f, colliders);
            
            if (count > 0)
            {
                for (int j = 0; j < count; j++)
                {
                    Collider collider = colliders[j];

                    if (collider.attachedRigidbody == rb)
                        continue;

                    if (Physics.GetIgnoreLayerCollision(collider.gameObject.layer, cc.gameObject.layer))
                        continue;

                    if (Physics.GetIgnoreCollision(collider, cc))
                        continue;

                    Vector3 otherPosition = collider.transform.position;
                    Quaternion otherRotation = collider.transform.rotation;

                    if (Physics.ComputePenetration(cc, nextPosition, rb.rotation, collider, otherPosition,
                            otherRotation, out Vector3 direction, out float distance))
                    {
                        Vector3 offset = direction * distance;
                        Debug.DrawLine(rb.position + collisionOffset, rb.position + collisionOffset + offset);
                        collisionOffset += offset;
                    }
                }
            }
            else
            {
                break;
            }
        }
        Vector3 nonOrbitalNewPosition = rb.position + collisionOffset + finalVelocity * deltaTime;
        Vector3 orbitalNewPosition = CurrentRing.ClampToRing(nonOrbitalNewPosition);
        rb.MovePosition(orbitalNewPosition);
    }
    
}