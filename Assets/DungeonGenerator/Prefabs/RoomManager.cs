using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/**
 * Das Skript RoomManager verarbeitet das Layout aus Tabasco.cs zu Dungeon Räumen.
 * Desweiteren werden die Portale zu angeschlossenen Räumen gesetzt.
 * Der Übergang ziwschen den Räumen wird ebenfalls in diesem Skript gesteuert.
 */
public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }

    public GameObject palyerPrefab;
    private Transform playerTransfrom;
    public CameraMovement camera;

    private List<Portal> portals;


    private int[,] dungeonLayout;

    [SerializeField] private int offsetX = 100;
    [SerializeField] private int offsetY = 100;

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

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
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

        portals = new List<Portal>();
        Portal[] newPortal = FindObjectsOfType<Portal>();
        foreach (var portal in newPortal)
        {
            AddPortalToList(portal);
        }
    }
    
    /*
     * Die Methode baut das Dungeon auf. Die roomLayoutList wird übesetzt in Prefabs die im 3D Raum platziert werden.
     */
    private void SetupDungeon()
    {
        tabasco.CreateNewDungeon();
        roomLayoutList = tabasco.CreateCordsAndTypeList();
        // roomLayoutList.AddRange(testLayout);
        (int x, int y) startRoomCords = (-1,-1);
        
        
        foreach (var room in roomLayoutList)
        {
            switch (room.roomType)
            {
                case 1: SpawnRoom(startRoomPrefab, (room.x, room.y));
                    startRoomCords = (room.x, room.y);
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

        foreach (var room in roomList)
        {
            if (room.Item2.x == startRoomCords.x && room.Item2.y == startRoomCords.y)
            {
                var playerObject = Instantiate(palyerPrefab, room.Item1.transform.position, Quaternion.identity, this.transform);
                playerTransfrom = playerObject.transform;
                camera.SetupCamera();
                return;
            }
        }
    }
    
    /*
     * Die Methode übernimmt alle notwendigen Schritte das Raum Prefab zu spawnen.
     * Dazu gehört das geprüft wird zu welcher Seite der zu spawnende Raum ein Portal benötigt.
     */
    private void SpawnRoom(GameObject roomPrefab, (int x, int y) position)
    {
        Vector3 roomPosition = new Vector3((position.x * offsetX), 0, (position.y * offsetY));
        var spawnedRoom = 
            Instantiate(roomPrefab, roomPosition, Quaternion.identity, this.gameObject.transform);
        
        CheckPortalConnections(spawnedRoom, position);
        roomList.Add((spawnedRoom, position));
    }
    
    /*
     * Diese Methode verarbeitet ob ein Portal in der entsprechenden Himmelsrichtung benötigt wird.
     */
    private void CheckPortalConnections(GameObject spawnedRoom, (int x, int y) position)
    {
        var dungeonRoom = spawnedRoom.GetComponent<DungeonRoom>();
        dungeonRoom.roomPosition = position;
        
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
    
    /**
     * Diese Mehthode Prüft ob es einen benachbarten Raum gibt.
     * Dazu muss die Koordinate des Raumes und die zu prüfende Himmelsrichtung übergeben werden.
     */
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
    
    /*
     * Diese Methode wickelt den Übergang zwischen zwei Räumen ab.
     * Dazu gehört das verschieben des Spielers und das steuern der Kamera.
     */
    public void TransitionToNextRoom(Direction direction, (int x, int y) roomPosition)
    {
        int x = roomPosition.x;
        int y = roomPosition.y;
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

        GameObject roomToTransitionTo = null;
        
        foreach (var room in roomList)
        {
            if (room.Item2.x == neighborX && room.Item2.y == neighborY)
            {
                roomToTransitionTo = room.Item1;
                break;
            }
        }
        
        if (roomToTransitionTo != null)
        {
            var dungeonRoom = roomToTransitionTo.GetComponent<DungeonRoom>();
            switch (direction)
            {
                case Direction.North:
                    playerTransfrom.position = dungeonRoom.southPortal.spawnTransform.position;
                    break;
                
                case Direction.East:
                    playerTransfrom.position = dungeonRoom.westPortal.spawnTransform.position;
                    break;
                
                case Direction.South:
                    playerTransfrom.position = dungeonRoom.northPortal.spawnTransform.position;
                    break;
                
                case Direction.West:
                    playerTransfrom.position = dungeonRoom.eastPortal.spawnTransform.position;
                    break;
            }
            Debug.Log("Player moved");
            StartCoroutine(DoFadeIn());
            dungeonRoom.StartRoomContent();
        }
        else
        {
            Debug.Log("RoomManagerError: NO ROOM TO TRANSITION TO FOUND!");
        }
    }
    
    /*
     * Dies ist eine Event-Function die ausgelöst wird sobald der Spieler ein Portal betritt.
     */
    private void OnPortalEnterFunction((int x, int y) position, Direction direction)
    {
        TransitionToNextRoom(direction, position);
    }
    
    public enum Direction
    {
        North,
        East,
        South,
        West
    }
    
    
    void AddPortalToList(Portal portal)
    {
        portals.Add(portal);
        portal.OnPortalEnter += OnPortalEnterFunction;
    }

    // Entferne Portal-Instanzen aus der Liste (z.B. beim Zerstören eines Portals)
    void RemovePortalFromList(Portal portal)
    {
        portals.Remove(portal);
        portal.OnPortalEnter -= OnPortalEnterFunction;
    }

    IEnumerator DoFadeIn()
    {
        CameraMovement.cameraState = CameraMovement.CameraState.translationMode;
        yield return new WaitForSeconds(0.2f);
        CameraFade.FadeIn();
    }
}

