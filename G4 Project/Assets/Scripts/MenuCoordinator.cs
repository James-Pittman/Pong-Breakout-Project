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


    private void OnEnable()
    {
        //BluetoothForAndroid.BtAdapterEnabled += HideTurnOnBluetoothText;
    }

    private void OnDisable()
    {

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

    public void StartServer()
    {
        role = "server";

        if (BluetoothForAndroid.IsBTEnabled())
        {
            serverScreen.SetActive(true);
            BluetoothForAndroid.CreateServer(UUID);
        }
        else
        {
            return;
        }
    }

    public void ConnectServer()
    {
        role = "client";

        if (BluetoothForAndroid.IsBTEnabled())
        {
            connectScreen.SetActive(true);
            BluetoothForAndroid.ConnectToServer(UUID);
        }
        else
        {
            return;
        }
    }
}
