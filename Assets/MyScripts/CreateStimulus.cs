using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using System.Linq;

public class CreateStimulus : MonoBehaviour
{
    public List<List<Stimulus>> SessionStimulusCollection;


    // Start is called before the first frame update
    void Start()
    {
        SessionStimulusCollection = CreateSessionStimulus();
            foreach (List<Stimulus> blockStim in SessionStimulusCollection)
        {
            foreach (Stimulus stim in blockStim)
            {
                Debug.Log(stim.GetValues());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    List<List<Stimulus>> CreateSessionStimulus()
    {
        List<string> conditionList = new List<string>() { "Vision", "Touch", "Combine", "VLowerFreqThanT", "VHigherFreqThanT" };
        string referenceDisk = "B11";

        List<string> tactileDisks = new List<string>() { "B3", "B7", "B11", "B15", "B19" };
        List<string> visualDisk = new List<string>() { "B3", "B7", "B11", "B15", "B19" };
        List<string> visualDiskLow = new List<string>() { "B1", "B3", "B7", "B11", "B15" };
        List<string> visualDiskHigh = new List<string>() { "B7", "B11", "B15", "B19", "B21" };

        List<Stimulus> stimulusCollection = new List<Stimulus>();

        foreach (string condition in conditionList)
        {
            for (int i = 0; i < 10; i++)
            {
                Stimulus stimulus = new Stimulus();
                stimulus.Condition = condition;
                stimulus.StimPairIndex = i;

                if (i < 5)
                {
                    stimulus.ReferenceLocation = 0;

                    switch (condition)
                    {
                        case ("Vision"):
                            stimulus.S1Vision = referenceDisk;
                            stimulus.S1Touch = "dummy";
                            stimulus.S2Vision = visualDisk[i];
                            stimulus.S2Touch = "dummy";
                            break;
                        case ("Touch"):
                            stimulus.S1Vision = "dummy";
                            stimulus.S1Touch = referenceDisk;
                            stimulus.S2Vision = "dummy";
                            stimulus.S2Touch = tactileDisks[i];
                            break;
                        case ("Combine"):
                            stimulus.S1Vision = referenceDisk;
                            stimulus.S1Touch = referenceDisk;
                            stimulus.S2Vision = visualDisk[i];
                            stimulus.S2Touch = tactileDisks[i];
                            break;
                        case ("VLowerFreqThanT"):
                            stimulus.S1Vision = referenceDisk;
                            stimulus.S1Touch = referenceDisk;
                            stimulus.S2Vision = visualDiskLow[i];
                            stimulus.S2Touch = tactileDisks[i];
                            break;
                        case ("VHigherFreqThanT"):
                            stimulus.S1Vision = referenceDisk;
                            stimulus.S1Touch = referenceDisk;
                            stimulus.S2Vision = visualDiskHigh[i];
                            stimulus.S2Touch = tactileDisks[i];
                            break;

                    }
       
                }
                else
                {
                    stimulus.ReferenceLocation = 1;

                    switch (condition)
                    {
                        case ("Vision"):
                            stimulus.S1Vision = visualDisk[i-5];
                            
                            stimulus.S1Touch = "dummy";
                            stimulus.S2Vision = referenceDisk;
                            stimulus.S2Touch = "dummy";
                            break;
                        case ("Touch"):
                            stimulus.S1Vision = "dummy";
                            
                            stimulus.S1Touch = tactileDisks[i-5];
                            stimulus.S2Vision = "dummy";
                            stimulus.S2Touch = referenceDisk;
                            break;
                        case ("Combine"):
                            stimulus.S1Vision = visualDisk[i - 5];
                            stimulus.S1Touch = tactileDisks[i - 5];
                            stimulus.S2Vision = referenceDisk;
                            stimulus.S2Touch = referenceDisk;
                            break;
                        case ("VLowerFreqThanT"):
                            stimulus.S1Vision = visualDiskLow[i - 5];
                           
                            stimulus.S1Touch = tactileDisks[i - 5];
                            stimulus.S2Vision = referenceDisk;
                            stimulus.S2Touch = referenceDisk;
                            break;
                        case ("VHigherFreqThanT"):
                            stimulus.S1Vision = visualDiskHigh[i - 5];
                            stimulus.S1Touch = tactileDisks[i - 5];
                            stimulus.S2Vision = referenceDisk;
                            stimulus.S2Touch = referenceDisk;
                            break;

                    }

                }

                switch (stimulus.StimPairIndex)
                {
                    case 0:
                    case 1:
                    case 8:
                    case 9:
                        stimulus.TargetResponse = "D";
                        break;
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                        stimulus.TargetResponse = "U";
                        break;
                    case 2:
                    case 7:
                        stimulus.TargetResponse = "DU";
                        break;
                }


                stimulusCollection.Add(stimulus);
            }
        }

        //repeat 50 stim 10 times, total 500 trials
        List<Stimulus> randomizedStim = ShuffleList(RepeatList(stimulusCollection, 10));

        //List<Stimulus> randomizedStim = RepeatList(stimulusCollection, 10);
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
