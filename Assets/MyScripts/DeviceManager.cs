using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.IO.Ports;


public class DeviceManager : MonoBehaviour
{

    SerialPort touchwheel = new SerialPort("COM4", 9600);
    public string touchWheelMessage;
    public bool isTouchWheelReady;
    // Start is called before the first frame update
    void Start()
    {
        if (!isTouchWheelReady)
        {
            StartCoroutine(CheckTouchWheelStatus());
        }

        isTouchWheelReady = false;

    }

    // Update is called once per frame
    void Update()
    {
        
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
