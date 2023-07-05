using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class Damageable : MonoBehaviour
{

    public int maxLife = 1;
    Animator animator;

    private int life;
    public int Life {
        get { return life; } 
        set 
        {
            life = value;
            if(life<=0)
            {
                IsAlive = false;
            }
        } 
    }

    public void Hit(int damage)
    {
        Life -= damage;
        animator.SetTrigger("GettingHit");
    }

    private bool isAlive;
    public bool IsAlive { get =>isAlive;
        set
        {
            isAlive = value;
            animator.SetBool("IsAlive",value);
        }
    }


    private void Awake()
    {
        Life = maxLife;
        animator = GetComponent<Animator>();
    }

    public void OnAfterDeath() => Destroy(gameObject);
}
