using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SVSBluetooth;

public class MenuCoordinator : MonoBehaviour
{
    public string UUID;

    string role;

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
        //BluetoothForAndroid.BtAdapterEnabled += HideTurnOnBluetoothText;
    }

    private void OnDisable()
    {

    }

    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        BluetoothForAndroid.Initialize();
    }

    public void TitleScreen()
    {
        title.SetActive(true);
    }

    public void Leaderboard()
    {
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
        role = "server";

        serverScreen.SetActive(true);
        if (BluetoothForAndroid.IsBTEnabled())
        {
            warning.SetActive(false);
            BluetoothForAndroid.CreateServer(UUID);
        }
        else
        {
            warning.SetActive(true);
        }
    }

    public void ConnectServer()
    {
        role = "client";

        connectScreen.SetActive(true);
        if (BluetoothForAndroid.IsBTEnabled())
        {
            warning.SetActive(false);
            BluetoothForAndroid.ConnectToServer(UUID);
        }
        else
        {
            warning.SetActive(true);
        }
    }
}
