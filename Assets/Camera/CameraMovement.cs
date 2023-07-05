using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Das Skirpt CameraMovement steuert die Kamera. Das beinhaltet die Bewegung der Kamer um dem Spieler zu folgen, als auch
 * den FadeIn und FadeOut für den Raumübergang.
 * Außerdem gibt es zwei Zustönde in der sich die Kamera befinden kann. Im NormalMode folgt die Kamera mit einer leichten
 * Latenz dem Spieler. Im TranslationMode nimmt die Kamera unmittelbar die angeforderte Position ein.
 */
public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private Vector3 offset;

    [SerializeField, Range(0.2f, 10f)]
    private float speed = 2f;

    private bool cameraReady = false;
    public static CameraState cameraState;
    
    public enum CameraState
    {
        normalMode,
        translationMode
    }


    private void Start()
    {
        if (target != null)
        {
            SetupCamera();
        }
    }

    private void LateUpdate()
    {
        if (cameraReady)
        {
            switch (cameraState)
            {
                case CameraState.normalMode:
                    Vector3 desiredPosition = target.position + offset;
                    transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * speed);
                    // transform.position = desiredPosition;
                    break;
                
                case CameraState.translationMode:
                    Vector3 desiredTranslation = target.position + offset;
                    transform.position = Vector3.Lerp(transform.position, desiredTranslation, Time.deltaTime * 1000);
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
}
