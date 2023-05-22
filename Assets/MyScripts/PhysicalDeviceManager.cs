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
    private bool running;

    GameObject device;
    GameObject tracker;
    GameObject scripts;

    InfoInspector infoInspector;
    

    // Start is called before the first frame update
    void Start()
    {

        scripts = GameObject.Find("Scripts");
        infoInspector = scripts.GetComponent<InfoInspector>();
        StartSerialComm();
    }
    void OnEnable()
    {
        
      /*  EventManager.StartListening("OnBlockStart", StartSerialComm);
        EventManager.StartListening("OnBlockFinished", StopSerialComm);*/
    }

    void OnDisable()
    {
        StopSerialComm();
        /*EventManager.StartListening("OnBlockStart", StartSerialComm);
        EventManager.StopListening("OnBlockFinished", StopSerialComm);*/
    }


    void Update()
    {

    }

    void StartSerialComm()
    {
   
            running = true;
            serialPort = new SerialPort("COM3", 9600);
            serialPort.Open();

            readThread = new Thread(new ThreadStart(ReadSerialData));
            readThread.Start();
    }

    void StopSerialComm()
    {
        if (serialPort != null)
        {
            running = false;
            readThread.Abort();
            serialPort.Close();
        }
    }

    void ReadSerialData()
    {
        while (running)
        {
            try
            {
                infoInspector.TouchWheelMessage = serialPort.ReadLine();
                infoInspector.IsTouchWheelReady = infoInspector.TouchWheelMessage.Equals("ready");
                /*Debug.Log("Received: " + infoInspector.TouchWheelMessage);*/
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }

    public void WriteSerialData(string data)
    {
        if (data != "dummy")
        {
            try
            {
                serialPort.Write(data);
                serialPort.BaseStream.Flush();
                /*Debug.Log("Sent: " + data);*/
                infoInspector.IsTouchWheelReady = false;
                Debug.Log(data);
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }

    void OnApplicationQuit()
    {
        StopSerialComm();
/*        running = false;
        readThread.Abort();
        serialPort.Close();*/
    }
    
}


    


