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

        testingSequence.onPrepStart.AddListener(CreateDataFile);
       /* testingSequence.onBlockStart.AddListener(StartDataRecording);*/
        testingSequence.onBlockFinish.AddListener(FinishDataRecording);
    }

    void CreateDataFile()
    {
        trialData = new List<TrialDataEntry>();
       
        for (int i = 0; i< 50; i++)
        {
            TrialDataEntry trialDataEntry = new TrialDataEntry(ID, Initial, Age, Gender, Condition);
            

            trialDataEntry.CurrentTrialNumber = i;
            trialDataEntry.TestingStimPosition = stimulusGeneration.BlockTestingStimPosition[i];


            if (trialDataEntry.TestingStimPosition == 1)
            {
                trialDataEntry.S1Touch = stimulusGeneration.BlockTactileStim[i];
                trialDataEntry.S1Vision = stimulusGeneration.BlockVisualStim[i];
                trialDataEntry.S2Touch = stimulusGeneration.TactileReferenceStim;
                trialDataEntry.S2Vision = stimulusGeneration.VisualReferenceStim;
            }
            else if (trialDataEntry.TestingStimPosition == 2)
            {
                trialDataEntry.S1Touch = stimulusGeneration.TactileReferenceStim;
                trialDataEntry.S1Vision = stimulusGeneration.VisualReferenceStim;
                trialDataEntry.S2Touch = stimulusGeneration.BlockTactileStim[i];
                trialDataEntry.S2Vision = stimulusGeneration.BlockVisualStim[i];
            }

            trialData.Add(trialDataEntry);
            Debug.Log(trialDataEntry.GetValues());
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
