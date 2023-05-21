using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoInspector : MonoBehaviour
{
    [Header("Session Info")]
    public int id;
    public string initial;
    public int age;
    public Gender gender;


    [Space]
    [Header("Block Info")]
    public int NumberOfBlocks;
    public int NumberOfRepetitions ;

    [Space]
    [Header("Experiment Status")]
    public bool IsExperimentReady;
    public bool IsTouchWheelReady;
    public bool IsEyeTrackingCalibrated;

    [Space]
    [Header("Status Trigger")]
    public bool IsInputFinished;
    public bool IsStimulusCreated ;
    public bool IsBlockRunning ;
    public bool IsResponseMade ;
    public bool IsFileSaved ;

    public int CurrentBlock;
    public int CurrentTrial;

    public string CurrentEvent;
    public string TouchWheelMessage;


    [Space]
    [Header("Action")]
    public bool TryEyeCalibration;
    public bool IsTrackerEnabled;


    //private Condition currentCondition;
    // Start is called before the first frame update
    void Start()
    {

        IsEyeTrackingCalibrated = false;
        IsTouchWheelReady = false;
        IsInputFinished = false;
        IsStimulusCreated = false;
        IsBlockRunning = false;
        IsResponseMade = false;
        IsFileSaved = false;

        CurrentBlock = 1;

        IsTrackerEnabled = true;

        //currentCondition = condition;
    }

    // Update is called once per frame
    void Update()
    {

        IsExperimentReady = !IsBlockRunning && IsTouchWheelReady && IsEyeTrackingCalibrated && CurrentBlock <= NumberOfBlocks;
     
    }

    public enum Gender
    {
        F,
        M,
        O
    }
}
