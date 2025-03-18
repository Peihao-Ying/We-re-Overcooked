using UnityEngine;

public class CameraFollowWorldStrict : MonoBehaviour
{
    public Transform target;
    public Vector3 worldOffset = new Vector3(-1, 28, -32);
    private Quaternion fixedRotation;

    void Start()
    {
        fixedRotation = transform.rotation;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + worldOffset;

            transform.rotation = fixedRotation;
        }
    }
}
