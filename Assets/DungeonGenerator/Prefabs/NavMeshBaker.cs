using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour {

    public NavMeshSurface[] surfaces;
    public Transform[] objectsToRotate;

    // Use this for initialization
    void Start () 
    {

        for (int j = 0; j < objectsToRotate.Length; j++) 
        {
            objectsToRotate [j].localRotation = Quaternion.Euler (new Vector3 (0, Random.Range (0, 360), 0));
        }

        for (int i = 0; i < surfaces.Length; i++) 
        {
            surfaces [i].BuildNavMesh ();    
        }    
    }

}