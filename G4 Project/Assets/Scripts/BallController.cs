using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameCoordinator coordinator;

    [HideInInspector]
    public int ownerID, thrust;

    [HideInInspector]
    public float xForce, yForce;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coordinator = FindObjectOfType<GameCoordinator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyForce()
    {
        rb.AddForce(new Vector2(xForce, yForce) * thrust, ForceMode2D.Force);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<PaddleController>() != null)
        {
            PaddleController paddle = col.gameObject.GetComponent<PaddleController>();
            ownerID = paddle.getOwnerID();
        }
    }
}
