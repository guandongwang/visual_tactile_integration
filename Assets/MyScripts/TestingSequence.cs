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

    void OnEnable()
    {
        EventManager.StartListening("DataFileReady", StartBlock);
    }

    void OnDisable()
    {
        EventManager.StopListening("DataFileReady", StartBlock);
    }
    // Update is called once per frame
    void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.Space) & infoInspector.ReadyToStartBlock)
        {
            infoInspector.CurrentEvent = "Input Finished";
            EventManager.TriggerEvent("InputFinish");
        }

        
    }

    void StartBlock()
    {
        Debug.Log("Start Coroutine");
        StartCoroutine(ExperimentBlock()); 
    }

    IEnumerator ExperimentBlock()
    {
        
        infoInspector.CurrentEvent = "Block " + (infoInspector.CurrentBlock) + " start";
        infoInspector.IsBlockRunning = true;
        

        foreach (TrialDataEntry entry in dataRecorder.trialData)
        {
           
            infoInspector.CurrentTrial = entry.TrialNumber;

            infoInspector.IsResponseMade = false;
            //Trial Start
            entry.TrialStartTime = Time.time;
            infoInspector.CurrentEvent = "Trial " + (entry.TrialNumber + 1) + " start";
        

            //S1 Onset
            entry.S1Onset = Time.time;


            //S1 Offset
            entry.S1Offset = Time.time;


            //Trial Start
            entry.S2Onset = Time.time;



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

           
        }
        infoInspector.CurrentBlock++;
        infoInspector.IsBlockRunning = false;
        EventManager.TriggerEvent("BlockFinished");
    }

}
