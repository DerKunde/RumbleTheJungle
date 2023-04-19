using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(Avatar))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 10;

    private InputActions inputActions;
    private Avatar _avatar;

    private void Awake()
    {
        inputActions = new InputActions();
        _avatar = GetComponent<Avatar>();
    }

    private void Update()
    {
        Vector3 move = inputActions.Player.Movement.ReadValue<Vector3>().normalized * _movementSpeed * Time.deltaTime;
        Debug.Log(move);
        _avatar.Flip(move.x > 0);
        transform.Translate(move);
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