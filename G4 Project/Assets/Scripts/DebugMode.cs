using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugMode : MonoBehaviour
{
    private ScoreKeeper[] scoreKeepers;
    private GameCoordinator coordinator;

    [HideInInspector]
    bool showConsole;

    private void Start()
    {
        scoreKeepers = FindObjectsOfType<ScoreKeeper>();
        coordinator = FindObjectOfType<GameCoordinator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            showConsole = !showConsole;
        }
    }

    private void OnGUI()
    {
        if (!showConsole)
            return;

        GUI.Box(new Rect(10, 10, 120, 150), "Debug Menu");

        // Enable PC Mode
        // Both paddles move at the same time using 'W' and 'S' or arrow keys for up/down
        if (GUI.Button(new Rect(20, 40, 100, 20), "PC Mode"))
        {
            var allPaddles = FindObjectsOfType(typeof(PaddleController));
            foreach (PaddleController paddle in allPaddles)
            {
                paddle.debugToggle = !paddle.debugToggle;
            }
        }

        // Restart the game
        if (GUI.Button(new Rect(20, 70, 100, 20), "Restart"))
        {
            coordinator.RestartGame();
        }

        // Spawn a new ball in center of screen
        if (GUI.Button(new Rect(20, 100, 100, 20), "Spawn Ball"))
        {
            coordinator.GenerateBall(2);
        }

        // Enable Infinite lives
        if (GUI.Button(new Rect(20, 130, 100, 20), "Infinite Lives"))
        {
            foreach (ScoreKeeper sk in scoreKeepers)
                sk.debugToggle = !sk.debugToggle;
        }
    }
}
