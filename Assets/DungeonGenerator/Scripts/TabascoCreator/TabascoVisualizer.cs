using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Das Skript bietet die Möglichkeit die generierten Layouts aus Tabasco anzuzeigen. Es ist sehr hilfreich um Tabasco
 * zu debuggen und zu prüfen ob das gewünschte Verhalten erzeugt wird.
 */
public class TabascoVisualizer : MonoBehaviour
{
    private const int START_ROOM = 1;
    private const int EXIT_ROOM = 2;
    private const int CORRIDOR_ROOM = 3;
    private const int ALTAR_ROOM = 4;
    private const int BOSS_ROOM = 5;
    
    private int[,] gridToDisplay;
    private GameObject[,] _gameObjects;

    [SerializeField] private Transform gridParent;
    [SerializeField] private Tabasco _tabasco;
    
    [SerializeField] private GameObject emptyCellPrefab;
    [SerializeField] private GameObject startRoomPrefab;
    [SerializeField] private GameObject exitRoomPrefab;
    [SerializeField] private GameObject corridorRoomPrefab;
    [SerializeField] private GameObject altarRoomPrefab;
    [SerializeField] private GameObject bossRoomPrefab;
    
    // Start is called before the first frame update
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            DisplayGrid();
        }
    }

    public void DisplayGrid()
    {
        if (_gameObjects != null)
        {
            ClearGridParent();
        }
        gridToDisplay = _tabasco.CreateNewDungeon();
        TranslateGridToGameObject(gridToDisplay);
    }
    
    private void TranslateGridToGameObject(int[,] intArray)
    {
        int width = intArray.GetLength(0);
        int height = intArray.GetLength(1);
        _gameObjects = new GameObject[width, height];

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
                        _gameObjects[x, y] = emptyCell;
                        break;

                    case START_ROOM:
                        var startRoom = Instantiate(startRoomPrefab, gridParent);
                        startRoom.transform.position = new Vector3(x, y, 0);
                        _gameObjects[x, y] = startRoom;
                        break;
                    case EXIT_ROOM:
                        var exitRoom = Instantiate(exitRoomPrefab, gridParent);
                        exitRoom.transform.position = new Vector3(x, y, 0);
                        _gameObjects[x, y] = exitRoom;
                        break;
                    case CORRIDOR_ROOM:
                        var corridor = Instantiate(corridorRoomPrefab, gridParent);
                        corridor.transform.position = new Vector3(x, y, 0);
                        _gameObjects[x, y] = corridor;
                        break;
                    case ALTAR_ROOM:
                        var altarRoom = Instantiate(altarRoomPrefab, gridParent);
                        altarRoom.transform.position = new Vector3(x, y, 0);
                        _gameObjects[x, y] = altarRoom;
                        break;
                    case BOSS_ROOM:
                        var bossRoom = Instantiate(bossRoomPrefab, gridParent);
                        bossRoom.transform.position = new Vector3(x, y, 0);
                        _gameObjects[x, y] = bossRoom;
                        break;
                }
            }
        }
    }

    private void ClearGridParent()
    {
        for (int i = 0; i < transform.childCount; i++) 
        {
            // Zerstöre das i-te Child-Transform
            Destroy(transform.GetChild(i).gameObject);
        }

        _gameObjects = null;
    }
}
