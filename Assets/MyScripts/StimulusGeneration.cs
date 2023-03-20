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
    
   

    /*public int numberOfStimulusPair;*/
    public int numberOfRepetitions;


    //Subject Info
    public int id;
    public string initial;
    public int age;
    public string gender;
    public string condition;

    public string tactileReferenceStim;
    public string visualReferenceStim;
    private List<string> _tacitleStimuli;
    private List<string> _visualStimuli;
    private List<int> _testingStimPosition;


    public Dictionary<string, string> tactileDisksDic;

    public List<string> tactileDisks;

    // Start is called before the first frame update
    void Start()
    {
        /*numberOfStimulusPair = 5;*/
        numberOfRepetitions = 10;

        tactileDisks = new List<string>() { "B7", "B9", "B11", "B13", "B15" };

        tactileDisksDic = new Dictionary<string, string>(5);
        tactileDisksDic.Add(tactileDisks[0], "A");
        tactileDisksDic.Add(tactileDisks[1], "B");
        tactileDisksDic.Add(tactileDisks[2], "C");
        tactileDisksDic.Add(tactileDisks[3], "D");
        tactileDisksDic.Add(tactileDisks[4], "E");


       

    }



    // Update is called once per frame
    void Update()
    {

    }

    public static List<TrialDataEntry> TrialDataFileSetup()
    { 
        List<TrialDataEntry> trialData;

        trialData = Enumerable.Repeat(new TrialDataEntry(), tactileDisks.Count * numberOfRepetitions).ToList(); ;

        int index = 1;

        foreach (TrialDataEntry entry in trialData)
        {
            entry.ID = id;
            entry.Name = initial;
            entry.Age = age;
            entry.Gender = gender;
            entry.Condition = condition;
            entry.CurrentTrialNumber = index;

            Debug.Log(entry.ToString());

            index++;
        }

        //create stimuli


        switch (condition)
        {
            case "V":

                break;

            case "T":

                break;

            case "VTEqual":

                tactileReferenceStim = "B11";
                visualReferenceStim = tactileReferenceStim;
                _tacitleStimuli = ShuffleList(RepeatList(tactileDisks, numberOfRepetitions));
                _visualStimuli = _tacitleStimuli;
                _testingStimPosition = ShuffleList(RepeatList(new List<int> { 1, 2 }, numberOfRepetitions));

                /*  for (int i = 0; i < testingStimPosition.Count; i++)
                  {

                      Debug.Log(testingStimPosition[i].ToString());
                  }*/
                break;

            case "V<T": // in terms of disk number
                break;


            case "V>T":

                break;

        }


        for (int i = 0; i < trialData.Count; i++)
        {
            trialData[i].testingStimPosition = _testingStimPosition[i];

            if (trialData[i].testingStimPosition == 1)
            {
                trialData[i].S1Touch = _tacitleStimuli[i];
                trialData[i].S1Vision = _visualStimuli[i];
                trialData[i].S2Touch = tactileReferenceStim;
                trialData[i].S2Vision = visualReferenceStim;
            }
            else if (trialData[i].testingStimPosition == 1)
            {
                trialData[i].S1Touch = tactileReferenceStim;
                trialData[i].S1Vision = visualReferenceStim;
                trialData[i].S2Touch = _tacitleStimuli[i];
                trialData[i].S2Vision = _visualStimuli[i];
            }
        }
        return trialData;
    }
    public class TrialDataEntry
    {
        //Session Info
        public int ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Condition { get; set; }


        public int CurrentTrialNumber { get; set; }
        public int CurrentResponse { get; set; }

        public bool IsTestingStimulusHigherFreq { get; set; }

        //Timing Info
        public float TrialStartTime { get; set; }
        public float S1Onset { get; set; }
        public float S1Offset { get; set; }
        public float S2Onset { get; set; }
        public float S2Offset { get; set; }
        public float ResponseMade { get; set; }

        //Trial Info
        public float TrialDuration { get; set; }
        public float ResponseTime { get; set; }

        //Stimulus Info, the int here refers to the B_ number

        public int testingStimPosition { get; set; }
        public string S1Vision { get; set; }
        public string S1Touch { get; set; }
        public string S2Vision { get; set; }
        public string S2Touch { get; set; }

        public override string ToString()
        {
            return String.Join(",", 
                                this.ID, this.Name, this.Age, this.Gender, this.Condition,
                                this.CurrentTrialNumber, this.CurrentResponse, this.IsTestingStimulusHigherFreq,
                                this.TrialStartTime, this.S1Onset, this.S1Offset, this.S2Onset, this.S2Offset, this.ResponseMade,
                                this.TrialDuration, this.ResponseTime, 
                                this.testingStimPosition, this.S1Vision, this.S1Touch, this.S2Vision, this.S2Touch
                                );
        }
        public string GetHeader()
        {
            var properties = this.GetType().GetProperties();
            string header = "";
            foreach (var property in properties)
            { header += "," + property.Name; }

            header = header.Remove(0,1);
            return header;
        }


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
  
