using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoInspector : MonoBehaviour
{
    [Header("Session Info")]
    public int id;
    public string initial;
    public int age;
    public Gender gender = new ();
    public Condition condition = new ();

    [Space]
    [Header("Block Info")]
    public bool IsEyeTrackingNeeded;
    public int NumberOfBlocks;
    public int NumberOfRepetitions;

    [Space]
    [Header("Status Monitor")]
    public bool IsEyeTrackingCalibrated;
    public bool IsInputFinished;
    public bool IsStimulusCreated;
    public bool IsBlockRunning;
    public bool IsResponseMade;
    public bool IsFileSaved;

    public int CurrentBlock;
    public int CurrentTrial;



    [Space]
    [Header("Action")]
    public bool tryEyeCalibration;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log(gender);
        }
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
