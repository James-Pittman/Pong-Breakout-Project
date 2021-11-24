using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    private int health;

    [HideInInspector]
    public bool powerFlag = false;

    private GameCoordinator coordinator;

    public void ActivateBlock()
    {
        powerFlag = (Random.value < coordinator.powerUpRandomVal) ? true : false;

        health = Random.Range(1, 8);

        gameObject.SetActive(true);

        UpdateColor();
    }

    // Start is called before the first frame update
    private void Awake()
    {
        coordinator = FindObjectOfType<GameCoordinator>();
        gameObject.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        health--;

        // Find the owner whose ball hit the block and add points to their score.
        int scorer = col.gameObject.GetComponent<BallController>().ownerID;
        
        ScoreKeeper keeper= null;
        if (scorer == 0 || scorer == 1)
        {
            keeper = coordinator.scoreKeepers[scorer];

            // Add points for hitting a block.
            keeper.AddBlockPoints();
        }

        UpdateColor();

        if (health <= 0)
        {
            if (powerFlag)
            {
                selectPower(col.gameObject);
                
                // Add points for getting a power-up.
                if (keeper != null)
                {
                    keeper.AddPowerUpPoints();
                }
            }

            coordinator.activeBlocks.Remove(gameObject);
            coordinator.inactiveBlocks.Add(gameObject);
            gameObject.SetActive(false);
        }
    }

    // Updates the color of the block to reflect the blocks health.
    private void UpdateColor()
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

    private void selectPower(GameObject ball)
    {
        coordinator.GenerateBall(ball.GetComponent<BallController>().ownerID);
    }
}
