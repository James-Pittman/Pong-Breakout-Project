using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneShift : MonoBehaviour
{
    public void GoToGame()
    {
        SceneManager.LoadScene("main");
    }

    public void SetDifficulty()
    {
        if (ImportantData.powerupFreq == 0)
            ImportantData.powerupFreq = 1;
        else
            ImportantData.powerupFreq = 0;

    }

    public void SetPlayer(string name, int id)
    {
        
    }

    public void GoToTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void GoToUselessLoading()
    {
        StartCoroutine(UselessLoading());
        
    }

    IEnumerator UselessLoading()
    {
        yield return new WaitForSeconds(1);
        GoToGame();
    }
}

