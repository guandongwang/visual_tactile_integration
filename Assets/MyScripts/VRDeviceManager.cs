using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Valve.VR;
using HTC.UnityPlugin.Vive;
using Random = UnityEngine.Random;


public class VrDeviceManager : MonoBehaviour
{
    InfoInspector infoInspector;

 
    GameObject device;
    GameObject tracker;

    bool currTrackerStatus;

    /*public SteamVR_Input_Sources tracker; */


    // Start is called before the first frame update
    void Start()
    {
        infoInspector = GameObject.Find("Scripts").GetComponent<InfoInspector>();
        currTrackerStatus = true;
    /*    tracker = GameObject.Find("Tracker1");
        device = GameObject.Find("device");*/
    }

   

    // Update is called once per frame
    void Update()
    {
/*        device.transform.position = tracker.transform.position;
        device.transform.rotation = tracker.transform.rotation;*/

        if (infoInspector.IsTrackerEnabled != currTrackerStatus)
        {
            currTrackerStatus = infoInspector.IsTrackerEnabled;
            GameObject.Find("Tracker1").GetComponent<VivePoseTracker>().enabled = currTrackerStatus;
           
        }
   
    



    }



    public void PresentDisk(string diskName, int angle)
    {

        GameObject.Find(diskName).GetComponent<Renderer>().enabled = true;
    }

    public void HideDisk(string diskName)
    {
        GameObject.Find(diskName).GetComponent<Renderer>().enabled = false;
   
    }



}
