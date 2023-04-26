using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Valve.VR;
using HTC.UnityPlugin.Vive;

public class VRDeviceManager : MonoBehaviour
{

    public List<GameObject> disks;
    public GameObject device;
    public GameObject tracker;

    /*public SteamVR_Input_Sources tracker; */


    // Start is called before the first frame update
    void Start()
    {

        for (int i = 1; i <= 21; i++)
        {
            GameObject obj = GameObject.Find("B" + i);
            if (obj != null)
            {
                disks.Add(obj);
            }
        }


    }

    // Update is called once per frame
    void Update()
    {

    }

    void DisableTrackerMovement()
    { 
        tracker.GetComponent<VivePoseTracker>().enabled = false; 
    }

    void EnableTrackerMovement()
    {
        tracker.GetComponent<VivePoseTracker>().enabled = true;
    }

    public void PresentDisk(string diskName)
    {
            //0.125m is the distance from the middle of the device to the top surface
            GameObject.Find(diskName).transform.localPosition = new Vector3(0f, -0.045f, 0.125f);
   
    }

    public void HideDisk(string diskName)
    {
         GameObject.Find(diskName).transform.localPosition = new Vector3(0f, -0.045f, 0.125f);
    }



}
