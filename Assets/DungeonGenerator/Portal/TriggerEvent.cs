using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : MonoBehaviour
{ 
    public delegate void TriggerEventHandler((int x, int y) position, RoomManager.Direction direction);

    public event TriggerEventHandler OnTriggerActivated;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
        }
    }
}
