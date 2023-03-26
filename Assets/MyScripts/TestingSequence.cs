using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestingSequence : MonoBehaviour
{

    public int BlockCount;
    public int TrialCount;

    public UnityEvent onInputFinish;
    public UnityEvent onStimulusCreated;

    public UnityEvent onBlockStart;
    public UnityEvent onBlockFinish;

    public UnityEvent onTrialStart;
    public UnityEvent onTrialFinish;

    DataRecorder dataRecorder;
    StimulusGeneration stimulusGeneration;
    public bool IsResponseMade;

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

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && onBlockStart != null)
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
        Debug.Log("Block " + BlockCount + " Start");

        int TrialCount = 0;

        foreach (TrialDataEntry entry in dataRecorder.trialData)
        {

            Debug.Log("Trial " + TrialCount + " Start");

            Debug.Log("Stimulus 1 Touch: " + entry.S1Touch);


            TrialCount++;
            yield return new WaitForSeconds(.1f);
            Debug.Log(entry.GetValues());

        }


    }

}
