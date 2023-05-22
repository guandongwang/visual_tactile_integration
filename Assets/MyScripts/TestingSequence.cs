using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using HTC.UnityPlugin.Vive;

public class TestingSequence : MonoBehaviour
{
    DataRecorder dataRecorder;
    InfoInspector infoInspector;
    VrDeviceManager vrDeviceManager;
    PhysicalDeviceManager physicalDeviceManager;
    CreateStimulus createStimulus;

    GameObject device;

    public Dictionary<string, string> tactileDisksDic;

    // Start is called before the first frame update
    void Start()
    {

        dataRecorder = GetComponent<DataRecorder>();
        infoInspector = GetComponent<InfoInspector>();
        createStimulus = GetComponent<CreateStimulus>();
        device = GameObject.Find("Device");
        vrDeviceManager = device.GetComponent<VrDeviceManager>();
        physicalDeviceManager = device.GetComponent<PhysicalDeviceManager>();



        List<string> tactileDisks = new List<string>() { "B3", "B7", "B11", "B15", "B19" };
        tactileDisksDic = new Dictionary<string, string>();
        tactileDisksDic.Add(tactileDisks[0], "a");
        tactileDisksDic.Add(tactileDisks[1], "b");
        tactileDisksDic.Add(tactileDisks[2], "c");
        tactileDisksDic.Add(tactileDisks[3], "d");
        tactileDisksDic.Add(tactileDisks[4], "e");
        tactileDisksDic.Add("dummy", "dummy");

    }

    void OnEnable()
    {
        //EventManager.StartListening("OnTrialDataFileReady", StartBlock);
    }

    void OnDisable()
    {
        //EventManager.StopListening("OnTrialDataFileReady", StartBlock);
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) & infoInspector.IsExperimentReady)
        {
            infoInspector.IsTrackerEnabled = false;
            infoInspector.CurrentEvent = "OnBlockStart";
            Debug.Log("aba");
            StartCoroutine(ExperimentBlock());
            /*EventManager.TriggerEvent("OnBlockStart");*/
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Stop the coroutine if it's running
            if (infoInspector.IsBlockRunning)
            {
                infoInspector.IsBlockRunning = false;
                StopCoroutine(ExperimentBlock());
            }
        }


    }
