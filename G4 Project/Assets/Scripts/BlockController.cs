using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    private int health;

    private int id;

    [HideInInspector]
    public bool powerFlag = false;

    private GameCoordinator coordinator;

    public void ActivateBlock(int blockNum)
    {
        if (blockNum != -1)
            id = blockNum;

        gameObject.SetActive(true);

        if (!coordinator.serverFlag)
            return;

        powerFlag = (UnityEngine.Random.value < coordinator.powerUpRandomVal) ? true : false;
        health = UnityEngine.Random.Range(1, 5);
        UpdateStarVisibility();
        UpdateColor();

        NetworkCoordinator.instance.WriteMessage(new byte[4] { (byte)1, (byte)id, Convert.ToByte(powerFlag), (byte)health });
    }

    // Updates the block based on server data received.
    public void UpdateBlock(bool flag, int healthUpdate, int ballID)
    {
        powerFlag = flag;
        health = healthUpdate;
        UpdateStarVisibility();
        UpdateColor();

        if (health <= 0)
        {
            if (powerFlag)
            {
                coordinator.SelectPowerUp(GameCoordinator.instance.activeBalls[ballID]);


                ScoreKeeper keeper = GameCoordinator.instance.GetScoreKeeper(GameCoordinator.instance.activeBalls[ballID].GetComponent<BallController>().ownerID);
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

    // Start is called before the first frame update
    private void Awake()
    {
        coordinator = FindObjectOfType<GameCoordinator>();
        gameObject.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        BallController ball = col.gameObject.GetComponent<BallController>();

        // Count the number of block levels destroyed and deduct health from the block.
        int blocksDestroyed = 1;
        if(ball.superCharges >= 1)
        {
            blocksDestroyed = health;
            health = 0;
            ball.RemoveSuperCharge();
        }
        else
        {
            health--;
        }

        // Find the owner whose ball hit the block and add points to their score.
        int scorer = ball.ownerID;
        
        ScoreKeeper keeper = null;
        if (scorer == 0 || scorer == 1)
        {
            keeper = coordinator.GetScoreKeeper(scorer);

            // Add points for hitting a block.
            keeper.AddBlockPoints(blocksDestroyed);
        }
        UpdateColor();

        NetworkCoordinator.instance.WriteMessage(new byte[5] { (byte)1, (byte)id, Convert.ToByte(powerFlag), (byte)health, (byte)ball.ballID });

        if (health <= 0)
        {
            if (powerFlag)
            {
                coordinator.SelectPowerUp(col.gameObject);
                
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

    private void UpdateStarVisibility()
    {
        if (powerFlag == true)
        {
            GameObject star = gameObject.transform.GetChild(2).gameObject;
            star.GetComponent<SpriteRenderer>().sortingOrder = 2;
        }
    }
}
