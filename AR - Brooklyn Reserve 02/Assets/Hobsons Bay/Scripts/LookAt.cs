using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [Header("Targets & Speed")]
    public Transform targetCamera;  // Assign AR Camera
    public float turnSpeed = 5f;    // Degrees per second

    [Header("Cone Limit (soft fallback)")]
    public float maxConeAngle = 60f;  // Max degrees from forward before clamping

    [Header("Hard Euler Limits (degrees, local space)")]
    public Vector2 pitchLimits = new Vector2(-45f, 45f);   // X: up/down
    public Vector2 yawLimits = new Vector2(-70f, 70f);     // Y: left/right
    public Vector2 rollLimits = new Vector2(-20f, 20f);    // Z: tilt

    void Update()
    {
        if (targetCamera == null) return;

        Vector3 directionToTarget = (targetCamera.position - transform.position).normalized;
        Vector3 forward = transform.forward;
        float angle = Vector3.Angle(forward, directionToTarget);

        Quaternion targetRot;
        if (angle <= maxConeAngle)
        {
            targetRot = Quaternion.LookRotation(directionToTarget);
        }
        else
        {
            // Clamp to cone edge
            Vector3 axis = Vector3.Cross(forward, directionToTarget).normalized;
            targetRot = Quaternion.AngleAxis(maxConeAngle, axis) * Quaternion.LookRotation(forward);
        }

        // Apply hard Euler clamps
        targetRot = ClampEuler(targetRot);

        // Smooth rotate
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
    }

    Quaternion ClampEuler(Quaternion rot)
    {
        Vector3 euler = rot.eulerAngles;
        euler.x = NormalizeAngle(euler.x); euler.y = NormalizeAngle(euler.y); euler.z = NormalizeAngle(euler.z);

        euler.x = Mathf.Clamp(euler.x, pitchLimits.x, pitchLimits.y);
        euler.y = Mathf.Clamp(euler.y, yawLimits.x, yawLimits.y);
        euler.z = Mathf.Clamp(euler.z, rollLimits.x, rollLimits.y);

        return Quaternion.Euler(euler);
    }

    float NormalizeAngle(float angle)
    {
        while (angle > 180f) angle -= 360f;
        while (angle < -180f) angle += 360f;
        return angle;
    }
}


