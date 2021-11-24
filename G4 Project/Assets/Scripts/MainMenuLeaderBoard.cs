using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuLeaderBoard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<Record> recordList = new List<Record>(); 
        Leaderboard board = new Leaderboard(recordList);
    }

}
