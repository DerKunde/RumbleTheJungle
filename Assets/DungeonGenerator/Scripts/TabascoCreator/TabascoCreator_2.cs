using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;


enum RoomTypes
{
    StartRoom = 1,
    ExitRoom = 2,
    Corridor = 3,
    BossRoom = 4,
    AltarRoom = 5,
}

public class TabascoCreator_2 : MonoBehaviour
{
    [Range(5, 20)]
    [SerializeField] private int gridSize;
    [SerializeField] private int numberOfAltarRooms;
    [SerializeField] private int numberOfBossRooms;
    [SerializeField] private int numberOfExitRooms;

    
    [SerializeField, Range(1,4)] private int maxRangeAltar;
    [SerializeField, Range(1, 4)] private int minRangeAltar;

    [SerializeField] private GameObject emptyCellPrefab;
    [SerializeField] private GameObject startRoomPrefab;
    [SerializeField] private GameObject exitRoomPrefab;
    [SerializeField] private GameObject corridorRoomPrefab;
    [SerializeField] private GameObject altarRoomPrefab;
    [SerializeField] private GameObject bossRoomPrefab;

    private const int START_ROOM = 1;
    private const int EXIT_ROOM = 2;
    private const int CORRIDOR_ROOM = -1;
    private const int ALTAR_ROOM = 4;
    private const int BOSS_ROOM = 5;

    private int[,] grid;
    private (int, int) startPosition;
    private (int, int) exitPosition;
    private (int, int) bossPosition;

    private List<(int, int, int)> specialRoomsInDungeon;
    private List<(int, int)> corridorList;

    [SerializeField] private Transform gridParent;

    // Start is called before the first frame update
    void Start()
    {
        grid = CreateIntGrid(gridSize, -2);
        specialRoomsInDungeon = new List<(int x, int y, int roomType)>();
        corridorList = new List<(int x, int y)>();

        var startPoint = SelectRandomPoint();
        startPosition = startPoint;
        specialRoomsInDungeon.Add((startPoint.x, startPoint.y, START_ROOM));

        fillGrid(new GameObject[gridSize, gridSize], emptyCellPrefab);

        var altarRoom1 = GetRandomPointInRange(startPoint, gridSize - maxRangeAltar , minRangeAltar);
        var altarRoom2 = GetRandomPointInRange(startPoint, gridSize - maxRangeAltar/2, minRangeAltar + 2);

        specialRoomsInDungeon.Add((altarRoom1.x, altarRoom1.y, ALTAR_ROOM));
        specialRoomsInDungeon.Add((altarRoom2.x, altarRoom2.y, ALTAR_ROOM));

        bossPosition = GetRandomPointInRange(startPoint, 12, 8);
        specialRoomsInDungeon.Add((bossPosition.Item1, bossPosition.Item2, BOSS_ROOM));

        exitPosition = GetRandomPointInRange(startPoint, gridSize - 1, gridSize - 10);
        specialRoomsInDungeon.Add((exitPosition.Item1, exitPosition.Item2, EXIT_ROOM));

        Init(startPoint);
    }
    
