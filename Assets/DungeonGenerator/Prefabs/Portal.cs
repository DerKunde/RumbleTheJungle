using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Portal : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer renderer;
    
    
    // public Room targetRoom;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            renderer.material.color = Color.red;
            CameraFade.FadeOut();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            renderer.material.color = Color.green;
            CameraFade.FadeIn();
        }
    }

    public void SetPortalState(bool state)
    {
        this.gameObject.SetActive(state);
    }
}
