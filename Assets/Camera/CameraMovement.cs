using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField] private Transform HubFloor;

    private Vector3 offset;

    [SerializeField, Range(0.2f, 10f)]
    private float speed = 2f;

    private bool cameraReady = false;
    public static CameraState cameraState;
    
    private Vector3 referenzPoint;

    [SerializeField] private float paddingX = 2f;
    [SerializeField] private float paddingZ = 2f;
    private float minX, maxX, minZ, maxZ;


    public enum CameraState
    {
        normalMode,
        translationMode,
        blockedInX,
        blockedInZ
    }


    private void Start()
    {
        if (target != null)
        {
            SetupCamera();
            setRoomReferenzPoint(HubFloor);
        }
    }

    private void LateUpdate()
    {
        if (cameraReady)
        {
            switch (cameraState)
            {
                case CameraState.normalMode:
                    float targetX = Mathf.Clamp(target.position.x, minX, maxX);
                    float targetY = transform.position.y;
                    float targetZ = Mathf.Clamp(target.position.z, minZ, maxZ);

                    Vector3 desiredPosition = new Vector3(targetX, targetY, targetZ) + offset;
                    desiredPosition.Set(desiredPosition.x, transform.position.y, desiredPosition.z);
                    transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * speed);
                    Debug.Log("DesiredCameraPos: " + desiredPosition);
                    break;
                
                case CameraState.translationMode:
                    Vector3 desiredTranslation = target.position + offset;
                    transform.position = desiredTranslation;
                    // transform.position = desiredPosition;
                    cameraState = CameraState.normalMode;
                    break;
            }
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

    public void setRoomReferenzPoint(DungeonRoom dungeonRoom)
    {
        var floor = dungeonRoom.floor;
        
        var floorWidth = floor.GetComponent<FloorDimensions>().floorWidth;
        var floorHeight = floor.GetComponent<FloorDimensions>().floorHeight;
        var floorPosition = floor.transform.position;

        minX = floorPosition.x - (floorWidth/2) + paddingX;
        maxX = floorPosition.x + (floorWidth/2) - paddingX;
        minZ = floorPosition.z - (floorHeight/2) + paddingZ;
        maxZ = floorPosition.z + (floorHeight/2) - paddingZ;
    }
    
    public void setRoomReferenzPoint(Transform floor)
    {
        var floorWidth = floor.GetComponent<FloorDimensions>().floorWidth;
        var floorHeight = floor.GetComponent<FloorDimensions>().floorHeight;
        var floorPosition = floor.transform.position;

        minX = floorPosition.x - (floorWidth/2) + paddingX;
        maxX = floorPosition.x + (floorWidth/2) - paddingX;
        minZ = floorPosition.z - (floorHeight/2) + paddingZ;
        maxZ = floorPosition.z + (floorHeight/2) - paddingZ;
    }
}
