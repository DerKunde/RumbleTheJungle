using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonGenManager : MonoBehaviour
{
    
    public static DungeonGenManager Instance { get; private set; }
    public List<Room> spawnedRooms = new List<Room>();
    public bool enableRoomSpawning = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    [SerializeField] private int maxNumberOfRooms;
    [SerializeField] private int countedRooms;
    
    public 
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (countedRooms >= maxNumberOfRooms)
        {
            enableRoomSpawning = false;
            foreach (var room in spawnedRooms)
            {
                if (room.roomSpawners.Count > 0)
                {
                    foreach (var spawner in room.roomSpawners)
                    {
                        spawner.SpawnOneWayRoom();
                    }   
                }
            }
        }
    }

    public void pingRoomCreated()
    {
        countedRooms++;
    }
}
