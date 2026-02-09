using UnityEngine;

public class ConstrainOffsetLook : MonoBehaviour
{
    [Header("Targets")]
    public Transform offsetObject;    // Child of AR camera
    public Transform neckBone;        // Neck/head bone

    [Header("Constraints")]
    public float maxDistance = 5f;
    public float maxAngle = 60f;
    public float minDistance = 0.5f;

    void LateUpdate()
    {
        if (offsetObject == null) return;

        Vector3 charPos = transform.position;
        Vector3 charForward = transform.forward;
        Vector3 toOffset = offsetObject.position - charPos;
        float dist = toOffset.magnitude;
        toOffset.Normalize();

        float dot = Vector3.Dot(charForward, toOffset);
        float maxDot = Mathf.Cos(maxAngle * Mathf.Deg2Rad);

        Vector3 clampedDir;
        if (dot < maxDot)
        {
            // Project to cone edge
            Vector3 axis = Vector3.Cross(charForward, toOffset).normalized;
            clampedDir = Quaternion.AngleAxis(maxAngle, axis) * charForward;
        }
        else
        {
            clampedDir = toOffset;
        }

        Vector3 clampedPos = charPos + clampedDir.normalized * Mathf.Clamp(dist, minDistance, maxDistance);
        offsetObject.position = clampedPos;

        if (neckBone != null)
        {
            neckBone.LookAt(offsetObject);
        }
    }
}
