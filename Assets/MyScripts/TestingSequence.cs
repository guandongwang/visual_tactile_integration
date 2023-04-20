using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestingSequence : MonoBehaviour
{
    DataRecorder dataRecorder;
    StimulusGeneration stimulusGeneration;
    InfoInspector infoInspector;


    // Start is called before the first frame update
    void Start()
    {
        dataRecorder = GetComponent<DataRecorder>();
        stimulusGeneration = GetComponent<StimulusGeneration>();
        infoInspector = GetComponent<InfoInspector>();

    }

    // Update is called once per frame
    void Update()
    {       

        bool readyToCreateStimulus = infoInspector.IsEyeTrackingCalibrated
            && !infoInspector.IsBlockRunning
            && infoInspector.CurrentBlock < infoInspector.NumberOfBlocks;

        if (Input.GetKeyDown(KeyCode.Space) & readyToCreateStimulus)
        {
            EventManager.TriggerEvent("InputFinish", null);
            EventManager.StartListening("DataFileReady", StartBlock);
        }

        
    }

   void OnDisable()
    {
        EventManager.StopListening("DataFileReady", StartBlock);
    }

    void StartBlock(Dictionary<string, object> message)
    {
        Debug.Log("Event: DataFileReady");
        StartCoroutine(ExperimentBlock()); 
    }

    IEnumerator ExperimentBlock()
    {
        infoInspector.IsBlockRunning = true;

        Debug.Log("Block " + (infoInspector.CurrentBlock + 1) + " Start");
        

        foreach (TrialDataEntry entry in dataRecorder.trialData)
        {
            infoInspector.CurrentTrial = entry.TrialNumber + 1;

            infoInspector.IsResponseMade = false;
            //Trial Start
            entry.TrialStartTime = Time.time;
            Debug.Log("Trial " + entry.TrialNumber + " Start");

            //S1 Onset
            entry.S1Onset = Time.time;
            Debug.Log("S1 Touch: " + entry.S1Touch + ", S1 Vision: " + entry.S1Vision);
            //S1 Offset
            entry.S1Offset = Time.time;


            //Trial Start
            entry.S2Onset = Time.time;
            Debug.Log("S2 Touch: " + entry.S2Touch + ", S2 Vision: " + entry.S2Vision);
       

            //S2 Offset
            entry.S2Offset = Time.time;

            //Response Cued
            entry.ResponseCued = Time.time;

            while (!infoInspector.IsResponseMade)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    entry.Response = "L"; //First stim higher freq
                    infoInspector.IsResponseMade = true;
                }

                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    entry.Response = "R"; //Second stim higher freq
                    infoInspector.IsResponseMade = true;
                }
                yield return null;
            }
            //Response Made
            entry.RespResult = entry.TargetResponse.Contains(entry.Response);
            Debug.Log("Resp Result: " + entry.RespResult);

            //Response Made
            entry.ResponseMade = Time.time;
            //Response Time
            entry.ResponseTime = entry.ResponseMade - entry.ResponseCued;

            yield return new WaitForSeconds(.1f);
     

            //Trial End
            entry.TrialEndTime = Time.time;
            //Trial Duration
            entry.TrialDuration = entry.TrialEndTime - entry.TrialStartTime;

            Debug.Log(entry.GetHeaders());
            Debug.Log(entry.GetValues());
        }
        infoInspector.CurrentBlock++;
        infoInspector.IsBlockRunning = false;
        EventManager.TriggerEvent("BlockFinished", null);
    }

}
