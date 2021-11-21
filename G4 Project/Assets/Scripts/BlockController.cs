using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    [SerializeField]
    private int health;

    // Start is called before the first frame update
    void Start()
    {
        UpdateColor();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        health--;
        UpdateColor();

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void UpdateColor()
    {
        GameObject child = gameObject.transform.GetChild(0).gameObject;
        Material mat = child.GetComponent<SpriteRenderer>().material;

        if (health == 1)
        {
            mat.color = Color.red;
        }
        else if (health == 2)
        {
            mat.color = Color.yellow;
        }
        else if (health == 3)
        {
            mat.color = Color.green;
        }
    }
}
