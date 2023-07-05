using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Vogel : MonoBehaviour
{
    public Transform agent;
    public float moveSpeed = 3;
    public float jumpForce = 5;
    public float jumpRadius = 7;
    public Animator animator;

    private NavMeshAgent navMeshAgent;
    private bool isFrozen = false; // Flag to track if the enemy is currently frozen
    private float freezeDuration = 2f; // Duration of freeze in seconds
    private float chargeDuration = 2f; // Duration of the charging action in seconds
    private bool canJump = true; // Flag to track if the enemy can perform a jump
    private float jumpTimer = 3f; // Time interval between jumps
    private float timeSinceLastJump = 0f; // Time elapsed since the last jump


    private Coroutine chargingCoroutine; // Reference to the charging coroutine

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = moveSpeed;
    }

    void Update()
    {
        // Calculate the distance between the agent and the enemy
        float distanceToAgent = Vector3.Distance(transform.position, agent.position);

        if (isFrozen)
        {
            animator.SetBool("IsRange", true);
            Jump();
        }
        else
        {
            animator.SetBool("IsRange", false);

            if (distanceToAgent <= jumpRadius && chargingCoroutine == null)
            {
                // Freeze the enemy when in range
                FreezeEnemy();

                // Start the charging coroutine
                chargingCoroutine = StartCoroutine(ChargeAtAgent());
            }
            else if (distanceToAgent > jumpRadius && chargingCoroutine != null)
            {
                // Cancel the charging coroutine if the agent moves out of range
                StopCoroutine(chargingCoroutine);
                chargingCoroutine = null;
            }
        }
    }

    void FreezeEnemy()
    {
        // Stop the enemy from moving
        navMeshAgent.isStopped = true;

        // Set the frozen flag to true
        isFrozen = true;

        // Start the coroutine to unfreeze the enemy after the freeze duration
       Invoke("UnfreezeEnemy",freezeDuration);
    }

    private void UnfreezeEnemy()
    {

        // Enable enemy movement after the freeze duration
        navMeshAgent.isStopped = false;

        // Set the frozen flag to false
        isFrozen = false;
    }

    IEnumerator ChargeAtAgent()
    {
        // Calculate the direction towards the agent
        Vector3 chargeDirection = (agent.position - transform.position).normalized;

        // Set the movement speed to the same value for charging
        navMeshAgent.speed = moveSpeed;

        // Move the enemy towards the agent for the charging duration
        yield return new WaitForSeconds(chargeDuration);

        // Reset the movement speed back to the default value after charging
        navMeshAgent.speed = moveSpeed;

        // Reset the charging coroutine reference
        chargingCoroutine = null;
    }

    void Jump()
    {
        // Freeze the enemy for 2 seconds at the beginning of the jump
        StartCoroutine(FreezeBeforeJump());
    }

    IEnumerator FreezeBeforeJump()
    {
        // Freeze the enemy
        FreezeEnemy();

        // Wait for 2 seconds
        yield return new WaitForSeconds(2f);

        // Unfreeze the enemy after the delay
        UnfreezeEnemy();

        // Start the jump coroutine
        StartCoroutine(JumpInSky());
    }

    IEnumerator JumpInSky()
    {
        // Store the initial position of the enemy
        Vector3 initialPosition = transform.position;

        // Store the previous position of the agent
        Vector3 previousAgentPosition = agent.position;

        // Calculate the jump height (adjust the value as needed)
        float jumpHeight = 20f;

        // Calculate the jump duration (adjust the value as needed)
        float jumpDuration = 1f;

        // Calculate the time elapsed
        float elapsedTime = 0f;

        // Perform the jump animation for the specified duration
        while (elapsedTime < jumpDuration)
        {
            // Calculate the percentage of completion for the jump
            float normalizedTime = elapsedTime / jumpDuration;

            // Calculate the vertical displacement using a quadratic equation
            float verticalDisplacement = Mathf.Sin(normalizedTime * Mathf.PI) * jumpHeight;

            // Calculate the new position by adding the vertical displacement
            Vector3 newPosition = initialPosition + Vector3.up * verticalDisplacement;

            // Move the enemy to the new position
            transform.position = newPosition;

            // Update the elapsed time
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Get the position of the agent from 2 seconds ago
        Vector3 targetPosition = previousAgentPosition;

        // Set the enemy to the target position
        transform.position = targetPosition;

        // Start the charging coroutine
        chargingCoroutine = StartCoroutine(ChargeAtAgent());
    }
}