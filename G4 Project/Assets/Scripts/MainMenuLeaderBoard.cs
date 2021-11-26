using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuLeaderBoard : MonoBehaviour
{
    List<Record> board;
    Leaderboard leaderboard;
    [SerializeField] private Text one;
    [SerializeField] private Text two;
    [SerializeField] private Text three;
    [SerializeField] private Text four;
    [SerializeField] private Text five;
    [SerializeField] private Text six;
    [SerializeField] private Text seven;
    [SerializeField] private Text eight;
    [SerializeField] private Text nine;
    [SerializeField] private Text ten;

    public void GetRecords()
    {
        leaderboard = Leaderboard.LoadRecords();
        board = leaderboard.GetTopRecords();
        switch (board.Count)
        {
            case 10:
                ten.text = "10. " + board[9].name + ": " + board[9].score;
                goto case 9;
            case 9:
                nine.text = "9. " + board[8].name + ": " + board[8].score;
                goto case 8;
            case 8:
                eight.text = "8. " + board[7].name + ": " + board[7].score;
                goto case 7;
            case 7:
                seven.text = "7. " + board[6].name + ": " + board[6].score;
                goto case 6;
            case 6:
                six.text = "6. " + board[5].name + ": " + board[5].score;
                goto case 5;
            case 5:
                five.text = "5. " + board[4].name + ": " + board[4].score;
                goto case 4;
            case 4:
                four.text = "4. " + board[3].name + ": " + board[3].score;
                goto case 3;
            case 3:
                three.text = "3. " + board[2].name + ": " + board[2].score;
                goto case 2;
            case 2:
                two.text = "2. " + board[1].name + ": " + board[1].score;
                goto case 1;
            case 1:
                one.text = "1. " + board[0].name + ": " + board[0].score;
                break;
        }
    }
}
