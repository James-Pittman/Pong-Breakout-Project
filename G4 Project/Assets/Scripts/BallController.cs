using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    // Caches for gameObjects/Components
    private Rigidbody2D rb;
    private GameCoordinator coordinator;

    // Id of the current balls owner (player).
    public int ownerID, originID;

    // Thrust applied to the ball.
    public int thrust;

    // Apply a force to the ball based on the balls xForce and yForce.
    public void ApplyForce(Vector2 vec)
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(vec.normalized * thrust, ForceMode2D.Force);
    }

    public void ApplyForce()
    {
        ApplyForce(rb.velocity);
    }

    public void UpdateThrust(int speed)
    {
        thrust = speed;
        ApplyForce();
    }

    public void NudgeX()
    {
        Vector2 currentDirection = rb.velocity.normalized;
        Vector2 nudge = new Vector2(Random.Range(0f, 0.1f), 0f);
        Vector2 newVelocity;
        if (currentDirection.x > 0)
        {
            newVelocity = currentDirection + nudge;
        }
        else
        {
            newVelocity = currentDirection - nudge;
        }

        ApplyForce(newVelocity);
    }

    public void NudgeY()
    {
        Vector2 currentDirection = rb.velocity.normalized;
        Vector2 nudge = new Vector2(0f, Random.Range(0f, 0.1f));
        Vector2 newVelocity;
        if (currentDirection.y > 0)
        {
            newVelocity = currentDirection + nudge;
        }
        else
        {
            newVelocity = currentDirection - nudge;
        }

        ApplyForce(newVelocity);
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
            ownerID = paddle.GetOwnerID();
        }

        
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (rb.velocity.normalized.x != 0 || rb.velocity.normalized.y != 0)
        {
            if (Mathf.Abs(rb.velocity.normalized.x) < 0.1)
            {
                Debug.Log("Nudging X because (x, y) = " + rb.velocity.normalized.x + ", " + rb.velocity.normalized.y);
                NudgeX();
            }
            else if (Mathf.Abs(rb.velocity.normalized.y) < 0.05)
            {
                Debug.Log("Nudging Y because (x, y) = " + rb.velocity.normalized.x + ", " + rb.velocity.normalized.y);
                NudgeY();
            }
        }
    }
}
