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
    InfoInspector infoInspector;
    public List<TrialDataEntry> trialData;
    
   


    // Start is called before the first frame update
    void Start()
    {
        testingSequence = GetComponent<TestingSequence>();
        stimulusGeneration = GetComponent<StimulusGeneration>();
        frameDataRecorder = GetComponent<FrameDataRecorder>();
        infoInspector = GetComponent<InfoInspector>();


    }

    void OnEnable()
    {
        EventManager.StartListening("StimulusCreated", CreateDataFile);
        EventManager.StartListening("BlockFinished", SaveToCSV);
    }

    void OnDisable()
    {
        EventManager.StopListening("StimulusCreated", CreateDataFile);
        EventManager.StopListening("BlockFinished", SaveToCSV);
    }

    
    void CreateDataFile(Dictionary<string, object> message)
    {
       
        trialData = new List<TrialDataEntry>();

        Debug.Log("blockStimPair count: " + stimulusGeneration.blockStimPair.Count);
        int index = 0;

        foreach(int stimPairIndex in stimulusGeneration.blockStimPair)
        {
            TrialDataEntry trialDataEntry = new TrialDataEntry(infoInspector.id, infoInspector.initial, infoInspector.age, infoInspector.gender.ToString(), infoInspector.condition.ToString());

            trialDataEntry.TrialNumber = index;
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
            index++;
          
        }

        EventManager.TriggerEvent("DataFileReady", null);
    }

    void SaveToCSV(Dictionary<string, object> message)
    {
        string baseFolder = "C:\\Users\\gwan5836\\OneDrive - The University of Sydney (Staff)\\2023\\vr texture integration\\raw data\\";

        string subjectFolder =  trialData[0].ID + "_" + trialData[0].Initial;

        string fileName = "trial_" + trialData[0].ID + "_" + trialData[0].Initial + "_" + trialData[0].Condition + "_" + System.DateTime.Now.ToString("yyyy_MM_dd_(HH.mm.ss)") + ".csv";

        string folderPath = Path.Combine(baseFolder, subjectFolder);

        if (!System.IO.Directory.Exists(folderPath))
        {
            System.IO.Directory.CreateDirectory(folderPath);
        }


        string filePath = Path.Combine(folderPath, fileName);

        string headers = trialData[0].GetHeaders();

        StringBuilder dataFile = new();
        dataFile.AppendLine(headers);

        foreach (TrialDataEntry entry in trialData)
        {
            dataFile.AppendLine(entry.GetValues());
        }

        File.WriteAllText(filePath, dataFile.ToString());
        Debug.Log("Trial Data Saved");
    }


    // Update is called once per frame
    void Update()
    {

    }
}