    private void Init((int x, int y) startPos)
    {
        foreach (var pos in specialRoomsInDungeon)
        {
            switch (pos.Item3)
            {
                case START_ROOM:
                    grid[pos.Item1, pos.Item2] = START_ROOM;
                    break;

                case EXIT_ROOM:
                    grid[pos.Item1, pos.Item2] = -1;
                    break;

                case CORRIDOR_ROOM:
                    grid[pos.Item1, pos.Item2] = CORRIDOR_ROOM;
                    break;

                case ALTAR_ROOM:
                    grid[pos.Item1, pos.Item2] = ALTAR_ROOM;
                    break;

                case BOSS_ROOM:
                    grid[pos.Item1, pos.Item2] = -1;
                    break;
            }
        }

        foreach (var pos in specialRoomsInDungeon)
        {
            if (pos.Item3 == ALTAR_ROOM)
            {
                var pathway = GetPathBetween(startPos, (pos.Item1, pos.Item2));
                if (pathway != null)
                {
                    foreach (var tile in pathway)
                    {
                        grid[tile.Item1, tile.Item2] = CORRIDOR_ROOM;
                        corridorList.Add(tile);
                    }
                }
            }
        }

        var pathToBoss = GetPathBetween((specialRoomsInDungeon[1].Item1, specialRoomsInDungeon[1].Item2),
            (specialRoomsInDungeon[3].Item1, specialRoomsInDungeon[3].Item2));
        
        foreach (var tile in pathToBoss)
        {
            grid[tile.Item1, tile.Item2] = CORRIDOR_ROOM;
            corridorList.Add(tile);
        }

        var closestPoints = GetClosestPoints(corridorList,
            new List<(int, int)> { exitPosition });
        
        var pathToExit = GetPathBetween(closestPoints[0], closestPoints[1]);
        foreach (var tile in pathToExit)
        {
            grid[tile.Item1, tile.Item2] = CORRIDOR_ROOM;
            corridorList.Add(tile);
        }

        grid[exitPosition.Item1, exitPosition.Item2] = EXIT_ROOM;
        grid[bossPosition.Item1, bossPosition.Item2] = BOSS_ROOM;
        TranslateGridToGameObject(grid);
    }


    private List<(int,int)> GetClosestPoints(List<(int, int)> list1, List<(int, int)> list2)
    {
        int shortestDistance = int.MaxValue;
        (int, int) shortestPoint1 = (int.MinValue,int.MinValue);
        (int, int) shortestPoint2 = (int.MinValue, int.MinValue);
         
        
        foreach (var pos1 in list1)
        {
            foreach (var pos2 in list2)
            {
                int distance = GetManhattanDistance(pos1, pos2);

                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    shortestPoint1 = pos1;
                    shortestPoint2 = pos2;
                }
                
            }
        }

