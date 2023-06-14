using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Portal : MonoBehaviour
{
    [SerializeField] public Transform spawnTransform;    
    
    [SerializeField]
    private MeshRenderer renderer;

    private DungeonRoom dungeonRoom;

    [HideInInspector]
    public RoomManager.Direction portalDirection;
    
    public delegate void TriggerEventHandler((int x, int y) position, RoomManager.Direction direction);
    public event TriggerEventHandler OnPortalEnter;

    private void Start()
    {
        dungeonRoom = this.GetComponentInParent<DungeonRoom>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            renderer.material.color = Color.red;
            OnPortalEnter?.Invoke(dungeonRoom.roomPosition, portalDirection);
            // CameraFade.FadeOut();
        }
    }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         renderer.material.color = Color.green;
    //         CameraFade.FadeIn();
    //     }
    // }

    public void SetPortalState(bool state)
    {
        this.gameObject.SetActive(state);
    }
}
