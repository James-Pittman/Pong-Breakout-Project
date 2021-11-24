using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    // Caches for gameObjects/Components
    private Rigidbody2D rb;
    private GameCoordinator coordinator;

    // Id of the current balls owner (player).
    public int ownerID;

    // Thrust applied to the ball.
    public int thrust;

    // Vector forces of ball.
    public float xForce, yForce;

    // Apply a force to the ball based on the balls xForce and yForce.
    public void ApplyForce()
    {
        rb.AddForce(new Vector2(xForce, yForce) * thrust, ForceMode2D.Force);
    }

    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coordinator = FindObjectOfType<GameCoordinator>();
    }

    // If the ball hit's a paddle, update the balls ownerID.
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<PaddleController>() != null)
        {
            PaddleController paddle = col.gameObject.GetComponent<PaddleController>();
            ownerID = paddle.getOwnerID();
        }
    }
}
