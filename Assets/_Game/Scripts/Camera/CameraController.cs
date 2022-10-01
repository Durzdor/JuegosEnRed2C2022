using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform followTarget;
    [SerializeField] Vector3 offset;
    [SerializeField] Vector3 rotation;

    public void SetTarget(Transform target)
    {
        followTarget = target;
    }

    public void LateUpdate()
    {
        if (!followTarget) return;
        transform.position = followTarget.position + offset;
        transform.rotation = Quaternion.Euler(rotation);
    }
}
