using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private int[,] dungeonLayout;

    [SerializeField] private int offsetX = 10;
    [SerializeField] private int offsetY = 10;

    [SerializeField] private GameObject startRoomPrefab;
    [SerializeField] private GameObject exitRoomPrefab;
    [SerializeField] private GameObject corridorRoomPrefab;
    [SerializeField] private GameObject altarRoomPrefab;
    [SerializeField] private GameObject bossRoomPrefab;

    private List<(GameObject, (int x, int y))> roomList = new List<(GameObject, (int x, int y))>();
    private List<(int x, int y, int roomType)> roomLayoutList = new List<(int x, int y, int roomType)>();

    private List<(int x, int y, int roomType)> testLayout = new List<(int x, int y, int roomType)>();
    

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
        testLayout = new List<(int x, int y, int roomType)>
        {
            (0,0,1),
            (0,1,1),
            (0,2,1),
            (0,3,1),
            (0,4,1),
            (0,5,1),
            (0,6,1)
        };
        
        SetupDungeon();
    }

    private void SetupDungeon()
    {
        tabasco.CreateNewDungeon();
        roomLayoutList = tabasco.CreateCordsAndTypeList();
        // roomLayoutList.AddRange(testLayout);
        
        foreach (var room in roomLayoutList)
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
        
        CheckPortalConnections(spawnedRoom, position);
        roomList.Add((spawnedRoom, position));
    }

    private void CheckPortalConnections(GameObject spawnedRoom, (int x, int y) position)
    {
        var dungeonRoom = spawnedRoom.GetComponent<DungeonRoom>();
        
        if (!HasNeighborRoom(position.x, position.y, Direction.North))
        {
            dungeonRoom.SetPortalState(Direction.North, false);
        }

        if (!HasNeighborRoom(position.x, position.y, Direction.East))
        {
            dungeonRoom.SetPortalState(Direction.East, false);
        }

        if (!HasNeighborRoom(position.x, position.y, Direction.South))
        {
            dungeonRoom.SetPortalState(Direction.South, false);
        }

        if (!HasNeighborRoom(position.x, position.y, Direction.West))
        {
            dungeonRoom.SetPortalState(Direction.West, false);
        }
    }

    public bool HasNeighborRoom(int x, int y, Direction direction)
    {
        // Ermittle die Koordinaten des Nachbarraums basierend auf der angegebenen Himmelsrichtung
        int neighborX = x;
        int neighborY = y;
        switch (direction)
        {
            case Direction.North:
                neighborY = y + 1;
                break;
            case Direction.South:
                neighborY = y - 1;
                break;
            case Direction.East:
                neighborX = x + 1;
                break;
            case Direction.West:
                neighborX = x - 1;
                break;
        }

        // Überprüfe, ob der Nachbarraum existiert und einen anderen Koordinatenwert hat als der aktuelle Raum
        foreach (var roomObject in roomLayoutList)
        {
            (int x, int y) roomPostion = (roomObject.x, roomObject.y);
            
            if (roomPostion.x == neighborX && roomPostion.y == neighborY && (roomPostion.x != x || roomPostion.y != y))
            {
                return true;
            }
        }

        return false;
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
    
    public enum Direction
    {
        North,
        East,
        South,
        West
    }
}

