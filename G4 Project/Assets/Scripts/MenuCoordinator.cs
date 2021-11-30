using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SVSBluetooth;
using UnityEngine.SceneManagement;

public class MenuCoordinator : MonoBehaviour
{
    public string UUID;

    private int difficulty = 0;

    private bool waitFlag = false;

    private float waitTime = 5.0f;

    public GameObject title;
    public GameObject leaderboard;
    public GameObject playMenu;
    public GameObject instructions;
    public GameObject credits;
    public GameObject serverScreen;
    public GameObject connectScreen;
    public GameObject warning;

    private void OnEnable()
    {
        BluetoothForAndroid.DeviceConnected += StartGame;
    }

    private void OnDisable()
    {
        BluetoothForAndroid.DeviceConnected -= StartGame;
    }

    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        BluetoothForAndroid.Initialize();
    }

    private void Update()
    {
        if (waitFlag)
        {
            waitTime -= Time.deltaTime;
        }
        if (waitTime <= 0)
        {
            SceneManager.LoadScene("main");
        }
    }

    public void ChangeDifficulty()
    {
        if (difficulty == 0)
            difficulty = 1;
        else
            difficulty = 0;
    }

    private void StartGame()
    {
        ImportantData.powerupFreq = difficulty;
        waitFlag = true;
        waitTime = 5.0f;
    }

    public void MainMenu()
    {
        NetworkCoordinator.instance.Disconnect();
        SceneManager.LoadScene("Title");
    }

    public void TitleScreen()
    {
        title.SetActive(true);
    }

    public void Leaderboard()
    {
        leaderboard.GetComponent<MainMenuLeaderBoard>().GetRecords();
        leaderboard.SetActive(true);
    }

    public void PlayScreen()
    {
        if (!BluetoothForAndroid.IsBTEnabled())
            BluetoothForAndroid.EnableBT();

        playMenu.SetActive(true);
    }

    public void Instructions()
    {
        instructions.SetActive(true);
    }

    public void Credits()
    {
        credits.SetActive(true);
    }

    public void DisableWarning()
    {
        warning.SetActive(false);
    }

    public void StartServer()
    {
        serverScreen.SetActive(true);
        if (BluetoothForAndroid.IsBTEnabled())
        {
            warning.SetActive(false);
            ImportantData.serverFlag = true;
            BluetoothForAndroid.CreateServer(UUID);
        }
        else
        {
            warning.SetActive(true);
        }
    }

    public void ConnectServer()
    {
        connectScreen.SetActive(true);
        if (BluetoothForAndroid.IsBTEnabled())
        {
            warning.SetActive(false);
            ImportantData.serverFlag = false;
            BluetoothForAndroid.ConnectToServer(UUID);
        }
        else
        {
            warning.SetActive(true);
        }
    }
}
