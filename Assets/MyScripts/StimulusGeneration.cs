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

    public int numberOfRepetitions;

    public string tactileReferenceStim;
    public string visualReferenceStim;

    public List<int> blockTactileStim;
    public List<int> blockVisualStim;
   /* public List<int> BlockTestingStimPosition;*/

   
    public Dictionary<string, string> tactileDisksDic;


    public List<string> tactileDisks;
    public List<string> visualDisks;

    public List<List<string>> tactileStimPair;
    public List<List<string>> visualStimPair;
    public List<int> blockStimPair;


    void OnEnable()
    {
        EventManager.StartListening("InputFinish", CreateStimulus);
    }

    void OnDisable()
    {
        EventManager.StopListening("InputFinish", CreateStimulus);
    }

    // Start is called before the first frame update
    void Start()
    {
        dataRecorder = GetComponent<DataRecorder>();
        testingSequence = GetComponent<TestingSequence>();

        numberOfRepetitions = 1;

        tactileDisks = new List<string>() { "B7", "B9", "B11", "B13", "B15" };

        
        tactileDisksDic = new Dictionary<string, string>(5);
        tactileDisksDic.Add(tactileDisks[0], "A");
        tactileDisksDic.Add(tactileDisks[1], "B");
        tactileDisksDic.Add(tactileDisks[2], "C");
        tactileDisksDic.Add(tactileDisks[3], "D");
        tactileDisksDic.Add(tactileDisks[4], "E");

        
    }



    void CreateStimulus(Dictionary<string, object> message) 
    {

        switch (dataRecorder.condition)
        {
            case "V":
                visualDisks = tactileDisks;
                tactileStimPair = DummyStimPair();
                visualStimPair = CreateCounterbalancedStimPair(visualDisks);
                break;

            case "T":
                tactileStimPair = CreateCounterbalancedStimPair(tactileDisks);
                visualStimPair = DummyStimPair();
                break;

            case "V=T":
                visualDisks = tactileDisks;
                tactileStimPair = CreateCounterbalancedStimPair(tactileDisks);
                visualStimPair = CreateCounterbalancedStimPair(visualDisks);
                break;

            case "V<T": // in terms of disk number
                visualDisks = new List<string>() { "B6", "B8", "B10", "B12", "B14" };
                tactileStimPair = CreateCounterbalancedStimPair(tactileDisks);
                visualStimPair = CreateCounterbalancedStimPair(visualDisks);
                break;

            case "V>T":
                visualDisks = new List<string>() { "B8", "B10", "B12", "B14", "B16" };
                tactileStimPair = CreateCounterbalancedStimPair(tactileDisks);
                visualStimPair = CreateCounterbalancedStimPair(visualDisks);
                break;
        }

        blockStimPair = ShuffleList(RepeatList(Enumerable.Range(0, 10).ToList(), numberOfRepetitions));
        Debug.Log("Stimulus Created");
        Debug.Log("tactileStimPair" + tactileStimPair.Count);
        /*        foreach (int i in blockStimPair)
                { Debug.Log(i); }*/


        EventManager.TriggerEvent("StimulusCreated", null);
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

        stimPair.Add(new List<string> { diskList[2], diskList[0] });
        stimPair.Add(new List<string> { diskList[2], diskList[1] });
        stimPair.Add(new List<string> { diskList[2], diskList[2] });
        stimPair.Add(new List<string> { diskList[2], diskList[3] });
        stimPair.Add(new List<string> { diskList[2], diskList[4] });

        stimPair.Add(new List<string> { diskList[0], diskList[2] });
        stimPair.Add(new List<string> { diskList[1], diskList[2] });
        stimPair.Add(new List<string> { diskList[2], diskList[2] });
        stimPair.Add(new List<string> { diskList[3], diskList[2] });
        stimPair.Add(new List<string> { diskList[4], diskList[2] });
       
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
  
