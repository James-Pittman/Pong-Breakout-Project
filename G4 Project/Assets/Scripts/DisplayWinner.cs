using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayWinner : MonoBehaviour
{
    public Text winnerField;

    void Awake()
    {
        winnerField.text = ImportantData.winnerName + " wins!";
    }

}
