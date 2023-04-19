using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<RoomSpawner> roomSpawners = new List<RoomSpawner>();
    void Start()
    {
        DungeonGenManager.Instance.pingRoomCreated();
        DungeonGenManager.Instance.spawnedRooms.Add(this);
        roomSpawners = this.GetComponentsInChildren<RoomSpawner>().ToList();
    }
    
}
