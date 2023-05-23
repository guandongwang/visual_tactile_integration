using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoInspector : MonoBehaviour
{
    [Header("Mode")]
    public bool IsDebugMode;

    [Space]
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

    public bool IsBlockRunning ;
    public bool IsResponseMade ;


    public int CurrentBlock;
    public int CurrentTrial;
    public string CurrentTrialCondition;

    public string CurrentEvent;
    public string TouchWheelMessage;


    [Space]
    [Header("Action")]
    public bool TryEyeCalibration;
    public bool IsTrackerEnabled;
    public bool TryResetTouchwheel;

    PhysicalDeviceManager physicalDeviceManager;
    TestingSequence testingSequence;
    GameObject device;
    

    //private Condition currentCondition;
    // Start is called before the first frame update
    void Start()
    {

        IsEyeTrackingCalibrated = false;
        IsTouchWheelReady = false;
        IsBlockRunning = false;
        IsResponseMade = false;

        IsTrackerEnabled = true;
        TryResetTouchwheel = false;

        device = GameObject.Find("Device");
        physicalDeviceManager = device.GetComponent<PhysicalDeviceManager>();
        testingSequence = GetComponent<TestingSequence>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TryResetTouchwheel)
        {
            TryResetTouchwheel = false;
            physicalDeviceManager.WriteSerialData("r");
        }


        if (IsDebugMode)
        { IsExperimentReady = !IsBlockRunning && IsTouchWheelReady && CurrentBlock <= NumberOfBlocks; }
        else 
        { 
            IsExperimentReady = !IsBlockRunning && IsTouchWheelReady && IsEyeTrackingCalibrated && CurrentBlock <= NumberOfBlocks; 
        }
        
     
    }

    public enum Gender
    {
        F,
        M,
        O
    }
}
