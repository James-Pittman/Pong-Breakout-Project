using System;
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

    // Id of this ball relative to the scene.
    public int ballID = -1;

    // Thrust applied to the ball.
    public int thrust;

    public int superCharges = 0;

    // Apply a force to the ball based on a vector representing the velocity.
    public void ApplyForce(Vector2 vec)
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(vec.normalized * thrust, ForceMode2D.Force);
    }

    // Applies a force to the bell based on the current velocity.
    // The purpose of this method is to update the velocity when the
    // thrust is changed.
    public void ApplyForce()
    {
        ApplyForce(rb.velocity);
    }

    // Sets the velocity of the ball based on the speed argument.
    // For example, if the current thrust is 500 and this function is called
    // with a value of 1000, the speed of the ball is doubled.
    public void UpdateThrust(int speed)
    {
        thrust = speed;
        ApplyForce();
    }

    // Nudges the ball velocity so that the x component isn't super close to zero.
    public void NudgeX()
    {
        Vector2 currDir = rb.velocity.normalized; // Current direction
        Vector2 nudge = new Vector2(UnityEngine.Random.Range(0.1f, 0.15f), 0f);
        Vector2 newVelocity = (UnityEngine.Random.value < 0.5) ? currDir + nudge : currDir - nudge;

        ApplyForce(newVelocity);
    }

    // Nudges the ball velocity so that the y component isn't super close to zero.
    public void NudgeY()
    {
        Vector2 currDir = rb.velocity.normalized; // Current direction
        Vector2 nudge = new Vector2(0f, UnityEngine.Random.Range(0.05f, 0.15f));
        Vector2 newVelocity = (UnityEngine.Random.value < 0.5) ? currDir + nudge : currDir - nudge;

        ApplyForce(newVelocity);
    }

    public void AddSuperCharges(int numCharges)
    {
        superCharges += numCharges;
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void RemoveSuperCharge()
    {
        superCharges--;
        if (superCharges <= 0)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void UpdateBall(float xPos, float yPos)
    {
        gameObject.transform.position = new Vector2(xPos, yPos);
    }

    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coordinator = FindObjectOfType<GameCoordinator>();
    }

    private void Update()
    {
        if (GameCoordinator.instance.serverFlag)
            SendUpdate();
    }

    private void SendUpdate()
    {
        if (ballID == -1)
            return;

        byte[] position = new byte[10];
        byte[] xPos = new byte[4];
        byte[] yPos = new byte[4];

        xPos = BitConverter.GetBytes(gameObject.transform.position.x);
        yPos = BitConverter.GetBytes(gameObject.transform.position.y);

        position[0] = (byte)2;
        position[1] = (byte)ballID;
        position[2] = xPos[0];
        position[3] = xPos[1];
        position[4] = xPos[2];
        position[5] = xPos[3];
        position[6] = yPos[0];
        position[7] = yPos[1];
        position[8] = yPos[2];
        position[9] = yPos[3];

        NetworkCoordinator.instance.WriteMessage(position);
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
                NudgeX();
            }
            else if (Mathf.Abs(rb.velocity.normalized.y) < 0.05)
            {
                NudgeY();
            }
        }
    }
}
