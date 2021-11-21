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
            mat.color = new Color(240f/255, 60f/255, 60f/255, 1);
        }
        else if (health == 2)
        {
            //mat.color = new Color(230, 130, 0, 1);
            mat.color = new Color(240f/255, 160f/255, 30f/255, 1);
        }
        else if (health == 3)
        {
            mat.color = new Color(230f/255, 230f/255, 0, 1);
        }
        else if (health == 4)
        {
            mat.color = new Color(60f/255, 240f/255, 70f/255, 1);
        }
        else if (health == 5)
        {
            mat.color = new Color(60f/255, 240f/255, 190f/255, 1);
        }
        else if (health == 6)
        {
            mat.color = new Color(110f/255, 170f/255, 240f/255, 1);
        }
        else if (health == 7)
        {
            mat.color = new Color(160f/255, 100f/255, 240f/255, 1);
        }
        else if (health == 8)
        {
            mat.color = new Color(240f/255, 110f/255, 230f/255, 1);
        }
    }
}
