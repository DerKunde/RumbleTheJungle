using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/*
 * Das Skript DungeonRoom steuert die Portale und den RoomContent
 */
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

    public Transform floor;
    public (int x, int y) roomPosition;



    [HideInInspector] public float roomDifficulty = 0.9f;
    

    public enum RoomType
    {
        startRoom,
        corridorRoom,
        altarRoom,
        bossRoom,
        exitRoom
    }
    
    /*
     * Die Methode setzt den benötigten Portalzustand.
     * Dazu muss die Richtung des anzusprenden Portals und der Status übergeben werden.
     */
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
    
    /*
     * WIP
     * Bestimmt die Schwierigkeitsstuffe des Raumes
     */
    private void DetermineRoomDifficulty()
    {
        //TODO: Irgendwie so anbinden, das die roomDifficulty steigt, je näher der Raum am ExitRoom ist.
        // roomDifficulty = Random.Range(0.01f, 0.99f);
        roomDifficulty = 0.8f;
    }
    
    /*
     * Diese Methode startet den Content des Raumes.
     * Dazu gehört Anzahl und Position der Gegener, sowie Props die den Raum gestalten.
     */
    public void StartRoomContent()
    {
        DetermineRoomDifficulty();
        roomContent.InitializeRoomContent(roomType);
    }
    
    /*
     * Wenn der Raum verlassen wird, wird der Content des Raumes wieder deaktiviert.
     */
    public void DisableRoomContent()
    {
        roomContent.PausRoomContent();
    }
}
