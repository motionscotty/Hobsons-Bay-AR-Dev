using UnityEngine;

public class ConstrainOffsetLook : MonoBehaviour
{
    [Header("Targets")]
    public Transform offsetObject;
    public Transform neckBone;

    [Header("Constraints")]
    public float maxDistance = 5f;
    public float maxAngle = 60f;
    public float minDistance = 0.5f;

    [Header("Offsets")]
    // Euler angles applied after LookAt, in the bone's local space.
    // For a backwards head: set Y = 180. For a sideways head: try X or Z.
    public Vector3 rotationOffset = Vector3.zero;

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
            neckBone.rotation *= Quaternion.Euler(rotationOffset); // local-space offset
        }
    }
}
