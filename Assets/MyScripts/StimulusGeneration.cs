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

    DataRecorder dataRecorder;

    public int numberOfRepetitions;

    public string TactileReferenceStim;
    public string VisualReferenceStim;

    public List<string> BlockTactileStim;
    public List<string> BlockVisualStim;
    public List<int> BlockTestingStimPosition;


    public Dictionary<string, string> tactileDisksDic;

    public List<string> tactileDisks;

   


    // Start is called before the first frame update
    void Start()
    {

        numberOfRepetitions = 10;

        tactileDisks = new List<string>() { "B7", "B9", "B11", "B13", "B15" };

        tactileDisksDic = new Dictionary<string, string>(5);
        tactileDisksDic.Add(tactileDisks[0], "A");
        tactileDisksDic.Add(tactileDisks[1], "B");
        tactileDisksDic.Add(tactileDisks[2], "C");
        tactileDisksDic.Add(tactileDisks[3], "D");
        tactileDisksDic.Add(tactileDisks[4], "E");

        switch (dataRecorder.Condition)
        {
            case "V":

                break;

            case "T":

                break;

            case "VTEqual":

                TactileReferenceStim = "B11";
                VisualReferenceStim = "B11";

                BlockTactileStim = ShuffleList(RepeatList(tactileDisks, numberOfRepetitions));
                BlockVisualStim = BlockTactileStim;
                BlockTestingStimPosition = ShuffleList(RepeatList(new List<int> { 1, 2 }, numberOfRepetitions));

                break;

            case "V<T": // in terms of disk number
                break;


            case "V>T":

                break;

        }

    }



    // Update is called once per frame
    void Update()
    {

    }

    public void StartDataRecording() 
    {
  

 
    }
  



    public static List<T> RepeatList<T>(List<T> originalList, int repeatCount)
    {
        List<T> repeatedList = new List<T>();

        // Repeat the original list the specified number of times
        for (int i = 0; i < repeatCount; i++)
        {
            repeatedList.AddRange(originalList);
        }
        return repeatedList;
    }



    public static List<T> ShuffleList<T>(List<T> listToShuffle)
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
  
