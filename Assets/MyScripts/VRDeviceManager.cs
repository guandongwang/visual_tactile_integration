using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Valve.VR;
using HTC.UnityPlugin.Vive;

public class VRDeviceManager : MonoBehaviour
{
    MainSequence mainSequence;
    DataRecorder dataRecorder;

    public List<GameObject> disks;
    public GameObject device;
    public GameObject tracker;

    /*public SteamVR_Input_Sources tracker; */


    // Start is called before the first frame update
    void Start()
    {
        mainSequence = GetComponent<MainSequence>();
        dataRecorder = GetComponent<DataRecorder>();

        mainSequence.onBlockStart.AddListener(DisableTrackerMovement);
        mainSequence.onBlockFinish.AddListener(EnableTrackerMovement);

        device = GameObject.Find("Device");
        tracker = device.transform.parent.gameObject;

        /*        mainSequence.onTrialStart.AddListener(SwitchDisk());
                mainSequence.onTrialFinish.AddListener(HideAllDisks);*/

        for (int i = 9; i <= 13; i++)
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

    public void SwitchDisk(int diskNo)
    {
        foreach (GameObject disk in disks)
        {
            /*dataRecorder.currDiskNo = -1;*/
            //-0.045m offset from the centre of the device
            disk.transform.localPosition = new Vector3(0f, -0.045f, 0f);
        }

        if (diskNo >= 0)
        { 
            Debug.Log("Switch Disk: " + diskNo);
            //0.125m is the distance from the middle of the device to the top surface
            disks[diskNo].transform.localPosition = new Vector3(0f, -0.045f, 0.125f);
        }
    }

}
