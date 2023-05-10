using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using Random = System.Random;
using System.Linq;


public class StimulusGeneration : MonoBehaviour
{
    TestingSequence testingSequence;
    DataRecorder dataRecorder;
    InfoInspector infoInspector;

    public string tactileReferenceStim;
    public string visualReferenceStim;

   
    public Dictionary<string, string> tactileDisksDic;


    public List<string> tactileDisks;
    public List<string> visualDisks;

    public List<List<string>> tactileStimPair;
    public List<List<string>> visualStimPair;
    public List<int> blockStimPair;


    void OnEnable()
    {
        EventManager.StartListening("OnSessionStart", CreateStimulus);
    }

    void OnDisable()
    {
        EventManager.StopListening("OnSessionStart", CreateStimulus);
    }

    // Start is called before the first frame update
    void Start()
    {
        dataRecorder = GetComponent<DataRecorder>();
        testingSequence = GetComponent<TestingSequence>();
        infoInspector = GetComponent<InfoInspector>();


        tactileDisks = new List<string>() { "B3", "B7", "B11", "B15", "B19"};

        
        tactileDisksDic = new Dictionary<string, string>(5);
        tactileDisksDic.Add(tactileDisks[0], "a");
        tactileDisksDic.Add(tactileDisks[1], "b");
        tactileDisksDic.Add(tactileDisks[2], "c");
        tactileDisksDic.Add(tactileDisks[3], "d");
        tactileDisksDic.Add(tactileDisks[4], "e");
        tactileDisksDic.Add("dummy", "dummy");

    }



    void CreateStimulus() 
    {
        infoInspector.CurrentEvent = "Create simulus";

        switch (infoInspector.condition)
        {
            case InfoInspector.Condition.Vision:
                visualDisks = tactileDisks;
                tactileStimPair = DummyStimPair();
                visualStimPair = CreateCounterbalancedStimPair(visualDisks);
                break;

            case InfoInspector.Condition.Touch:
                tactileStimPair = CreateCounterbalancedStimPair(tactileDisks);
                visualStimPair = DummyStimPair();
                break;

            case InfoInspector.Condition.Combine:
                visualDisks = tactileDisks;
                tactileStimPair = CreateCounterbalancedStimPair(tactileDisks);
                visualStimPair = CreateCounterbalancedStimPair(visualDisks);
                break;

            case InfoInspector.Condition.VLowerFreqThanT: // in terms of disk number
                visualDisks = new List<string>() { "B1", "B5", "B9", "B13", "B17" };
                tactileStimPair = CreateCounterbalancedStimPair(tactileDisks);
                visualStimPair = CreateCounterbalancedStimPair(visualDisks);
                break;

            case InfoInspector.Condition.VHigherFreqThanT:
                visualDisks = new List<string>() { "B5", "B9", "B13", "B17", "B21" };
                tactileStimPair = CreateCounterbalancedStimPair(tactileDisks);
                visualStimPair = CreateCounterbalancedStimPair(visualDisks);
                break;
        }

        blockStimPair = ShuffleList(RepeatList(Enumerable.Range(0, 10).ToList(), infoInspector.NumberOfRepetitions));

        infoInspector.CurrentEvent = "Stimulus created";
        EventManager.TriggerEvent("OnStimulusCreated");
       

    }

    List<List<string>> DummyStimPair()
    {

        //couterbalanced stimulus for touch
        List<List<string>> stimPair = new List<List<string>>(10);
        for (int i = 0; i < 10; i++)
        { stimPair.Add(new List<string> { "dummy", "dummy" }); }
    
        return stimPair;
    }

    List<List<string>> CreateCounterbalancedStimPair(List<string> diskList)
    {
      
        //couterbalanced stimulus for touch
        List<List<string>> stimPair = new List<List<string>>(10);

        stimPair.Add(new List<string> { "B11", diskList[0] });
        stimPair.Add(new List<string> { "B11", diskList[1] });
        stimPair.Add(new List<string> { "B11", diskList[2] });
        stimPair.Add(new List<string> { "B11", diskList[3] });
        stimPair.Add(new List<string> { "B11", diskList[4] });

        stimPair.Add(new List<string> { diskList[0], "B11" });
        stimPair.Add(new List<string> { diskList[1], "B11" });
        stimPair.Add(new List<string> { diskList[2], "B11" });
        stimPair.Add(new List<string> { diskList[3], "B11" });
        stimPair.Add(new List<string> { diskList[4], "B11" });
       
        return stimPair;
    }


    List<T> RepeatList<T>(List<T> originalList, int repeatCount)
    {
        List<T> repeatedList = new List<T>();

        // Repeat the original list the specified number of times
        for (int i = 0; i < repeatCount; i++)
        {
            repeatedList.AddRange(originalList);
        }
        return repeatedList;
    }



    List<T> ShuffleList<T>(List<T> listToShuffle)
    {
        Random rng = new Random();
        for (int i = listToShuffle.Count - 1; i > 0; i--)
        {
            var k = rng.Next(i + 1);
            var value = listToShuffle[k];
            listToShuffle[k] = listToShuffle[i];
            listToShuffle[i] = value;
        }
        return listToShuffle;
    }
}
  
