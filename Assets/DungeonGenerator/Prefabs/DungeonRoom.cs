using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoom : MonoBehaviour
{
    [Header("Portals")]
    [SerializeField] private Portal northPortal;
    [SerializeField] private Portal eastPortal;
    [SerializeField] private Portal southPortal;
    [SerializeField] private Portal westPortal;


    public void SetPortalState(RoomManager.Direction direction, bool portalState)
    {
        switch (direction)
        {
            case RoomManager.Direction.North:
                northPortal.SetPortalState(portalState);
                break;
            
            case RoomManager.Direction.East:
                eastPortal.SetPortalState(portalState);
                break;
            
            case RoomManager.Direction.South:
                southPortal.SetPortalState(portalState);
                break;
            
            case RoomManager.Direction.West:
                westPortal.SetPortalState(portalState);
                break;
        }
    }
    
    
}
