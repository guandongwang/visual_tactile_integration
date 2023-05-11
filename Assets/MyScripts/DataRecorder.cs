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
    CreateStimulus createStimulus;


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
        EventManager.StartListening("OnStimulusCreated", CreateDataFile);
        EventManager.StartListening("BlockFinished", SaveToCSV);
    }

    void OnDisable()
    {
        EventManager.StopListening("OnStimulusCreated", CreateDataFile);
        EventManager.StopListening("BlockFinished", SaveToCSV);
    }

    
    void CreateDataFile()
    {
        infoInspector.CurrentEvent = "Creat Trial Data File";
        trialData = new List<TrialDataEntry>();

        //get the stimulus list for current block from createStimulus 
        List<Stimulus> blockStimulus = createStimulus.SessionStimulusCollection[infoInspector.CurrentBlock];
        int index = 0;

        foreach(Stimulus stimulus in blockStimulus)
        {
            TrialDataEntry trialDataEntry = new TrialDataEntry(infoInspector.id, infoInspector.initial, infoInspector.age, infoInspector.gender.ToString(), infoInspector.condition.ToString());

            trialDataEntry.TrialNumber = index;
            trialDataEntry.StimPairIndex = stimulus.StimPairIndex;
            //trialDataEntry.IsTestingStimAtPos2 = stimPairIndex < 5;

            trialDataEntry.S1Vision = stimulus.S1Vision;
            trialDataEntry.S1Touch = stimulus.S1Vision;
            trialDataEntry.S2Vision = stimulus.S2Vision;
            trialDataEntry.S2Touch = stimulus.S2Touch;
             //trialDataEntry.S1VisionOri = 0;
            //trialDataEntry.S2VisionOri = 0;

            switch (stimulus.StimPairIndex)
            {
                case 0:
                case 1:
                case 8:
                case 9:
                    trialDataEntry.TargetResponse = "D";
                    break;
                case 3:
                case 4:
                case 5:
                case 6:
                    trialDataEntry.TargetResponse = "U";
                    break;
                case 2:
                case 7:
                    trialDataEntry.TargetResponse = "DU";
                    break;
            }

            
            trialData.Add(trialDataEntry);
            index++;
          
        }

        infoInspector.CurrentEvent = "OnTrialDataFileReady";
        EventManager.TriggerEvent("OnTrialDataFileReady");

    }

    void SaveToCSV()
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
