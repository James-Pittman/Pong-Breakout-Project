using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour
{

    [HideInInspector]
    public bool debugToggle = false;

    private Rigidbody2D paddle;

    [SerializeField]
    private int ownerID;

    public int getOwnerID()
    {
        return ownerID;
    }

    private void Start()
    {
        paddle = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Movement();
    }

    // Touch movement for paddles
    private void Movement()
    {
        // Check to see if debug PC mode is on.
        if (debugToggle)
        {
            PCMovement();
        }
        // Touch movement
        else
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
    }

    // PC movement for debug
    // Moves both paddles
    private void PCMovement()
    {
        float direction = Input.GetAxis("Vertical");
        paddle.position = new Vector2(paddle.position.x, Mathf.Clamp(paddle.position.y + (direction / 20), -3.7f, 3.7f));
    }
}
