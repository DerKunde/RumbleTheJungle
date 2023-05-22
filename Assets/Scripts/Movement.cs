using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Avatar))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 10;
    [SerializeField] private bool useDashDistance;
    [Range(0, 10)]
    [SerializeField] private float dashDistance = 3f;
    

    private InputActions inputActions;
    private Avatar _avatar;
    private bool canDash = true;
    private bool isDashing = false;
    private float dashtime = 0.2f;
    private float dashCooldown = 1f;
    private float dashspeed = 1f;

    private void Awake()
    {
        inputActions = new InputActions();
        _avatar = GetComponent<Avatar>();
    }

    private void Update()
    {
        if (isDashing)
        {
            return;
        }

        Moving();

        if (inputActions.Player.Dash.triggered && canDash)
        {
            StartCoroutine(Dash(inputActions.Player.Movement.ReadValue<Vector3>().normalized));
        }
    }

    private IEnumerator Dash(Vector3 direction)
    {
        canDash = false;
        isDashing = true;
        _avatar.FlipY(true);
        dashspeed = 10f;
        var dash_co = StartCoroutine(Dashing(direction));
        var pos = transform.position;

        if(useDashDistance)
            yield return new WaitUntil(()=>Vector3.Distance(transform.position, pos) >= dashDistance);
        else
            yield return new WaitForSeconds(dashtime);
        _avatar.FlipY(false);
        isDashing = false;
        dashspeed = 1f;
        StopCoroutine(dash_co);
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private IEnumerator Dashing(Vector3 direction)
    {
        while(true)
        {
            MoveTo(direction);
            yield return new WaitForEndOfFrame();
        }
    }
    private void MoveTo(Vector3 direction)
    {
        Vector3 move = direction * _movementSpeed * Time.deltaTime * dashspeed;
        _avatar.Flip(move.x > 0);
        transform.Translate(move);
    }
    private void Moving()
    {
        Vector3 move = inputActions.Player.Movement.ReadValue<Vector3>().normalized;
        MoveTo(move);
    }

    public void OnMoving(InputAction.CallbackContext ctx)
    {
        Debug.Log($"Value: {ctx.ReadValue<Vector3>()}");
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}
