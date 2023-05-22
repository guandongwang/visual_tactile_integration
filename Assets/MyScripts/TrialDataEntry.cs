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
    public int ReferenceLocation { get; set; }
    public string S1Vision { get; set; }
    //public int S1VisionOri { get; set; }
    public string S1Touch { get; set; }
    public int S1Steps { get; set; }
    public float S1Orientation { get; set; }
    public string S2Vision { get; set; }
    //public int S2VisionOri { get; set; }
    public string S2Touch { get; set; }
    public int S2Steps { get; set; }
    public float S2Orientation { get; set; }

    //public bool IsTestingStimAtPos2 { get; set; }


    public string Response { get; set; }
    public string TargetResponse { get; set; }
    public bool RespResult { get; set; }

    public float TrialDuration { get; set; }
    public float ResponseTime { get; set; }


    public float TrialStartTime { get; set; }

    public float S1Begin { get; set; }
    public float S1PresnetationBegin{ get; set; }
    public float S1End { get; set; }
    public float S1PresnetationEnd { get; set; }

    public float S2Begin { get; set; }
    public float S2PresnetationBegin { get; set; }
    public float S2End { get; set; }

    public float S2PresnetationEnd { get; set; }

    public float ResponseCued { get; set; }//same as s2 presentation end
    public float ResponseMade { get; set; }
    public float TrialEndTime { get; set; }



    public TrialDataEntry(int id, String initial, int age, string gender, int block)
    : base(id, initial, age, gender, block) { }

    public TrialDataEntry(DataEntry dataEntry)
      : base(dataEntry.ID, dataEntry.Initial, dataEntry.Age, dataEntry.Gender, dataEntry.Block) { }

}
