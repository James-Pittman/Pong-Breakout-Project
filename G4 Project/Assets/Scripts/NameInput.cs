using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// https://www.youtube.com/watch?v=xGmduA-ObyA&t=200s
public class NameInput : MonoBehaviour
{
    public Button btn;
    public InputField player1Field;
    public InputField player2Field;

    // Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(NameInputHandler);
    }

    public void NameInputHandler()
    {
        string p1 = player1Field.text;
        string p2 = player2Field.text;
        if (string.IsNullOrEmpty(p1))
        {
            p1 = "Player 1";
        }
        if (string.IsNullOrEmpty(p2))
        {
            p2 = "Player 2";
        }

        ImportantData.player1Name = p1;
        ImportantData.player2Name = p2;
    }
}
