using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainSequence : MonoBehaviour
{

    public UnityEvent onBlockStart;
    public UnityEvent onBlockFinish;

    public UnityEvent onTrialStart;
    public UnityEvent onTrialFinish;

    DataRecorder dataRecorder;
    StimulusCreation stimulusCreation;
    VRDeviceManager vrDeviceManager;
    PhysicalDeviceManager physicalDeviceManager;

    public static bool blockIsRunning;

    public int blockCount;
    /*public List<int> stimulusSequence;*/

    public bool isResponseMade;


    public string[] diskName;


    void Start()
    {

        diskName = new string[] { "a", "b", "c", "d", "e" };

        dataRecorder = GetComponent<DataRecorder>();
        stimulusCreation = GetComponent<StimulusCreation>();
        vrDeviceManager = GetComponent<VRDeviceManager>();
        physicalDeviceManager = GetComponent<PhysicalDeviceManager>();

        if (onBlockStart == null)
        {
            onBlockStart = new UnityEvent();
        }

        if (onBlockFinish == null)
        {
            onBlockFinish = new UnityEvent();
        }

      
        onBlockStart.AddListener(BeginTestingBlock);

        blockIsRunning = false;

        blockCount = 0;

      

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && onBlockStart != null && !blockIsRunning && physicalDeviceManager.isTouchWheelReady)
        {
            blockIsRunning = true;
            onBlockStart.Invoke();

         /*   physicalDeviceManager.SwitchPhysicalDisk(4);
            physicalDeviceManager.touchWheel.BaseStream.Flush();*/

        }
    }


    void BeginTestingBlock()
    {
        Debug.Log("start");
 
        StartCoroutine(ExperimentBlock());

    }


    IEnumerator ExperimentBlock()
    {

        Debug.Log("start coroutine");

        dataRecorder.currTrialCount = 0;

        foreach (List<int> stim in stimulusCreation.stimulusPairSequence)
        {
            Debug.Log("Trial: " + dataRecorder.currTrialCount);
            /*onTrialStart.Invoke();*/
            isResponseMade = false;

            Debug.Log("stim1:, " +  stim[1]);
            dataRecorder.currTrialCount += 1;
            dataRecorder.currDiskNo = stim[0];

    
            physicalDeviceManager.WriteSerialData(diskName[dataRecorder.currDiskNo]);
            while (!physicalDeviceManager.isTouchWheelReady)
            { yield return null; }
            physicalDeviceManager.WriteSerialData("p");
            while (!physicalDeviceManager.isTouchWheelReady)
            { yield return null; }
            vrDeviceManager.SwitchDisk(dataRecorder.currDiskNo);
            yield return new WaitForSeconds(1.5f);



           
            physicalDeviceManager.WriteSerialData("n");
            while (!physicalDeviceManager.isTouchWheelReady)
            { yield return null; }
            vrDeviceManager.SwitchDisk(-1);
            yield return new WaitForSeconds(1f);

            Debug.Log("stim2: " + stim[1]);
            dataRecorder.currDiskNo = stim[1];

            
            physicalDeviceManager.WriteSerialData(diskName[dataRecorder.currDiskNo]);
            while (!physicalDeviceManager.isTouchWheelReady)
            { yield return null; }
            physicalDeviceManager.WriteSerialData("p");
            while (!physicalDeviceManager.isTouchWheelReady)
            { yield return null; }
            vrDeviceManager.SwitchDisk(dataRecorder.currDiskNo);
            yield return new WaitForSeconds(1.5f);


            physicalDeviceManager.WriteSerialData("n");
            while (!physicalDeviceManager.isTouchWheelReady)
            { yield return null; }
            vrDeviceManager.SwitchDisk(-1);



            while (!isResponseMade)
            {

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    Debug.Log("response Made");
                    isResponseMade = true;
                    dataRecorder.currResponse = 0;
                    //add trial data when response is made
                    dataRecorder.trialDataTable.Rows.Add(dataRecorder.id, dataRecorder.initial, dataRecorder.age, dataRecorder.gender,
                    dataRecorder.frameCount, dataRecorder.timeElapsed,
                    dataRecorder.currTrialCount, stim[0], stim[1], dataRecorder.currResponse);
                    /*onTrialFinish.Invoke();*/
                }

                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    Debug.Log("response Made");
                    isResponseMade = true;
                    dataRecorder.currResponse = 1;

                    dataRecorder.trialDataTable.Rows.Add(dataRecorder.id, dataRecorder.initial, dataRecorder.age, dataRecorder.gender,
                    dataRecorder.frameCount, dataRecorder.timeElapsed,
                    dataRecorder.currTrialCount, stim[0], stim[1], dataRecorder.currResponse);
                    /*onTrialFinish.Invoke();*/
                }

                yield return null; 
            }

            yield return new WaitForSeconds(.5f);
        }
        blockIsRunning = false;
        onBlockFinish.Invoke();
        blockCount++;
        Debug.Log("finish");
    }


}
