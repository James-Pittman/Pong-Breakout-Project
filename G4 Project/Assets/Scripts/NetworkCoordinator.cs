using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SVSBluetooth;

public class NetworkCoordinator : MonoBehaviour
{
    public static NetworkCoordinator instance;

    private void OnEnable()
    {
        BluetoothForAndroid.ReceivedByteMessage += GetMessage;
    }

    private void OnDisable()
    {
        BluetoothForAndroid.ReceivedByteMessage -= GetMessage;
    }

    private void Start()
    {
        if (instance == null)
            instance = this;
    }

    void GetMessage(byte[] message)
    {
        switch ((int)message[0])
        {
            // Start of game data.
            case 0:
                GameCoordinator.instance.UpdateStartData(message);
                break;
            default:
                break;
        }
    }

    public void WriteMessage(byte[] message)
    {
        BluetoothForAndroid.WriteMessage(message);
    }
}
