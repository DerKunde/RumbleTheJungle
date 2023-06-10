using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class Tabasco : MonoBehaviour
{
    [Range(5, 20)] [SerializeField] private int width;
    [Range(5, 20)] [SerializeField] private int height;
    
    [Header ("Special Rooms Altar")]
    [SerializeField] private int numberOfAltars;
    [SerializeField] private int altarMinDistanceToStart;
    [SerializeField] private int altarMaxDistanceToStart;

    [Header ("Special Rooms Boss")]
    [SerializeField] private int numberOfBosses;
    [SerializeField] private int bossMinDistanceToStart;
    [SerializeField] private int bossMaxDistanceToStart;
    
    [Header ("Special Rooms Exit")]
    [SerializeField] private int numberOfExits;
    [SerializeField] private int exitMinDistanceToStart;
    [SerializeField] private int exitMaxDistanceToStart;


    private int[,] grid;

    private (int, int) startPoint;

    private List<(int, int, int)> markerList;
    private List<(int, int)> altarList;
    private List<(int, int)> corridorList;
    private List<(int, int)> bossList;
    private List<(int, int)> exitList;


    private const int START_ROOM = 1;
    private const int EXIT_ROOM = 2;
    private const int CORRIDOR_ROOM = 3;
    private const int ALTAR_ROOM = 4;
    private const int BOSS_ROOM = 5;

    void Start()
    {
        markerList = new List<(int x, int y, int type)>();
        altarList = new List<(int x, int y)>();
        corridorList = new List<(int x, int y)>();
        bossList = new List<(int x, int y)>();
        exitList = new List<(int x, int y)>();

    }

    public List<(int x, int y, int roomType)> CreateCordsAndTypeList()
    {
        List<(int x, int y, int roomType)> roomList = new List<(int x, int y, int roomType)>();

        roomList.Add((startPoint.Item1, startPoint.Item2, 1));
        foreach (var roomCord in altarList)
        {
            roomList.Add((roomCord.Item1, roomCord.Item2, 4));
        }

        foreach (var roomCord in corridorList)
        {
            roomList.Add((roomCord.Item1, roomCord.Item2, 3));
        }

        foreach (var roomCord in bossList)
        {
            roomList.Add((roomCord.Item1, roomCord.Item2, 5));
        }

        foreach (var roomCord in exitList)
        {
            roomList.Add((roomCord.Item1, roomCord.Item2, 2));
        }

        return RemoveDuplicates(roomList);
    }

    public int[,] CreateNewDungeon()
    {
        altarList.Clear();
        bossList.Clear();
        exitList.Clear();
        markerList.Clear();
        corridorList.Clear();
        
        
        grid = CreateIntGrid(width, height, -2);
        
        markerList = SetupMarkers(numberOfAltars, numberOfBosses, numberOfExits);
        SetMarkersInGrid();
        
        //Connect start with altars
        SetPathToTargets(startPoint, altarList, ALTAR_ROOM);
        
        //Get Closest Altar to Boss
        var alterToBoss = GetClosestPoints(altarList, bossList);
        //Path between the Points
        SetPathToTarget(alterToBoss[0], alterToBoss[1]);
        
        //Assemble Corridor + startPoint List
        List<(int, int)> roomList = new List<(int, int)>(corridorList);
        roomList.Add(startPoint);
        
        //GetClosestRoomToExit
        var roomsToExit = GetClosestPoints(roomList, exitList);
        SetPathToTarget(roomsToExit[0], roomsToExit[1]);
        
        ResetMarkes(exitList, EXIT_ROOM);
        ResetMarkes(bossList, BOSS_ROOM);

        return grid;
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

    private void SetPathToTargets((int, int) start, List<(int, int)> targets, int type)
    {
        foreach (var position in targets)
        {
            var pathway = GetPathBetween(start, position);
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

    private void SetPathToTarget((int, int) start, (int, int) target)
    {
        var pathway = GetPathBetween(start, target);
        if (pathway != null)
        {
            foreach (var tile in pathway)
            {
                grid[tile.Item1, tile.Item2] = CORRIDOR_ROOM;
                corridorList.Add(tile);
            }
        }
    }

    private void ResetMarkes(List<(int, int)> positions, int type)
    {
        foreach (var position in positions)
        {
            grid[position.Item1, position.Item2] = type;
        }
    }

    private void SetMarkersInGrid()
    {
        foreach (var pos in markerList)
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
    }

    private List<(int, int)> GetPathBetween((int x, int y) startPos, (int x, int y) targetPos)
    {
        int[,] distance = new int[width, height];
        bool[,] visited = new bool[width, height];
        (int, int)[,] prev = new (int, int)[width, height];
        List<(int, int)> unvisited = new List<(int, int)>();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
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

                    if (neighbor.Item1 < 0 || neighbor.Item1 >= width || neighbor.Item2 < 0 ||
                        neighbor.Item2 >= height)
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

    private List<(int, int, int)> SetupMarkers(int numberOfAltarRooms, int numberOfBossRooms, int numberOfExitRooms)
    {
        startPoint = SelectRandomPoint();
        markerList.Add((startPoint.Item1, startPoint.Item2, START_ROOM));
        
        for (int i = 1; i <= numberOfAltarRooms; i++)
        {
            var tempPoint = GetRandomPointInRange(startPoint, altarMaxDistanceToStart, altarMinDistanceToStart);
            altarList.Add(tempPoint);
            markerList.Add((tempPoint.x, tempPoint.y, ALTAR_ROOM));
        }

        for (int i = 1; i <= numberOfBossRooms; i++)
        {
            var tempPoint = GetRandomPointInRange(startPoint, bossMaxDistanceToStart, bossMinDistanceToStart);
            bossList.Add(tempPoint);
            markerList.Add((tempPoint.x, tempPoint.y, BOSS_ROOM));
        }

        for (int i = 1; i <= numberOfExitRooms; i++)
        {
            var tempPoint = GetRandomPointInRange(startPoint, exitMaxDistanceToStart, exitMinDistanceToStart);
            exitList.Add(tempPoint);
            markerList.Add((tempPoint.x, tempPoint.y, EXIT_ROOM));
        }

        return markerList;
    }

    private (int x, int y) GetRandomPointInRange((int x, int y) referencePoint, int maxDistance, int minDistance)
    {
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

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

    private int[,] CreateIntGrid(int width, int height, int defaultValue)
    {
        int[,] gridToCreate = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                gridToCreate[x, y] = defaultValue;
            }
        }

        return gridToCreate;
    }

    private void CheckListForDoubles(List<(int, int)> listToCheck)
    {
        var uniquePoints = new HashSet<(int, int)>(listToCheck);
        var numDuplicates = listToCheck.Count - uniquePoints.Count;
        if (numDuplicates > 0)
        {
            listToCheck.Clear();
            listToCheck.AddRange(uniquePoints);
            while (listToCheck.Count < numDuplicates + uniquePoints.Count)
            {
                listToCheck.Add(GetRandomPointInRange(startPoint, 10, 4));
            }
        }
    }
    
    public int GetManhattanDistance((int x, int y) point1, (int x, int y) point2)
    {
        return Math.Abs(point1.x - point2.x) + Math.Abs(point1.y - point2.y);
    }
    
    private (int x, int y) SelectRandomPoint()
    {
        System.Random rand = new System.Random();
        int x = rand.Next(width);
        int y = rand.Next(height);

        return (x, y);
    }

    private List<(int x, int y, int roomType)> RemoveDuplicates(List<(int x, int y, int roomType)> list)
    {
        HashSet<(int x, int y, int roomType)> set = new HashSet<(int x, int y, int roomType)>(list);
        return new List<(int x, int y, int roomType)>(set);
    }
}