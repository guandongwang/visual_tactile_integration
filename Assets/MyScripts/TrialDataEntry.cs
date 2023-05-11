using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;

public class TrialDataEntry : DataEntry
{

    public int TrialNumber { get; set; }

    public string Condition { get; set; }
    public int StimPairIndex { get; set; }
    public int RefernceLocation { get; set; }
    public string S1Vision { get; set; }
    //public int S1VisionOri { get; set; }
    public string S1Touch { get; set; }
    public string S2Vision { get; set; }
    //public int S2VisionOri { get; set; }
    public string S2Touch { get; set; }


    //public bool IsTestingStimAtPos2 { get; set; }
    
        
    public string Response { get; set; }
    public string TargetResponse { get; set; }
    public bool RespResult { get; set; }

    public float TrialDuration { get; set; }
    public float ResponseTime { get; set; }


    public float TrialStartTime { get; set; }
    public float S1Onset { get; set; }
    public float S1Offset { get; set; }
    public float S2Onset { get; set; }
    public float S2Offset { get; set; }
    public float ResponseCued { get; set; }
    public float ResponseMade { get; set; }
    public float TrialEndTime { get; set; }



    public TrialDataEntry(int id, String initial, int age, string gender, string condition)
    : base(id, initial, age, gender, condition) { }

    public TrialDataEntry(DataEntry dataEntry)
      : base(dataEntry.ID, dataEntry.Initial, dataEntry.Age, dataEntry.Gender, dataEntry.Condition) { }

}
