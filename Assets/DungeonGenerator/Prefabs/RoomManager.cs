using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private int[,] dungeonLayout;

    [SerializeField] private int offsetX = 5;
    [SerializeField] private int offsetY = 5;

    [SerializeField] private GameObject startRoomPrefab;
    [SerializeField] private GameObject exitRoomPrefab;
    [SerializeField] private GameObject corridorRoomPrefab;
    [SerializeField] private GameObject altarRoomPrefab;
    [SerializeField] private GameObject bossRoomPrefab;

    [SerializeField]
    private Tabasco tabasco;
    
    public void SetDungeonLayout(int[,] grid)
    {
        dungeonLayout = grid;
    }

    public void Start()
    {
        SetupDungeon();
    }

    private void SetupDungeon()
    {
        tabasco.CreateNewDungeon();
        var roomsToSpawn = tabasco.CreateCordsAndTypeList();

        foreach (var room in roomsToSpawn)
        {
            switch (room.roomType)
            {
                case 1: SpawnRoom(startRoomPrefab, (room.x, room.y));
                    break;
                
                case 2: SpawnRoom(exitRoomPrefab, (room.x, room.y));
                    break;
                
                case 3: SpawnRoom(corridorRoomPrefab, (room.x, room.y));
                    break;
                
                case 4: SpawnRoom(altarRoomPrefab, (room.x, room.y));
                    break;
                
                case 5: SpawnRoom(bossRoomPrefab, (room.x, room.y));
                    break;
            }
        }
    }

    private void SpawnRoom(GameObject roomPrefab, (int, int) position)
    {
        Vector3 roomPosition = new Vector3((position.Item1 * offsetX), 0, (position.Item2 * offsetY));
        var spawnedRoom = 
            Instantiate(roomPrefab, roomPosition, Quaternion.identity, this.gameObject.transform);
    }

    private void CheckPortalConnections()
    {
        
    }

    private (int, int) FindStartRoom()
    {
        for (int x = 0; x < dungeonLayout.GetLength(0); x++)
        {
            for (int y = 0; y < dungeonLayout.GetLength(1); y++)
            {
                if (dungeonLayout[x, y] == 1)
                {
                    return (x, y);
                }
            }
        }

        return (-1, -1);
    }
    
    
    
    
    
}
