using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.IO.Ports;
using Valve.VR;


public class DeviceManager : MonoBehaviour
{
    public SteamVR_Action_Pose tracker;
    public GameObject device;
    SerialPort touchwheel = new SerialPort("COM3", 9600);
    public string touchWheelMessage;
    public bool isTouchWheelReady;
    // Start is called before the first frame update
    void Start()
    {
        device.transform.position = tracker.localPosition;

        if (!isTouchWheelReady)
        {
            StartCoroutine(CheckTouchWheelStatus());
        }

        isTouchWheelReady = false;

    }

    // Update is called once per frame
    void Update()
    {
        device.transform.position = tracker.localPosition;
    }

    IEnumerator CheckTouchWheelStatus()
    {
        // update seiral message 
        while (!isTouchWheelReady)
        {
            touchWheelMessage = touchwheel.ReadLine();
            isTouchWheelReady = touchWheelMessage.Equals("ready");

            yield return null;

        }

        Debug.Log("Touchwheel Ready");
        yield break;
    }

  
}
