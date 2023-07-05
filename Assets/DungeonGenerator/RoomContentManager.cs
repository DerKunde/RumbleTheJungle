using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
/*
 * Das Skript RoomContentManager bestimmt welcher Conten in den Raum geladen werden soll.
 * Dazu hält dieses Skript verschiedene Presets die zufällig ausgewählt werden um das Dungeon dynamisch zu gestalten.
 */
public class RoomContentManager : MonoBehaviour
{
    [SerializeField] private GameObject worldParent;
    [SerializeField] private GameObject enemyParent;

    [SerializeField] private GameObject piedraPrefab;
    [SerializeField] private GameObject bushPrefab;

    [SerializeField] private GameObject referencePoint;


    [SerializeField] private List<GameObject> worldPropPrefabs;
    [SerializeField] private List<GameObject> enemySpawnpointPrefabs;

    private DungeonRoom dungeonRoom;

    /*
     * Wird vom jeweiligen Raum aufgerufen um zu bestimmen welcher Content in den Raum gesetzt wird.
     */
    public void InitializeRoomContent(DungeonRoom.RoomType roomType)
    {
        dungeonRoom = this.GetComponentInParent<DungeonRoom>();
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
        
        InitializeEnemys();
    }
    
    public void PausRoomContent()
    {
        worldParent.SetActive(false);
    }

    private void InitializeStartRoom()
    {
        var randomIndex = Random.Range(0f, worldPropPrefabs.Count-1);
        SpawnPropPrefab(worldPropPrefabs[(int)randomIndex]);
    }
    
    private void InitializeCorridorRoom()
    {
        var randomIndex = Random.Range(0f, worldPropPrefabs.Count-1);
        SpawnPropPrefab(worldPropPrefabs[(int)randomIndex]);
    }
    
    private void InitializeAltarRoom()
    {
        var randomIndex = Random.Range(0f, worldPropPrefabs.Count-1);
        SpawnPropPrefab(worldPropPrefabs[(int)randomIndex]);
    }
    
    private void InitializeBossRoom()
    {
        var randomIndex = Random.Range(0f, worldPropPrefabs.Count-1);
        SpawnPropPrefab(worldPropPrefabs[(int)randomIndex]);
    }
    
    private void InitializeExitRoom()
    {
        var randomIndex = Random.Range(0f, worldPropPrefabs.Count-1);
        SpawnPropPrefab(worldPropPrefabs[(int)randomIndex]);
    }


    private void SpawnPropPrefab(GameObject propPrefab)
    {
        var spawnedPropset = Instantiate(propPrefab, referencePoint.transform.position, quaternion.identity, worldParent.transform);
    }

    private void InitializeEnemys()
    {
        var patternObject = DetermineEnemyPattern();
        var pattern = patternObject.GetComponent<EnemySpawnPoints>();
        pattern.SetupPoints();
        var numberOfPiedraToSpawn = DetermineNumberToSpawn(dungeonRoom.roomDifficulty, pattern.piedraSpawnPositions.Count);
        var numberOfBushToSpawn = DetermineNumberToSpawn(dungeonRoom.roomDifficulty, pattern.bushSpawnPositions.Count);
        SpawnEnemysOnPositions(numberOfPiedraToSpawn, pattern.piedraSpawnPositions, piedraPrefab);
        SpawnEnemysOnPositions(numberOfBushToSpawn, pattern.bushSpawnPositions, bushPrefab);
    }

    private int DetermineNumberToSpawn(float roomDifficulty,int maxCount)
    {
        float factor = 1f - roomDifficulty;

        int numberOfEnemys = (int)((maxCount - factor * maxCount) * UnityEngine.Random.value + factor * maxCount);
        return numberOfEnemys;
    }
    
    private void SpawnEnemysOnPositions(int numberOfEnemeys, List<Vector3> positions, GameObject enemyToSpawn)
    {
        List<Vector3> selectedSpawnPositions = new List<Vector3>();
        while (selectedSpawnPositions.Count < numberOfEnemeys)
        {
            int randomIndex = Random.Range(0, positions.Count);
            if (!selectedSpawnPositions.Contains(positions[randomIndex]))
            {
                selectedSpawnPositions.Add(positions[randomIndex]);
            }
        }

        foreach (var position in selectedSpawnPositions)
        {
            Instantiate(enemyToSpawn, position, Quaternion.identity, enemyParent.transform);
        }
    }
    
    private GameObject DetermineEnemyPattern()
    {
        var objectToReturn = Instantiate(enemySpawnpointPrefabs[0], referencePoint.transform.position,
            Quaternion.identity, enemyParent.transform);
        return objectToReturn;
    }
    
    
    
}
