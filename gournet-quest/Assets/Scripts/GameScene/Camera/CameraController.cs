using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform target;
    [Header("===== Follow =====")]
    [SerializeField] float smoothSpeed = 0.125f;

    private void LateUpdate()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        if (target != null)
        {
            Vector3 desiredPostion = target.position;
            Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPostion, smoothSpeed);
            transform.position = smoothedPos;
        }
    }

    public void SetupCamera(Transform target)
    {
        this.target = target;
        Camera.SetupCurrent(transform.GetChild(0).GetComponent<Camera>());
    }

}