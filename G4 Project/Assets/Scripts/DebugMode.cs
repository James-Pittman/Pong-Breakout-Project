using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugMode : MonoBehaviour
{
    [HideInInspector]
    bool showConsole;

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

        GUI.Box(new Rect(10, 10, 100, 90), "Debug Menu");

        // Enable PC Mode
        // Both paddles move at the same time using 'W' and 'S' or arrow keys for up/down
        if (GUI.Button(new Rect(20, 40, 80, 20), "PC Mode"))
        {
            var allPaddles = FindObjectsOfType(typeof(PaddleController));
            foreach (PaddleController paddle in allPaddles)
            {
                paddle.debugToggle = !paddle.debugToggle;
            }
        }

        // Restart the game
        if (GUI.Button(new Rect(20, 70, 80, 20), "Restart"))
        {
            gameObject.GetComponent<GameCoordinator>().RestartGame();
        }
    }
}