/*
    void StartBlock()
    {
       *//* EventManager.TriggerEvent("OnBlockStart");*//*
        StartCoroutine(ExperimentBlock());
        Debug.Log("aaa");
    }
*/
   

    void StimulusToData(Stimulus stimulus, TrialDataEntry trialDataEntry)
    {
        trialDataEntry.Condition = stimulus.Condition;
        trialDataEntry.StimPairIndex = stimulus.StimPairIndex;
        trialDataEntry.ReferenceLocation = stimulus.ReferenceLocation;
        trialDataEntry.S1Vision = stimulus.S1Vision;
        trialDataEntry.S1Touch = stimulus.S1Vision;
        trialDataEntry.S1Steps = stimulus.S1Steps;
        trialDataEntry.S1Orientation = stimulus.S1Orientation;
        trialDataEntry.S2Vision = stimulus.S2Vision;
        trialDataEntry.S2Touch = stimulus.S2Touch;
        trialDataEntry.S2Steps = stimulus.S2Steps;
        trialDataEntry.S2Orientation = stimulus.S2Orientation;
        trialDataEntry.TargetResponse = stimulus.TargetResponse;
    }
    IEnumerator ExperimentBlock()
    {
        
        infoInspector.CurrentEvent = "Block " + (infoInspector.CurrentBlock) + " start";
        infoInspector.IsBlockRunning = true;

        List<Stimulus> blockStimulus = createStimulus.SessionStimulusCollection[infoInspector.CurrentBlock];
        List<TrialDataEntry> trialData = new List<TrialDataEntry>();
        //list<framedataentry> framedata = new list<framedataentry>();
        int index = 0;
        
        foreach (Stimulus stimulus in blockStimulus)
        {
            TrialDataEntry entry = new TrialDataEntry(infoInspector.id,
                infoInspector.initial, infoInspector.age,
                infoInspector.gender.ToString(),infoInspector.CurrentBlock);
            
            entry.TrialNumber = index;
            infoInspector.CurrentTrial = entry.TrialNumber;

            StimulusToData(stimulus, entry);
            
            infoInspector.IsResponseMade = false;

            //Trial Start
            entry.TrialStartTime = Time.time;
            infoInspector.CurrentEvent = "Trial " + (entry.TrialNumber) + " start";
        

            //S1 Begin
            entry.S1Begin = Time.time;

            if (stimulus.Condition == "Vision")
            {
                //Compensation for other condition's ITI
                yield return new WaitForSeconds(2.5f);
            }
            else
            {
                physicalDeviceManager.WriteSerialData(tactileDisksDic[stimulus.S1Touch] + stimulus.S1Steps);
                while (!infoInspector.IsTouchWheelReady)
                { yield return new WaitForSeconds(0.1f); }
                physicalDeviceManager.WriteSerialData("p");
                while (!infoInspector.IsTouchWheelReady)
                { yield return new WaitForSeconds(0.1f); }
            }
            vrDeviceManager.PresentDisk(stimulus.S1Vision, stimulus.S1Orientation);

            //S1 Presnetation Begin
            entry.S1PresnetationBegin = Time.time;
            yield return new WaitForSeconds(2f);
            //S1 End
            entry.S1End = Time.time;

            if (stimulus.Condition != "Vision")
            {
                physicalDeviceManager.WriteSerialData("n");
                while (!infoInspector.IsTouchWheelReady)
                { yield return new WaitForSeconds(0.1f); }
            }
            
            vrDeviceManager.HideDisk(entry.S1Vision);



            //S2
            entry.S2Begin = Time.time;

            if (stimulus.Condition == "Vision")
            {
                //Compensation for other Condition's ISI, not so important, so tune down to 1s
                yield return new WaitForSeconds(1f);
            }
            else
            {
                physicalDeviceManager.WriteSerialData(tactileDisksDic[stimulus.S2Touch] + stimulus.S2Steps);
                while (!infoInspector.IsTouchWheelReady)
                { yield return new WaitForSeconds(0.1f); }

                physicalDeviceManager.WriteSerialData("p");
                while (!infoInspector.IsTouchWheelReady)
                { yield return new WaitForSeconds(0.1f); }
            }
          
            entry.S2PresnetationBegin = Time.time;
            vrDeviceManager.PresentDisk(stimulus.S2Vision, stimulus.S2Orientation);
            yield return new WaitForSeconds(2f);

            //S2 Offset
            entry.S2End = Time.time;

            if (stimulus.Condition != "Vision")
            {
                physicalDeviceManager.WriteSerialData("n");
                while (!infoInspector.IsTouchWheelReady)
                { yield return null; } 
            }

            entry.S2PresnetationEnd = Time.time;
            vrDeviceManager.HideDisk(entry.S2Vision);
            //Response Cued
            entry.ResponseCued = Time.time;
            vrDeviceManager.PresentDisk("cue",0);

            while (!infoInspector.IsResponseMade)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    entry.Response = "U"; //Freq went up from 1 to 2
                    infoInspector.IsResponseMade = true;
                }

                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    entry.Response = "D"; //Freq went down from 1 to 2
                    infoInspector.IsResponseMade = true;
                }
                yield return null;
            }
            //Response Made
            entry.ResponseMade = Time.time;

            

            //Response Made
            vrDeviceManager.HideDisk("cue");
            entry.RespResult = entry.TargetResponse.Contains(entry.Response);
            Debug.Log("Resp Result: " + entry.RespResult);

         
            //Response Time
            entry.ResponseTime = entry.ResponseMade - entry.ResponseCued;

            yield return new WaitForSeconds(.1f);



            //Trial End
            entry.TrialEndTime = Time.time;
            //Trial Duration
            entry.TrialDuration = entry.TrialEndTime - entry.TrialStartTime;

            trialData.Add(entry);
            index++;
        }
        infoInspector.CurrentBlock++;
        infoInspector.IsBlockRunning = false;
        EventManager.TriggerEvent("BlockFinished");
        DataRecorder.SaveToCSV(trialData, "trial");
        DataRecorder.SaveToCSV(dataRecorder.FrameData, "frame");
        dataRecorder.FrameData = new List<FrameDataEntry>();//clear the frame data after saving
        infoInspector.IsTrackerEnabled = true;
        
    }

}
