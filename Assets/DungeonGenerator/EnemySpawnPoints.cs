using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Dieses Skript definiert die Presets um verschiedene Gegner-Muster zu erzeugen.
 */
public class EnemySpawnPoints : MonoBehaviour
{

    [SerializeField]
    private List<Transform> piedraSpawnPoints;
    [SerializeField]
    private List<Transform> bushSpawnPoints;

    public List<Vector3> piedraSpawnPositions;
    // public List<Vector3> bushSpawnPositions;

    // Start is called before the first frame update
    public void SetupPoints()
    {
        piedraSpawnPositions = TransformToVector3(piedraSpawnPoints);
        // bushSpawnPositions = TransformToVector3(bushSpawnPoints);

    }

    private List<Vector3> TransformToVector3(List<Transform> transformList)
    {
        List<Vector3> vector3List = new List<Vector3>(); 

        foreach (var transform in transformList)
        {
            vector3List.Add(transform.position);
        }

        return vector3List;
    }
    
}