        return new List<(int, int)>{shortestPoint1, shortestPoint2};
    }
    
    public int GetManhattanDistance((int x, int y) point1, (int x, int y) point2)
    {
        return Math.Abs(point1.x - point2.x) + Math.Abs(point1.y - point2.y);
    }
    
    private int[,] CreateIntGrid(int gridSize, int defaultValue)
    {
        int[,] gridToCreate = new int[gridSize, gridSize];
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                gridToCreate[x, y] = defaultValue;
            }
        }

        return gridToCreate;
    }

    private void TranslateGridToGameObject(int[,] intArray)
    {
        int width = intArray.GetLength(0);
        int height = intArray.GetLength(1);
        GameObject[,] gameObjectArray = new GameObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int cellState = intArray[x, y];

                switch (cellState)
                {
                    case -2:
                        var emptyCell = Instantiate(emptyCellPrefab, gridParent);
                        emptyCell.transform.position = new Vector3(x, y, 0);
                        gameObjectArray[x, y] = emptyCell;
                        break;

                    case START_ROOM:
                        var startRoom = Instantiate(startRoomPrefab, gridParent);
                        startRoom.transform.position = new Vector3(x, y, 0);
                        gameObjectArray[x, y] = startRoom;
                        break;
                    case EXIT_ROOM:
                        var exitRoom = Instantiate(exitRoomPrefab, gridParent);
                        exitRoom.transform.position = new Vector3(x, y, 0);
                        gameObjectArray[x, y] = exitRoom;
                        break;
                    case CORRIDOR_ROOM:
                        var corridor = Instantiate(corridorRoomPrefab, gridParent);
                        corridor.transform.position = new Vector3(x, y, 0);
                        gameObjectArray[x, y] = corridor;
                        break;
                    case ALTAR_ROOM:
                        var altarRoom = Instantiate(altarRoomPrefab, gridParent);
                        altarRoom.transform.position = new Vector3(x, y, 0);
                        gameObjectArray[x, y] = altarRoom;
                        break;
                    case BOSS_ROOM:
                        var bossRoom = Instantiate(bossRoomPrefab, gridParent);
                        bossRoom.transform.position = new Vector3(x, y, 0);
                        gameObjectArray[x, y] = bossRoom;
                        break;
                }
            }
        }
    }

    private (int x, int y) GetRandomPointInRange((int x, int y) referencePoint, int maxDistance, int minDistance)
    {
        int width = gridSize;
        int height = gridSize;

        int minX = Math.Max(0, referencePoint.x - maxDistance);
        int minY = Math.Max(0, referencePoint.y - maxDistance);
        int maxX = Math.Min(width - 1, referencePoint.x + maxDistance);
        int maxY = Math.Min(height - 1, referencePoint.y + maxDistance);

        Random rand = new Random();
        int x, y;

        do
        {
            x = rand.Next(minX, maxX + 1);
            y = rand.Next(minY, maxY + 1);
        } while (Distance(referencePoint, (x, y)) > maxDistance || Distance(referencePoint, (x, y)) < minDistance);

        return (x, y);
    }

    private static double Distance((int x, int y) point1, (int x, int y) point2)
    {
        int dx = point1.x - point2.x;
        int dy = point1.y - point2.y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    private void fillGrid(GameObject[,] array, GameObject objectToFillWith)
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                var cell = Instantiate(objectToFillWith, gridParent);
                cell.gameObject.transform.position = new Vector3(x, y, 0);
                array[x, y] = cell;
            }
        }
    }

    private List<(int, int)> GetPathBetween((int x, int y) startPos, (int x, int y) targetPos)
    {
        int[,] distance = new int[gridSize, gridSize];
        bool[,] visited = new bool[gridSize, gridSize];
        (int, int)[,] prev = new (int, int)[gridSize, gridSize];
        List<(int, int)> unvisited = new List<(int, int)>();

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                distance[i, j] = int.MaxValue;
                visited[i, j] = false;
                prev[i, j] = (-1, -1);
                unvisited.Add((i, j));
            }
        }

        distance[startPos.x, startPos.y] = 0;

        while (unvisited.Count > 0)
        {
            (int, int) current = unvisited.OrderBy(p => distance[p.Item1, p.Item2]).First();

            if (current.Equals(targetPos))
            {
                break;
            }

            unvisited.Remove(current);
            visited[current.Item1, current.Item2] = true;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }
                    if (i != 0 && j != 0)
                    {
                        continue;
                    }

                    (int, int) neighbor = (current.Item1 + i, current.Item2 + j);

                    if (neighbor.Item1 < 0 || neighbor.Item1 >= gridSize || neighbor.Item2 < 0 ||
                        neighbor.Item2 >= gridSize)
                    {
                        continue;
                    }

                    if (visited[neighbor.Item1, neighbor.Item2])
                    {
                        continue;
                    }

                    int alt = distance[current.Item1, current.Item2] + GetDistance(current, neighbor);

                    if (alt < distance[neighbor.Item1, neighbor.Item2])
                    {
                        distance[neighbor.Item1, neighbor.Item2] = alt;
                        prev[neighbor.Item1, neighbor.Item2] = current;
                    }
                }
            }
        }

        List<(int, int)> path = new List<(int, int)>();
        (int, int) currentPos = targetPos;

        while (!currentPos.Equals(startPos))
        {
            path.Insert(0, currentPos);
            currentPos = prev[currentPos.Item1, currentPos.Item2];
        }

        if (path.Count > 0)
        {
            path.RemoveAt(path.Count - 1);

        }
        return path;
    }

    private int GetDistance((int, int) pos1, (int, int) pos2)
    {
        int dx = pos1.Item1 - pos2.Item1;
        int dy = pos1.Item2 - pos2.Item2;

        return (int)Math.Sqrt(dx * dx + dy * dy);
    }

    private (int x, int y) SelectRandomPoint()
    {
        System.Random rand = new System.Random();
        int x = rand.Next(gridSize);
        int y = rand.Next(gridSize);

        return (x, y);
    }
}