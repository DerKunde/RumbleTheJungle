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

    [SerializeField] private Rigidbody rb;
    [SerializeField] private NavMeshAgent navAgent;
    private Transform target;
    private Vector3 moveDirection;
    Animator animator;
    private float rangesave;
    public float AttackRange { 
        get
        {
            return animator.GetFloat("RangeToPlayer");
        }
        set
        {
            animator.SetFloat("RangeToPlayer", value);
        }
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rangesave = navAgent.stoppingDistance;
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
            this.transform.rotation = Quaternion.Euler(new Vector3(30, 0, 0));
            _animator.SetFloat("Speed", navAgent.speed);         
        }
        AttackRange = Vector3.Distance(target.transform.position, this.transform.position);
    }

    private void FixedUpdate()
    {
        
    }

    public void CanNotMove()
    {
        navAgent.stoppingDistance = 10000;
    }
    public void CanMove()
    {
        navAgent.stoppingDistance = rangesave;
    }
}
