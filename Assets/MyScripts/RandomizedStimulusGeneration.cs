using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using System.Linq;

public class RandomizedStimulusGeneration : MonoBehaviour
{
    public List<List<Stimulus>> SessionStimulusCollection;


    // Start is called before the first frame update
    void Start()
    {
        SessionStimulusCollection = CreateSessionStimulus();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    SessionStimulusCollection = CreateSessionStimulus();
        //    foreach (List<Stimulus> blockStim in SessionStimulusCollection)
        //    {
        //        foreach (Stimulus stim in blockStim)
        //        {
        //            Debug.Log(stim.GetValues());
        //        }
        //    }


        //}
    }

    List<List<Stimulus>> CreateSessionStimulus()
    {
        List<string > tactileDisks = new List<string>() { "B3", "B7", "B11", "B15", "B19" };
        List<string> visualDisk = new List<string>() { "B3", "B7", "B11", "B15", "B19" };
        List<string> visualDiskLow = new List<string>() { "B1", "B3", "B7", "B11", "B15"};
        List<string> visualDiskHigh = new List<string>() { "B3", "B7", "B11", "B15", "B19", "B21" };
        string referenceDisk = "B11";

        List<Stimulus> stimulusCollection = new List<Stimulus>();

        //visual ref/test
        for (int i = 0; i <5; i++)
        {
            Stimulus stimulus = new Stimulus();
            stimulus.Condition = "Vision";
            stimulus.StimPairIndex = i;
            stimulus.ReferenceLocation = 1;
            stimulus.S1Vision = referenceDisk;
            stimulus.S2Vision = visualDisk[i];
            stimulus.S1Touch = "dummy";
            stimulus.S2Touch = "dummy";

            stimulusCollection.Add(stimulus);
        }
        //visual test/ref
        for (int i = 0; i < 5; i++)
        {
            Stimulus stimulus = new Stimulus();
            stimulus.Condition = "Vision";
            stimulus.StimPairIndex = i + 5;
            stimulus.ReferenceLocation = 2;
            stimulus.S1Vision = visualDisk[i];
            stimulus.S2Vision = referenceDisk;
            stimulus.S1Touch = "dummy";
            stimulus.S2Touch = "dummy";
            stimulusCollection.Add(stimulus);
        }

        //Touch ref/test
        for (int i = 0; i < 5; i++)
        {
            Stimulus stimulus = new Stimulus();
            stimulus.Condition = "Touch";
            stimulus.StimPairIndex = i;
            stimulus.S1Vision = "dummy";
            stimulus.S2Vision = "dummy";
            stimulus.S1Touch = referenceDisk;
            stimulus.S2Touch = tactileDisks[i];

            stimulusCollection.Add(stimulus);
        }

        //Touch test/ref
        for (int i = 0; i < 5; i++)
        {
            Stimulus stimulus = new Stimulus();
            stimulus.Condition = "Touch";
            stimulus.StimPairIndex = i + 5;
            stimulus.S1Vision = "dummy";
            stimulus.S2Vision = "dummy";
            stimulus.S1Touch = tactileDisks[i];
            stimulus.S2Touch = referenceDisk;
            stimulusCollection.Add(stimulus);
        }

        //Combine
        for (int i = 0; i < 5; i++)
        {
            Stimulus stimulus = new Stimulus();
            stimulus.Condition = "Combine";
            stimulus.StimPairIndex = i;
            stimulus.S1Vision = referenceDisk;
            stimulus.S2Vision = visualDisk[i];
            stimulus.S1Touch = referenceDisk;
            stimulus.S2Touch = tactileDisks[i];

            stimulusCollection.Add(stimulus);
        }

        for (int i = 0; i < 5; i++)
        {
            Stimulus stimulus = new Stimulus();
            stimulus.Condition = "Combine";
            stimulus.StimPairIndex = i + 5;
            stimulus.S1Vision = visualDisk[i];
            stimulus.S2Vision = referenceDisk;
            stimulus.S1Touch = tactileDisks[i];
            stimulus.S2Touch = referenceDisk;
            stimulusCollection.Add(stimulus);
        }

        //VLowerFreqThanT
        for (int i = 0; i < 5; i++)
        {
            Stimulus stimulus = new Stimulus();
            stimulus.Condition = "VLowerFreqThanT";
            stimulus.StimPairIndex = i;
            stimulus.S1Vision = referenceDisk;
            stimulus.S2Vision = visualDiskLow[i];
            stimulus.S1Touch = referenceDisk;
            stimulus.S2Touch = tactileDisks[i];

            stimulusCollection.Add(stimulus);
        }

        for (int i = 0; i < 5; i++)
        {
            Stimulus stimulus = new Stimulus();
            stimulus.Condition = "VLowerFreqThanT";
            stimulus.StimPairIndex = i + 5;
            stimulus.S1Vision = visualDiskLow[i];
            stimulus.S2Vision = referenceDisk;
            stimulus.S1Touch = tactileDisks[i];
            stimulus.S2Touch = referenceDisk;
            stimulusCollection.Add(stimulus);
        }
        //VHigherFreqThanT
        for (int i = 0; i < 5; i++)
        {
            Stimulus stimulus = new Stimulus();
            stimulus.Condition = "VHigherFreqThanT";
            stimulus.StimPairIndex = i ;
            stimulus.S1Vision = referenceDisk;
            stimulus.S2Vision = visualDiskHigh[i];
            stimulus.S1Touch = referenceDisk;
            stimulus.S2Touch = tactileDisks[i];

            stimulusCollection.Add(stimulus);
        }

        
        for (int i = 0; i < 5; i++)
        {
            Stimulus stimulus = new Stimulus();
            stimulus.Condition = "VHigherFreqThanT";
            stimulus.StimPairIndex = i + 5;
            stimulus.S1Vision = visualDiskHigh[i];
            stimulus.S2Vision = referenceDisk;
            stimulus.S1Touch = tactileDisks[i];
            stimulus.S2Touch = referenceDisk;
            stimulusCollection.Add(stimulus);
        }

        //repeat 50 stim 10 times, total 500 trials
        //List<Stimulus> randomizedStim = ShuffleList(RepeatList(stimulusCollection, 10));

        List<Stimulus> randomizedStim = RepeatList(stimulusCollection, 10);
        List<List<Stimulus>> SessionStimulusCollection = new List<List<Stimulus>>();

        int startIndex = 0;
        int blockTrialNumber = 50;
        for (int i = 0; i < 10; i++)
        {

            // Create a new smaller list and add the range of items to it
            List<Stimulus> blockStim = randomizedStim.GetRange(startIndex, blockTrialNumber);
            SessionStimulusCollection.Add(blockStim);

            // Update the start index for the next iteration
            startIndex += blockTrialNumber;
        }
        return SessionStimulusCollection;

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


