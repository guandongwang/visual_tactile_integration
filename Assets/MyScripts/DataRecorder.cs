using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataRecorder : MonoBehaviour
{
    TestingSequence testingSequence;

    public List<TrialDataEntry> trialData;
   
    //session Info
    public int id;
    public string initial;
    public int age;
    public string gender;
    public string condition;

    // Start is called before the first frame update
    void Start()
    {
        testingSequence = GetComponent<TestingSequence>();
        testingSequence.onBlockStart.AddListener(StartDataRecording);
        testingSequence.onBlockFinish.AddListener(FinishDataRecording);
    }

    void StartDataRecording()
    {
        trialData = new List<TrialDataEntry>();
           
        for (int i = 0; i< 50; i++)
        {
            TrialDataEntry trialDataEntry = new TrialDataEntry(id, initial, age,gender,condition);
            trialData.Add(trialDataEntry);
        }
        
    }

    void FinishDataRecording()
    {
        
    }

   

    // Update is called once per frame
    void Update()
    {
        
    }
}
