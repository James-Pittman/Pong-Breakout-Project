using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SVSBluetooth;

public class NetworkCoordinator : MonoBehaviour
{
    private void OnEnable()
    {
        //BluetoothForAndroid.ReceivedByteMessage += GetMessage;
    }

    private void OnDisable()
    {
        //BluetoothForAndroid.ReceivedByteMessage -= GetMessage;
    }
}
