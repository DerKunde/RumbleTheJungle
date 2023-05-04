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

    private void Start()
    {
        offset = transform.position - target.position;
    }


    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * speed);
    }
}
