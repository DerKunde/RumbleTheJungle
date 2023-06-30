using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    public static event Action<Enemy> OnEnemyKilled;
    private Animator _animator;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private NavMeshAgent navAgent;
    private Transform target;
    private Vector3 moveDirection;

    private void Awake()
    {

    }
    
    private void Start()
    {
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

    
}
