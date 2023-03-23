using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;

public class TrialDataEntry
{
    public int ID { get; set; }
    public string Initial { get; set; }    
    public int Age { get; set; }
    public string Gender { get; set; }
    public string Condition { get; set; }
    

  
    public int CurrentTrialNumber { get; set; }
    public int TestingStimPosition { get; set; }
    public string S1Vision { get; set; }
    public string S1Touch { get; set; }
    public string S2Vision { get; set; }
    public string S2Touch { get; set; }

        
    public int CurrentResponse { get; set; }
    public bool IsTestingStimulusHigherFreq { get; set; }
    public float TrialDuration { get; set; }
    public float ResponseTime { get; set; }


    public float TrialStartTime { get; set; }
    public float S1Onset { get; set; }
    public float S1Offset { get; set; }
    public float S2Onset { get; set; }
    public float S2Offset { get; set; }
    public float ResponseMade { get; set; }
    


    //constructer declaration
    public TrialDataEntry(int id, String initial, int age, string gender, string condition) 
                
    {

        ID = id;
        Initial = initial;
        Age = age;
        Gender = gender;
        Condition = condition;

    }

    public string GetValues()
    {
        return string.Join(", ", typeof(TrialDataEntry).GetProperties().Select(p => p.GetValue(this)));

    }

    public string GetHeaders()
    {

        return string.Join(", ", typeof(TrialDataEntry).GetProperties().Select(p => p.Name));
  
    }


 

      
}
