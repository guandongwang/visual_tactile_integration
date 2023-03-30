using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestingSequence : MonoBehaviour
{

    public int blockCount;
    public int maxBlockNumber;

    public UnityEvent onInputFinish;
    public UnityEvent onStimulusCreated;

    public UnityEvent onBlockStart;
    public UnityEvent onBlockFinish;

    public UnityEvent onTrialStart;
    public UnityEvent onTrialFinish;

    DataRecorder dataRecorder;
    StimulusGeneration stimulusGeneration;

    public bool isResponseMade;
    public bool IsBlockRunning;
    public bool IsEyeTrackingCalibrated = false;


    // Start is called before the first frame update
    void Start()
    {
        dataRecorder = GetComponent<DataRecorder>();
        stimulusGeneration = GetComponent<StimulusGeneration>();

        if (onInputFinish == null)
        {
            onInputFinish = new UnityEvent();
        }

        if (onStimulusCreated == null)
        {
            onStimulusCreated = new UnityEvent();
        }

        if (onBlockStart == null)
        {
            onBlockStart = new UnityEvent();
        }

        if (onBlockFinish == null)
        {
            onBlockFinish = new UnityEvent();
        }

        onBlockStart.AddListener(StartBlock);
        
        blockCount = 0;
        maxBlockNumber = 2;


    }

    // Update is called once per frame
    void Update()
    {
        bool _isBlockReady = IsEyeTrackingCalibrated
            && !IsBlockRunning 
            && onBlockStart != null 
            && blockCount < maxBlockNumber;

        if (Input.GetKeyDown(KeyCode.Space) & _isBlockReady)
        {
            onInputFinish.Invoke(); 
        }

    }

    void StartBlock()
    { 
        StartCoroutine(ExperimentBlock()); 
    }

    IEnumerator ExperimentBlock()
    {
        IsBlockRunning = true;
        Debug.Log("Block " + (blockCount + 1) + " Start");
        

        foreach (TrialDataEntry entry in dataRecorder.trialData)
        {
            isResponseMade = false;
            /*   entry.TrialStartTime = Time.time;
               Debug.Log(entry.GetValues());
               yield return null;*/
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

            while (!isResponseMade)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    entry.Response = "L"; //First stim higher freq
                    isResponseMade = true;
                }

                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    entry.Response = "R"; //Second stim higher freq
                    isResponseMade = true;
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
        blockCount++;
        IsBlockRunning = false;
        onBlockFinish.Invoke();
    }

}
