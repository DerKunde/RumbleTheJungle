using System;
using System.Collections;
using UnityEngine;

/*
 * Das Portal Skript steuert das Verhalten eines Portals. Dazu gehört das auslösen des OnPortalEnter Events.
 * Außerdem kann hier der Threshold eingestellt werden, welcher angibt wie lange der Spieler sich im Portal befinden
 * muss um den Übergang in den nächsten Raum auszulösen.
 */

[RequireComponent(typeof(BoxCollider))]
public class Portal : MonoBehaviour
{
    [SerializeField] public Transform spawnTransform;

    [SerializeField] private MeshRenderer renderer;

    private DungeonRoom dungeonRoom;

    public RoomManager.Direction portalDirection;

    public delegate void TriggerEventHandler((int x, int y) position, RoomManager.Direction direction);

    public event TriggerEventHandler OnPortalEnter;

    private bool playerInTrigger = false;
    private float elapsedTime = 0f;
    public float delayTime = 2f; // Verzögerungszeit in Sekunden

    private void Start()
    {
        dungeonRoom = this.GetComponentInParent<DungeonRoom>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            renderer.material.color = Color.red;
            playerInTrigger = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= delayTime && playerInTrigger)
            {
                StartCoroutine(DoFadeIn());
                playerInTrigger = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            renderer.material.color = Color.green;
            playerInTrigger = false;
        }
    }

    public void SetPortalState(bool state)
    {
        this.gameObject.SetActive(state);
    }

    private IEnumerator DoFadeIn()
    {
        CameraFade.FadeOut();
        yield return new WaitForSeconds(CameraFade.speedScale * 2);
        OnPortalEnter?.Invoke(dungeonRoom.roomPosition, portalDirection);
        dungeonRoom.DisableRoomContent();
    }
}
