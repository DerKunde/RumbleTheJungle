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

    private List<GameObject> roomList;

    [SerializeField]
    private Tabasco tabasco;
    
    private const int NORTH = 0;
    private const int EAST = 1;
    private const int SOUTH = 2;
    private const int WEST = 3;

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

    private void SpawnRoom(GameObject roomPrefab, (int x, int y) position)
    {
        Vector3 roomPosition = new Vector3((position.x * offsetX), 0, (position.y * offsetY));
        var spawnedRoom = 
            Instantiate(roomPrefab, roomPosition, Quaternion.identity, this.gameObject.transform);
        
        roomList.Add(spawnedRoom);
    }

    private void CheckPortalConnections()
    {
        /*
         * Check in which direction must be placed portals
         */
    }

    private bool HasNeighborInDirection(int direction, (int x, int y) coords)
    {
        //Check if Neighbor exists
        return true;
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
