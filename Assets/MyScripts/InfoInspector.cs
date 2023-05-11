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
    //public Condition condition;

    [Space]
    [Header("Block Info")]
    public int NumberOfBlocks;
    public int NumberOfRepetitions ;

    [Space]
    [Header("Status Monitor")]
    public bool ReadyToStartBlock;
    public bool IsTouchWheelReady;
    public bool IsEyeTrackingCalibrated ;
    public bool IsInputFinished ;
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

     ReadyToStartBlock = !IsBlockRunning && CurrentBlock <= NumberOfBlocks;
     
        //if (condition != currentCondition)
        //{
        //    currentCondition = condition;
        //    CurrentBlock = 1;
        //}
   
    }


    //public enum Condition
    //{
    //    Vision,
    //    Touch,
    //    Combine,
    //    VLowerFreqThanT,
    //    VHigherFreqThanT
    //}

    public enum Gender
    {
        F,
        M,
        O
    }
}
