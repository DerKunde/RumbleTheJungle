using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoom : MonoBehaviour
{
    [Header("Portals")]
    [SerializeField] public Portal northPortal;
    [SerializeField] public Portal eastPortal;
    [SerializeField] public Portal southPortal;
    [SerializeField] public Portal westPortal;

    [SerializeField] private RoomContentManager roomContent;
    [SerializeField]
    private RoomType roomType;

    public (int x, int y) roomPosition;

    public enum RoomType
    {
        startRoom,
        corridorRoom,
        altarRoom,
        bossRoom,
        exitRoom
    }
    
    public void SetPortalState(RoomManager.Direction direction, bool portalState)
    {
        switch (direction)
        {
            case RoomManager.Direction.North:
                northPortal.SetPortalState(portalState);
                northPortal.portalDirection = RoomManager.Direction.North;
                break;
            
            case RoomManager.Direction.East:
                eastPortal.SetPortalState(portalState);
                eastPortal.portalDirection = RoomManager.Direction.East;
                break;
            
            case RoomManager.Direction.South:
                southPortal.SetPortalState(portalState);
                southPortal.portalDirection = RoomManager.Direction.South;
                break;
            
            case RoomManager.Direction.West:
                westPortal.SetPortalState(portalState);
                westPortal.portalDirection = RoomManager.Direction.West;
                break;
        }
    }

    public void StartRoomContent()
    {
        roomContent.InitializeRoomContent(roomType);
    }

    public void DisableRoomContent()
    {
        roomContent.PausRoomContent();
    }

    private void Start()
    {
        
    }
    
    
}
