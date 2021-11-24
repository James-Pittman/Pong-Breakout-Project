using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour
{

    [HideInInspector]
    public bool debugToggle = false;

    [SerializeField]
    private int ownerID;

    private Rigidbody2D paddle;

    private void Start()
    {
        paddle = GetComponent<Rigidbody2D>();
    }

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
    }

    public int GetOwnerID()
    {
        return ownerID;
    }
}
