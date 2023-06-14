using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private Vector3 offset;

    [SerializeField, Range(0.2f, 10f)]
    private float speed = 2f;

    private bool cameraReady = false;
    

    private void LateUpdate()
    {
        if (cameraReady)
        {
            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * speed);
            // transform.position = desiredPosition;
        }
    }
    
    /*
     * Make sure Player is spawned befor Camera Setup is used.
     */
    public void SetupCamera()
    {
        var targetObject = GameObject.FindWithTag("Player").transform;
        var targetPosition = targetObject.transform.position;
        target = targetObject;
        this.transform.position += targetPosition;
        offset = this.transform.position - targetPosition;
        cameraReady = true;
    }

    public void SetTarget(Transform transformTarget)
    {
        target = transformTarget;
    }
}
