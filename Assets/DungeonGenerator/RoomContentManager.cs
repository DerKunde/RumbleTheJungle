using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RoomContentManager : MonoBehaviour
{
    [SerializeField] private GameObject worldParent;
    [SerializeField] private GameObject enemyParent;

    [SerializeField] private GameObject referencePoint;


    [SerializeField] private List<GameObject> worldPropPrefabs;
    [SerializeField] private List<GameObject> enemySpawnpointPrefabs;

    
    public void InitializeRoomContent(DungeonRoom.RoomType roomType)
    {
        switch (roomType)
        {
            case DungeonRoom.RoomType.startRoom:
                InitializeStartRoom();
                break;
            
            case DungeonRoom.RoomType.corridorRoom:
                InitializeCorridorRoom();
                break;
            
            case DungeonRoom.RoomType.altarRoom:
                InitializeAltarRoom();
                break;
            
            case DungeonRoom.RoomType.bossRoom:
                InitializeBossRoom();
                break;
            
            case DungeonRoom.RoomType.exitRoom:
                InitializeExitRoom();
                break;
        }
    }

    public void PausRoomContent()
    {
        
    }

    private void InitializeStartRoom()
    {
        SpawnPropPrefab(worldPropPrefabs[0]);
    }
    
    private void InitializeCorridorRoom()
    {
        SpawnPropPrefab(worldPropPrefabs[0]);

    }
    
    
    private void InitializeAltarRoom()
    {
        SpawnPropPrefab(worldPropPrefabs[0]);

    }
    
    private void InitializeBossRoom()
    {
        SpawnPropPrefab(worldPropPrefabs[0]);

    }
    
    private void InitializeExitRoom()
    {
        SpawnPropPrefab(worldPropPrefabs[0]);

    }


    private void SpawnPropPrefab(GameObject propPrefab)
    {
        var spawnedPropset = Instantiate(propPrefab, referencePoint.transform.position, quaternion.identity, worldParent.transform);

    }

    private void SpawnEnemys()
    {
        
    }
    
    
    
}
