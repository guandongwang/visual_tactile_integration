using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.IO.Ports;
using Valve.VR;
using System.Threading;


public class PhysicalDeviceManager : MonoBehaviour
{
    private SerialPort serialPort;
    private Thread readThread;
    private bool running = true;

    public string touchWheelMessage;
    public bool isTouchWheelReady;

    public GameObject device;
    public GameObject tracker;

    

    // Start is called before the first frame update
    void Start()
    {
        

        device = GameObject.Find("Device");
        tracker = device.transform.parent.gameObject;


        isTouchWheelReady = false;



        serialPort = new SerialPort("COM3", 9600);
        serialPort.Open();

        readThread = new Thread(new ThreadStart(ReadSerialData));
        readThread.Start();

    }


    void Update()
    {

    }

    void ReadSerialData()
    {
        while (running)
        {
            try
            {
                string data = serialPort.ReadLine();
                isTouchWheelReady = data.Equals("ready");
               /* Debug.Log("Received: " + data);*/
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }

    public void WriteSerialData(string data)
    {
        try
        {
            serialPort.Write(data);
            serialPort.BaseStream.Flush();
            Debug.Log("Sent: " + data);
            isTouchWheelReady = false;
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    void OnApplicationQuit()
    {
        running = false;
        readThread.Abort();
        serialPort.Close();
    }
    
}


    


