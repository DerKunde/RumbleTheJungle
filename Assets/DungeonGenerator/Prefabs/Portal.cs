using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Portal : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer renderer;

    private bool portalActive = false;
    
    // public Room targetRoom;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            renderer.material.color = Color.red;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        renderer.material.color = Color.green;
    }

    public void TogglePortalState()
    {
        portalActive = !portalActive;
    }
}
