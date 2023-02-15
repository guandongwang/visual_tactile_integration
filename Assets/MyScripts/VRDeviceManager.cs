using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Valve.VR;

public class VRDeviceManager     : MonoBehaviour
{
    MainSequence mainSequence;
    DataRecorder dataRecorder;

    public List<GameObject> disks;
    public GameObject device;

    /*public SteamVR_Input_Sources tracker; */
   

    // Start is called before the first frame update
    void Start()
    {
        mainSequence = GetComponent<MainSequence>();
        dataRecorder = GetComponent<DataRecorder>();
        device = GameObject.Find("Device");
        

        /*        mainSequence.onTrialStart.AddListener(SwitchDisk());
                mainSequence.onTrialFinish.AddListener(HideAllDisks);*/

        for (int i = 1; i <= 5; i++)
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
        if (Input.GetKeyDown(KeyCode.R))
        {
          /*  device.GetComponent<SteamVR_TrackedObject>().enabled = false;*/
        }

    
    }

    public void SwitchDisk(int diskNo)
    {
        foreach (GameObject disk in disks)
        {
            dataRecorder.currDiskNo = -1;
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
