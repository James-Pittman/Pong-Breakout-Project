using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    // Caches for gameObjects/Components
    private Rigidbody2D paddle;

    // Bool to track if the paddle has moved since last update.
    private bool movement;

    // Debug toggle to control movement options.
    [HideInInspector]
    public bool debugToggle = false;

    // The ID of this player/paddle.
    public int ownerID;

    // Returns the ownerID of this paddle.
    public int GetOwnerID()
    {
        return ownerID;
    }

    public void UpdatePosition(float yPos)
    {
        paddle.position = new Vector2(gameObject.transform.position.x, yPos);
    }

    // Initialize caches.
    private void Start()
    {
        paddle = GetComponent<Rigidbody2D>();
    }

    // Update the movement every frame.
    private void Update()
    {
        Movement();
    }

    // Determine the type of movement the paddles will be using
    private void Movement()
    {
        // Check to see if debug PC mode is on, if so use PC Movement
        if (debugToggle)
        {
            PCMovement();
            return;
        }

        // Use Touch Movement
        TouchMovement();
    }

    // PC movement for debug
    private void PCMovement()
    {
        float direction = Input.GetAxis("Vertical");
        paddle.position = new Vector2(paddle.position.x, Mathf.Clamp(paddle.position.y + (direction / 20), -3.7f, 3.7f));
    }

    // Touch movement for Android
    private void TouchMovement()
    {
        // Make sure you can't move the other players paddle.
        //if (ownerID != Convert.ToInt32(!GameCoordinator.instance.serverFlag))
        //    return;

        //movement = true;
        foreach (Touch touch in Input.touches)
        {
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            Vector2 myPosition = paddle.position;
            if (Mathf.Abs(touchPos.x - myPosition.x) <= 2)
            {
                myPosition.y = Mathf.Lerp(myPosition.y, touchPos.y, 12f);
                myPosition.y = Mathf.Clamp(myPosition.y, -3.7f, 3.7f);
                paddle.position = myPosition;
            }
        }

        SendUpdate(ownerID);
    }

    private void SendUpdate(int owner)
    {
        byte[] update = new byte[6];
        byte[] yPos = new byte[4];
        yPos = BitConverter.GetBytes(gameObject.transform.position.y);

        update[0] = (byte)3;

        if (owner == 0)
            update[1] = (byte)1;
        else
            update[1] = (byte)0;

        update[2] = yPos[0];
        update[3] = yPos[1];
        update[4] = yPos[2];
        update[5] = yPos[3];

        NetworkCoordinator.instance.WriteMessage(update);
    }
}
