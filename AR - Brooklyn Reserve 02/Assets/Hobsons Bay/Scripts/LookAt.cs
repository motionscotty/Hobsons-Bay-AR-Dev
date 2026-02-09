using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform targetCamera;      // AR camera
    public float turnSpeed = 5f;        // How fast the head turns
    public float maxAngle = 60f;        // Max degrees away from forward

    void Update()
    {
        if (targetCamera == null) return;

        // Direction from this object to the camera
        Vector3 toTarget = (targetCamera.position - transform.position).normalized;

        // Desired rotation to look at the camera
        Quaternion targetRot = Quaternion.LookRotation(toTarget);

        // How far is that from our current forward?
        float angle = Quaternion.Angle(Quaternion.LookRotation(transform.forward), targetRot);

        // If the camera is too far behind/sideways, clamp by rotating towards it but only up to maxAngle
        if (angle > maxAngle)
        {
            // Rotation that is maxAngle away from current forward toward the camera
            targetRot = Quaternion.RotateTowards(
                Quaternion.LookRotation(transform.forward),
                targetRot,
                maxAngle
            );
        }

        // Smoothly rotate towards the (possibly clamped) target rotation
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            turnSpeed * Time.deltaTime
        );
    }
}


