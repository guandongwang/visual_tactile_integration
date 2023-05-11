using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using HTC.UnityPlugin.Vive;

public class TestingSequence : MonoBehaviour
{
    DataRecorder dataRecorder;
    StimulusGeneration stimulusGeneration;
    InfoInspector infoInspector;
    VrDeviceManager vrDeviceManager;
    PhysicalDeviceManager physicalDeviceManager;
    CreateStimulus createStimulus;

    GameObject device;

   

    // Start is called before the first frame update
    void Start()
    {
        

        dataRecorder = GetComponent<DataRecorder>();
        stimulusGeneration =GetComponent<StimulusGeneration>();
        infoInspector = GetComponent<InfoInspector>();

        device = GameObject.Find("Device");
        vrDeviceManager = device.GetComponent<VrDeviceManager>();
        physicalDeviceManager = device.GetComponent<PhysicalDeviceManager>();

    }

    void OnEnable()
    {
        EventManager.StartListening("OnTrialDataFileReady", StartBlock);
    }

    void OnDisable()
    {
        EventManager.StopListening("OnTrialDataFileReady", StartBlock);
    }
    // Update is called once per frame
    void Update()
    {

       

        if (Input.GetKeyDown(KeyCode.Space) & infoInspector.ReadyToStartBlock)
        {
            infoInspector.IsTrackerEnabled = false;
            infoInspector.CurrentEvent = "OnSessionStart";
            EventManager.TriggerEvent("OnSessionStart");
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

    void StartBlock()
    {
        EventManager.TriggerEvent("OnBlockStart");
        StartCoroutine(ExperimentBlock());


    }

    IEnumerator ExperimentBlockVisionOnly()
    {
        physicalDeviceManager.enabled = false;
        
        infoInspector.CurrentEvent = "Block " + (infoInspector.CurrentBlock) + " start";
        infoInspector.IsBlockRunning = true;


        foreach (TrialDataEntry entry in dataRecorder.trialData)
        {

            infoInspector.CurrentTrial = entry.TrialNumber;

            infoInspector.IsResponseMade = false;
            //Trial Start
            entry.TrialStartTime = Time.time;
            infoInspector.CurrentEvent = "Trial " + (entry.TrialNumber) + " start";

            //S1 Onset
            entry.S1Onset = Time.time;
            vrDeviceManager.PresentDisk(entry.S1Vision);
            yield return new WaitForSeconds(2f);

            //S1 Offset
            entry.S1Offset = Time.time;


            vrDeviceManager.HideDisk(entry.S1Vision);

            //compensate for other condition
            yield return new WaitForSeconds(2.5f);


            //s2
            entry.S2Onset = Time.time;
            vrDeviceManager.PresentDisk(entry.S2Vision);
            yield return new WaitForSeconds(2f);

            //S2 Offset
            entry.S2Offset = Time.time;
            vrDeviceManager.HideDisk(entry.S2Vision);

            //Response Cued
            entry.ResponseCued = Time.time;
            vrDeviceManager.PresentDisk("cue");

            while (!infoInspector.IsResponseMade)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    entry.Response = "U"; //Freqeuncy went up from 1 to 2
                    infoInspector.IsResponseMade = true;
                }

                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    entry.Response = "D"; //Freqeuncy went down from 1 to 2
                    infoInspector.IsResponseMade = true;
                }
                yield return null;
            }
            //Response Made
            vrDeviceManager.HideDisk("cue");
            entry.RespResult = entry.TargetResponse.Contains(entry.Response);
            Debug.Log("Resp Result: " + entry.RespResult);

            //Response Made
            entry.ResponseMade = Time.time;
            //Response Time
            entry.ResponseTime = entry.ResponseMade - entry.ResponseCued;

            yield return new WaitForSeconds(1f);


            //Trial End
            entry.TrialEndTime = Time.time;
            //Trial Duration
            entry.TrialDuration = entry.TrialEndTime - entry.TrialStartTime;


        }
        infoInspector.CurrentBlock++;
        infoInspector.IsBlockRunning = false;
        EventManager.TriggerEvent("BlockFinished");
        infoInspector.IsTrackerEnabled = true;
    }

    void StimulusToData(Stimulus stimulus, TrialDataEntry trialDataEntry)
    {

        trialDataEntry.Condition = stimulus.Condition;
        trialDataEntry.StimPairIndex = stimulus.StimPairIndex;
        trialDataEntry.S1Vision = stimulus.S1Vision;
        trialDataEntry.S1Touch = stimulus.S1Vision;
        trialDataEntry.S2Vision = stimulus.S2Vision;
        trialDataEntry.S2Touch = stimulus.S2Touch;
    }
    IEnumerator ExperimentBlock()
    {
        
        infoInspector.CurrentEvent = "Block " + (infoInspector.CurrentBlock) + " start";
        infoInspector.IsBlockRunning = true;

        List<Stimulus> blockStimulus = createStimulus.SessionStimulusCollection[infoInspector.CurrentBlock];

        int index = 0;

        foreach (Stimulus stimulus in blockStimulus)
        {
            TrialDataEntry entry = new TrialDataEntry(infoInspector.id, infoInspector.initial, infoInspector.age, infoInspector.gender.ToString());

            entry.TrialNumber = index;

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
                physicalDeviceManager.WriteSerialData(stimulusGeneration.tactileDisksDic[entry.S1Touch]);
                while (!infoInspector.IsTouchWheelReady)
                { yield return new WaitForSeconds(0.1f); }
                physicalDeviceManager.WriteSerialData("p");
                while (!infoInspector.IsTouchWheelReady)
                { yield return new WaitForSeconds(0.1f); }
            }
            vrDeviceManager.PresentDisk(entry.S1Vision);

            //S1 Presnetation Begin
            entry.S1PresnetationBegin = Time.time;
            yield return new WaitForSeconds(2f);
            //S1 End
            entry.S1PresnetationEnd = Time.time;

            if (stimulus.Condition != "Vision")
            {
                physicalDeviceManager.WriteSerialData("n");
                while (!infoInspector.IsTouchWheelReady)
                { yield return new WaitForSeconds(0.1f); }
            }
            vrDeviceManager.HideDisk(entry.S1Vision);
            //S1 Presnetation End
            entry.S1PresnetationEnd = Time.time;

 


            //S2
            entry.S2Begin = Time.time;

            if (stimulus.Condition == "Vision")
            {
                //Compensation for other Condition's ISI
                yield return new WaitForSeconds(2.5f);
            }
            else
            {
                physicalDeviceManager.WriteSerialData(stimulusGeneration.tactileDisksDic[entry.S2Touch]);
                while (!infoInspector.IsTouchWheelReady)
                { yield return new WaitForSeconds(0.1f); }

                physicalDeviceManager.WriteSerialData("p");
                while (!infoInspector.IsTouchWheelReady)
                { yield return new WaitForSeconds(0.1f); }
            }
          
            entry.S2PresnetationBegin = Time.time;
            vrDeviceManager.PresentDisk(entry.S2Vision);
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
            vrDeviceManager.PresentDisk("cue");

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

           
        }
        infoInspector.CurrentBlock++;
        infoInspector.IsBlockRunning = false;
        EventManager.TriggerEvent("BlockFinished");
        infoInspector.IsTrackerEnabled = true;
    }

}
