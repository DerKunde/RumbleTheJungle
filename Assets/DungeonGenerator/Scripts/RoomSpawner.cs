using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomSpawner : MonoBehaviour
{
    public enum DoorDirections
    {
        north,
        east,
        south,
        west
    }

    private RoomTemplates templates;
    private int randomIndex;
    private bool spawned = false;
    

    public DoorDirections doorDirection;

    private void Awake()
    {
        templates = GameObject.FindGameObjectWithTag("RoomTemplates").GetComponent<RoomTemplates>();
    }

    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("RoomTemplates").GetComponent<RoomTemplates>();
        if (DungeonGenManager.Instance.enableRoomSpawning)
        {
            Invoke("Spawn", 0.1f);   
        }
    }
    
    private void Spawn()
    {
        if (!spawned)
        {
            if (doorDirection == DoorDirections.north)
            {
                //Spawn room with south 
                randomIndex = Random.Range(0, templates.northDoorRooms.Length);
                Instantiate(templates.northDoorRooms[randomIndex], transform.position, templates.northDoorRooms[randomIndex].transform.rotation);
            }
            else if (doorDirection == DoorDirections.east)
            {
                //spawn room with west door
                randomIndex = Random.Range(0, templates.eastDoorRooms.Length);
                Instantiate(templates.eastDoorRooms[randomIndex], transform.position, templates.eastDoorRooms[randomIndex].transform.rotation);
            }
            else if (doorDirection == DoorDirections.south)
            {
                //spawn room with north door
                randomIndex = Random.Range(0, templates.southDoorRooms.Length);
                Instantiate(templates.southDoorRooms[randomIndex], transform.position, templates.southDoorRooms[randomIndex].transform.rotation);
            }
            else if (doorDirection == DoorDirections.west)
            {
                //spawn room with east door
                randomIndex = Random.Range(0, templates.westDoorRooms.Length);
                Instantiate(templates.westDoorRooms[randomIndex], transform.position, templates.westDoorRooms[randomIndex].transform.rotation);
            }

            spawned = true;
        }
    }

    public void SpawnOneWayRoom()
    {
        Debug.Log("OneWayRoom");
        if (doorDirection == DoorDirections.north)
        {
            Instantiate(templates.northDoorRooms[0], transform.position,
                templates.northDoorRooms[0].transform.rotation);
        }
        else if (doorDirection == DoorDirections.east)
        {
            Instantiate(templates.eastDoorRooms[0], transform.position,
                templates.eastDoorRooms[0].transform.rotation);
        }
        else if (doorDirection == DoorDirections.south)
        {
            Instantiate(templates.southDoorRooms[0], transform.position,
                templates.southDoorRooms[0].transform.rotation);
        }
        else if (doorDirection == DoorDirections.west)
        {
            Instantiate(templates.westDoorRooms[0], transform.position,
                templates.westDoorRooms[0].transform.rotation);
        }

        spawned = true;
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RoomSpawnPoint"))
        {
            Debug.Log("Room spawn point deleted!");
            GetComponentInParent<Room>().roomSpawners.Remove(this);
            Destroy(gameObject);
        }
    }
}
