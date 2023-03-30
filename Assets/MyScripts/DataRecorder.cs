using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;



public class DataRecorder : MonoBehaviour
{
    TestingSequence testingSequence;
    StimulusGeneration stimulusGeneration;
    FrameDataRecorder frameDataRecorder;
    public List<TrialDataEntry> trialData;
    
   
    //session Info
    public int id;
    public string initial;
    public int age;
    public string gender;
    public string condition;

    
    private string _trialDataFileName;

    // Start is called before the first frame update
    void Start()
    {
        /*trialData = new List<TrialDataEntry>();*/

        testingSequence = GetComponent<TestingSequence>();
        stimulusGeneration = GetComponent<StimulusGeneration>();
        frameDataRecorder = GetComponent<FrameDataRecorder>();

        testingSequence.onStimulusCreated.AddListener(CreateDataFile);
        /*testingSequence.onBlockStart.AddListener(FinishDataPrep);*/
        /* testingSequence.onBlockStart.AddListener(StartDataRecording);*/
        testingSequence.onBlockFinish.AddListener(SaveToCSV);
    }

    /*void DataPrep() 
    { StartCoroutine(CreateDataFile()); }*/



    void CreateDataFile()
    {
       
        trialData = new List<TrialDataEntry>();

        Debug.Log("blockStimPair count: " + stimulusGeneration.blockStimPair.Count);
        int _index = 0;

        foreach(int stimPairIndex in stimulusGeneration.blockStimPair)
        {
            TrialDataEntry trialDataEntry = new TrialDataEntry(id, initial, age, gender, condition);

            trialDataEntry.TrialNumber = _index;
            trialDataEntry.StimPairIndex = stimPairIndex;
            trialDataEntry.IsTestingStimAtPos2 = stimPairIndex < 5;

            trialDataEntry.S1Touch = stimulusGeneration.tactileStimPair[stimPairIndex][0];
            trialDataEntry.S1Vision = stimulusGeneration.visualStimPair[stimPairIndex][0];
            trialDataEntry.S2Touch = stimulusGeneration.tactileStimPair[stimPairIndex][1];
            trialDataEntry.S2Vision = stimulusGeneration.visualStimPair[stimPairIndex][1];

            switch (stimPairIndex)
            {
                case 0:
                case 1:
                case 8:
                case 9:
                    trialDataEntry.TargetResponse = "L";
                    break;
                case 3:
                case 4:
                case 5:
                case 6:
                    trialDataEntry.TargetResponse = "R";
                    break;
                case 2:
                case 7:
                    trialDataEntry.TargetResponse = "LR";
                    break;
           

            }

            
            trialData.Add(trialDataEntry);
            _index++;
          
        }

        testingSequence.onBlockStart.Invoke();
    }

    void SaveToCSV()
    {
        string _folderPath = @"C:/Users/gwan5836/OneDrive - The University of Sydney (Staff)/2023/vr texture integration/raw data/" + trialData[0].ID + "_" + trialData[0].Initial;
        string _trialDataFileName = "trial_" + trialData[0].ID + "_" + trialData[0].Initial + "_" + trialData[0].Condition + "_" + System.DateTime.Now.ToString("yyyy_MM_dd_(HH.mm.ss)") + ".csv";

        if (!System.IO.Directory.Exists(_folderPath))
        {
            System.IO.Directory.CreateDirectory(_folderPath);
        }


        string _filePath = _folderPath + '/' + _trialDataFileName;

        string headers = trialData[0].GetHeaders();

        StringBuilder dataFile = new();
        dataFile.AppendLine(headers);

        foreach (TrialDataEntry entry in trialData)
        {
            dataFile.AppendLine(entry.GetValues());
        }

        File.WriteAllText(_filePath, dataFile.ToString());
        Debug.Log("Trial Data Saved");
    }


    // Update is called once per frame
    void Update()
    {
/*        Debug.Log(transform.position);
        Debug.Log(transform.rotation);*/
    }
}
