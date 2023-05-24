using System;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private Camera camera;
    public static Camera CameraInstance { get; private set; }

    [SerializeField] private GameObject avatarPosition;

    private void Awake()
    {
        if (CameraInstance != null && CameraInstance != this)
        {
            Destroy(this);
        }
        else
        {
            CameraInstance = camera;
        }
    }

    private void LateUpdate()
    {
        CameraInstance.gameObject.transform.position = new Vector3(avatarPosition.gameObject.transform.position.x ,CameraInstance.gameObject.transform.position.y, avatarPosition.gameObject.transform.position.z - 5);
    }
}
