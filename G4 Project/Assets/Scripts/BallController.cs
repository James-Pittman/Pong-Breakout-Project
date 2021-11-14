using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{

    [SerializeField]
    private int thrust;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        float randomDirection = Random.Range(0, 2) * 2 - 1;
        rb.AddForce(new Vector2(3 * randomDirection, 15 * randomDirection) * thrust, ForceMode2D.Force);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetBall()
    {
        transform.position = Vector2.zero;
        rb.velocity = Vector2.zero;
        float randomDirection = Random.Range(0, 2) * 2 - 1;
        rb.AddForce(new Vector2(3 * randomDirection, 15 * randomDirection) * thrust, ForceMode2D.Force);
    }
}
