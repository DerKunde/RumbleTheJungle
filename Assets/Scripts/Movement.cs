using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Movement : MonoBehaviour
{
    public float movementSpeed = 0.0009f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            //Move Left
            MoveUp();
        }

        if (Input.GetKey(KeyCode.A))
        {
            //Move Right
            MoveLeft();
        }

        if (Input.GetKey(KeyCode.S))
        {
            //Move Up
            MoveDown();
        }

        if (Input.GetKey(KeyCode.D))
        {
            //MoveDown
            MoveRight();
        }
    }

    private void MoveUp()
    {
        this.transform.position += new Vector3(0, 0, movementSpeed);
    }

    private void MoveDown()
    {
        this.transform.position += new Vector3(0, 0, -movementSpeed);
    }

    private void MoveLeft()
    {
        this.transform.position += new Vector3(-movementSpeed, 0, 0);
    }

    private void MoveRight()
    {
        this.transform.position += new Vector3(movementSpeed, 0, 0);
    }
    
}
