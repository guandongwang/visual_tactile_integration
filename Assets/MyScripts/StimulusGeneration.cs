using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using Random = System.Random;


public class StimulusGeneration : MonoBehaviour
{
    public TrialDataEntry[] trialData;
    public TrialDataEntry testDataEntry = new TrialDataEntry();

    public int numberOfStimulusPair;
    public int numberOfRepetitions;


    //Subject Info
    public int id;
    public string initial;
    public int age;
    public string gender;
    public string condition;


    // Start is called before the first frame update
    void Start()
    {
        numberOfStimulusPair = 5;
        numberOfRepetitions = 10;

        trialData = new TrialDataEntry[numberOfStimulusPair * numberOfRepetitions];

        int index = 0;

        foreach (TrialDataEntry trialDataEntry in trialData)
        {
            trialDataEntry.ID = id;
            trialDataEntry.Name = initial;
            trialDataEntry.Age = age;
            trialDataEntry.Gender = gender;
            trialDataEntry.Condition = condition;

            trialDataEntry.CurrentTrialNumber = index;
            index++;
        }

        switch (condition)
        {
            case "Vision":
                
                break;

            case "Touch":
               
                break;

            case "T = V":

                break;

            case "T = V + 1":

                break;

        }



        /*      testDataEntry.CurrentTrialNumber = 1;
                testDataEntry.CurrentResponse = 0;

                testDataEntry.TrialStartTime = 0f;
                testDataEntry.S1Onset = .1f;
                testDataEntry.S1Offset = .1f;
                testDataEntry.S2Onset = .1f;
                testDataEntry.S2Offset = .1f;
                testDataEntry.ResponseMade = .1f;

                testDataEntry.TrialDuration = .1f;
                testDataEntry.ResponseTime = .1f;

                testDataEntry.S1Vision = 1;
                testDataEntry.S1Touch = 2;
                testDataEntry.S2Vision = 3;
                testDataEntry.S2Touch = 4;*/

    }





    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log(testDataEntry.ToString());
            Debug.Log(testDataEntry.GetHeader());
        }
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
        public int S1Vision { get; set; }
        public int S1Touch { get; set; }
        public int S2Vision { get; set; }
        public int S2Touch { get; set; }

        public override string ToString()
        {
            return String.Join(",", 
                                this.ID, this.Name, this.Age, this.Gender, this.Condition,
                                this.CurrentTrialNumber, this.CurrentResponse, 
                                this.TrialStartTime, this.S1Onset, this.S1Offset, this.S2Onset, this.S2Offset, this.ResponseMade,
                                this.TrialDuration, this.ResponseTime, 
                                this.S1Vision, this.S1Touch, this.S2Vision, this.S2Touch
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
        List<T> repeatedList = new List<T>(originalList.Count * repeatCount);

        // Repeat the original list the specified number of times
        for (int i = 0; i < repeatCount; i++)
        {
            repeatedList.AddRange(originalList);
        }
        return repeatedList;
    }



    public static List<T> ShuffleList<T>(List<T> list)
    {
        Random rng = new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }

}
  
