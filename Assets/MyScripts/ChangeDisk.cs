using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChangeDisk : MonoBehaviour
{
    MainSequence mainSequence;
    DataRecorder dataRecorder;

    public List<GameObject> disks;

    // Start is called before the first frame update
    void Start()
    {
        mainSequence = GetComponent<MainSequence>();
        dataRecorder = GetComponent<DataRecorder>();

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

    }

    public void SwitchDisk(int diskNo)
    {
        foreach (GameObject disk in disks)
        {
            dataRecorder.currDiskNo = -1;
            disk.transform.position = new Vector3(0f, 0f, 0f);
        }

        if (diskNo >= 0)
        { 
            Debug.Log("Switch Disk: " + diskNo);
            /*HideAllDisks();*/
            disks[diskNo].transform.position = new Vector3(0f, 0.05f, 0f);
        }
    }

  /*  public void HideAllDisks()
    {
       
    }*/
}
