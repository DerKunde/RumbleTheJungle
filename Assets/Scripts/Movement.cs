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

<<<<<<< HEAD
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            MoveUp();
        }

        if (Input.GetKey(KeyCode.A))
        {
            MoveLeft();
            this.transform.rotation = Quaternion.Euler(new Vector3(30f, 0f, 0f));

        }

        if (Input.GetKey(KeyCode.S))
        {
            MoveDown();
        }

        if (Input.GetKey(KeyCode.D))
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(-30f, 180f, 0f));
            MoveRight();
        }
    }
=======
    private InputActions inputActions;
    private Avatar _avatar;
>>>>>>> origin/AlternativeMovement

    private void Awake()
    {
        inputActions = new InputActions();
        _avatar = GetComponent<Avatar>();
    }

    private void Update()
    {
        Vector3 move = inputActions.Player.Movement.ReadValue<Vector3>().normalized * _movementSpeed * Time.deltaTime;
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
