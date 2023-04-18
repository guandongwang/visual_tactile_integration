using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoInspector : MonoBehaviour
{
    [Header("Session Info")]
    public int id = 1;
    public string initial = "test";
    public int age = 25;
    public Gender gender = new ();
    public Condition condition = new ();

    [Space]
    [Header("Block Info")]
    public int NumberOfBlocks = 1;
    public int NumberOfRepetitions = 1;

    [Space]
    [Header("Status Monitor")]
    public bool IsEyeTrackingCalibrated = false;
    public bool IsInputFinished = false;
    public bool IsStimulusCreated = false;
    public bool IsBlockRunning = false;
    public bool IsResponseMade = false;
    public bool IsFileSaved = false;

    public int CurrentBlock = 0;
    public int CurrentTrial = 0;



    [Space]
    [Header("Action")]
    public bool TryEyeCalibration;
    public bool ResetTrackerPosition;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
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
