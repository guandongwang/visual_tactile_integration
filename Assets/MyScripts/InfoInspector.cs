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
    public Condition condition;

    [Space]
    [Header("Block Info")]
    public int NumberOfBlocks;
    public int NumberOfRepetitions ;

    [Space]
    [Header("Status Monitor")]
    public bool ReadyToStartBlock;
    public bool IsEyeTrackingCalibrated ;
    public bool IsInputFinished ;
    public bool IsStimulusCreated ;
    public bool IsBlockRunning ;
    public bool IsResponseMade ;
    public bool IsFileSaved ;

    public int CurrentBlock;
    public int CurrentTrial;

    public string CurrentEvent;


    [Space]
    [Header("Action")]
    public bool TryEyeCalibration;
    public bool ResetTrackerPosition;


    // Start is called before the first frame update
    void Start()
    {
        NumberOfBlocks = 1;
        NumberOfRepetitions = 1;


        IsEyeTrackingCalibrated = false;
        IsInputFinished = false;
        IsStimulusCreated = false;
        IsBlockRunning = false;
        IsResponseMade = false;
        IsFileSaved = false;

        CurrentBlock = 1;
    }

    // Update is called once per frame
    void Update()
    {
            ReadyToStartBlock =!IsBlockRunning && CurrentBlock <= NumberOfBlocks;
    }

    public enum Condition
    {
        Vison,
        Touch,
        Combine,
        VLowerFreqThanT,
        VHigherFreqThanT
    }

    public enum Gender
    {
        F,
        M,
        O
    }
}
