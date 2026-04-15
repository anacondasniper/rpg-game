using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Offset")]
    public Vector3 offset = new Vector3(0f, 10f, -7f);
    public float followSpeed = 12f;

    [Header("Rotation")]
    public float pitchAngle = 50f;

    void Start()
    {
        transform.rotation = Quaternion.Euler(pitchAngle, 0f, 0f);
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPos, followSpeed * Time.deltaTime);
    }
}
