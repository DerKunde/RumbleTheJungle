using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    public static event Action<Enemy> OnEnemyKilled;
    [SerializeField] private int health, maxHealth = 2;
    private Animator _animator;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private NavMeshAgent navAgent;
    private Transform target;
    private Vector3 moveDirection;

    private void Awake()
    {

    }


    private void Start()
    {
        health = maxHealth;
        target = GameObject.FindWithTag("Player").transform;
        _animator = GetComponent<Animator>();
        _animator.applyRootMotion = false;
    }

    private void Update()
    {
        if (target)
        {
            navAgent.destination = target.position;
            this.transform.rotation  = Quaternion.Euler(new Vector3(30,0,0));
            _animator.SetFloat("Speed", navAgent.speed);
        }
    }

    private void FixedUpdate()
    {
        
    }

    public void TakeDamage(int damageAmount)
    {
        // Debug.Log($"Damage Amount: {damageAmount}");
        health -= damageAmount;
        // Debug.Log($"{transform.name} Health: {health}");

        if (health <= 0)
        {
            OnEnemyKilled?.Invoke(this);
        }
    }
    
}
