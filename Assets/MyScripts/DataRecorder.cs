using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataRecorder : MonoBehaviour
{
    TestingSequence testingSequence;
    StimulusGeneration stimulusGeneration;

    public List<TrialDataEntry> trialData;
   
    //session Info
    public int ID;
    public string Initial;
    public int Age;
    public string Gender;
    public string Condition;

    // Start is called before the first frame update
    void Start()
    {
        testingSequence = GetComponent<TestingSequence>();
        stimulusGeneration = GetComponent<StimulusGeneration>();

        testingSequence.onStimulusCreated.AddListener(CreateDataFile);
       /* testingSequence.onBlockStart.AddListener(StartDataRecording);*/
      /*  testingSequence.onBlockFinish.AddListener(FinishDataRecording);*/
    }

    void CreateDataFile()
    {
        trialData = new List<TrialDataEntry>();

        int _index = 0;
        foreach(int stimPairIndex in stimulusGeneration.blockStimPair)
        {
            TrialDataEntry trialDataEntry = new TrialDataEntry(ID, Initial, Age, Gender, Condition);
            

            trialDataEntry.CurrentTrialNumber = _index;
            trialDataEntry.StimPairIndex = stimPairIndex;

            trialDataEntry.S1Touch = stimulusGeneration.tactileStimPair[_index][0];
            trialDataEntry.S1Vision = stimulusGeneration.visualStimPair[_index][0];
            trialDataEntry.S2Touch = stimulusGeneration.tactileStimPair[_index][1];
            trialDataEntry.S2Vision = stimulusGeneration.visualStimPair[_index][1];

            trialData.Add(trialDataEntry);
            Debug.Log(trialDataEntry.GetValues());
            _index++;
        }

        testingSequence.onBlockStart.Invoke();
    }

    void FinishDataRecording()
    {
        
    }

   

    // Update is called once per frame
    void Update()
    {
        
    }
}
