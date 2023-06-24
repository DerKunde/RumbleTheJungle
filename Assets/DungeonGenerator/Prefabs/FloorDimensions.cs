using UnityEngine;

public class FloorDimensions : MonoBehaviour
{
    [SerializeField] private Collider collider;
    
    [HideInInspector]
    public float floorWidth;
    [HideInInspector]
    public float floorHeight;

    void Awake()
    {
        CalculateFloorDimensions();
        Debug.Log("Floor Width: " + floorWidth);
        Debug.Log("Floor Height: " + floorHeight);
    }

    void CalculateFloorDimensions()
    {
        if (collider != null)
        {
            Bounds bounds = collider.bounds;
            floorWidth = bounds.size.x * transform.localScale.x;
            floorHeight = bounds.size.z * transform.localScale.z;
            return;
        }

        Debug.LogWarning("No renderer or collider found on the floor GameObject.");
        floorWidth = 0f;
        floorHeight = 0f;
    }
}