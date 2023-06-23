using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class Playercontrols : MonoBehaviour
{
    Rigidbody rb;
    Animator animator;
    Vector2 moveinput;
    public float Movementspeed = 10;
    public UnityEvent<bool> onDirectionChanged;
    private bool _isFacingRight = true;
    private bool _isMoving = false;
    
    private Vector2 lastmoveinput;
    public float dashspeed = 80;
    public Vector3 CurrentMove { 
        get
        {
            if (CanMove)
                return new Vector3(moveinput.x * Movementspeed, rb.velocity.y, moveinput.y * Movementspeed);
            else
                return new Vector3(lastmoveinput.x * dashspeed, rb.velocity.y, lastmoveinput.y * dashspeed);
        }
    }
    public bool IsFacingRight { get => _isFacingRight;

        private set
        {
            if (_isFacingRight != value)
            {
                //spriteRenderer.flipX = value;
                onDirectionChanged?.Invoke(value);
            }
            _isFacingRight = value;
        } 
    }

    public bool IsMoving { get => _isMoving;
        private set
        {
            _isMoving = value;
            animator.SetBool("IsMoving", value);
        }
    }

    public bool HasCooldown => animator.GetFloat("Cooldown") >= 0;

    public bool CanMove { get => animator.GetBool("CanMove"); private set => animator.SetBool("CanMove", value); }
    public float DashCooldown { get => animator.GetFloat("Cooldown"); private set => animator.SetFloat("Cooldown",value); }
    
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (HasCooldown)
        {
            DashCooldown -= Time.deltaTime;
        }
        // Debug.Log("PlayerPosition: " + this.transform.position);
    }

    private void FixedUpdate()
    {
        rb.velocity = CurrentMove;
    }

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        moveinput = ctx.ReadValue<Vector2>();
        if (CanMove)
        {
            SetFacingDirection(moveinput);
            IsMoving = moveinput != Vector2.zero;
        }

    }

    public void OnDash(InputAction.CallbackContext ctx)
    {
        
        if (ctx.started && !HasCooldown)
        {
            Debug.Log("Dashing");
            Dashing();
        }
    }

    private void Dashing()
    {
        lastmoveinput = moveinput;
        if (lastmoveinput!=Vector2.zero)
        {
            animator.SetTrigger("Dash");
            CanMove = false;
        }
    }

    public void OnLightAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Debug.Log("Light Attack was clicked");
            animator.SetTrigger("Light Attack");
        }

    }

    public void OnHeavyAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Debug.Log("Heavy Attack was clicked");
            animator.SetTrigger("Heavy Attack");
        }
    }

    public void OnRangedAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            Debug.Log("Ranged Attack was clicked");
            animator.SetBool("Ranged Attack", true);
        }

        if (ctx.performed)
        {
            Debug.Log("Ranged Attack was held");
            animator.SetBool("Charged", true);
        }

        if (ctx.canceled)
        {
            Debug.Log("Ranged Attack was released");
            animator.SetBool("Ranged Attack", false);
        }
    }

    public void refreshfacingdirection()
    {
        SetFacingDirection(moveinput);
    }

    private void SetFacingDirection(Vector2 moveinput)
    {
        if (moveinput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (moveinput.x<0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }
}
