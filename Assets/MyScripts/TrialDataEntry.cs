using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;

public class TrialDataEntry
{
    public SessionInfo SessionInfoData { get; set; }
    public TrialInfo TrialInfoData { get; set; }


    public class SessionInfo
    {
        public int ID { get; set; }
        public string Initial { get; set; }    
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Condition { get; set; }
    }

    public class TrialInfo
    {
        public int CurrentTrialNumber { get; set; }
        public int testingStimPosition { get; set; }
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
    }


    //constructer declaration
    public TrialDataEntry(int ID, String Initial, int Age, string Gender, string Condition) 
                
    {
        SessionInfoData = new SessionInfo();
        TrialInfoData = new TrialInfo();

        SessionInfoData.ID = ID;
        SessionInfoData.Initial = Initial;
        SessionInfoData.Age = Age;
        SessionInfoData.Gender = Gender;
        SessionInfoData.Condition = Condition;

    }

    public string GetValues()
    {
        string sessionInfoValues = string.Join(", ", typeof(SessionInfo).GetProperties().Select(p => p.GetValue(SessionInfoData)));
        string trialInfoValues = string.Join(", ", typeof(TrialInfo).GetProperties().Select(p => p.GetValue(TrialInfoData)));

        /*        PropertyInfo[] sessionInfoProperties = SessionInfoData.GetType().GetProperties();
                string sessionInfoValues = string.Join(",", sessionInfoProperties.Select(p => p.GetValue(SessionInfoData)).Where(v => v != null));

                PropertyInfo[] trialInfoProperties = TrialInfoData.GetType().GetProperties();
                string trialInfoValues = string.Join(",", trialInfoProperties.Select(p => p.GetValue(TrialInfoData)).Where(v => v != null));*/

        return $"{sessionInfoValues}, {trialInfoValues}";
    }

    public string GetHeaders()
    {

  

        string sessionInfoHeader = string.Join(", ", typeof(SessionInfo).GetProperties().Select(p => p.Name));
        string trialInfoHeader = string.Join(", ", typeof(TrialInfo).GetProperties().Select(p => p.Name));
        return $"{sessionInfoHeader}, {trialInfoHeader}";
    }


 

      
}
