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
        public string Name { get; set; }
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
    public TrialDataEntry(int ID, String Name, int Age, string Gender, string Condition) 
                
    {
        SessionInfoData = new SessionInfo();
            
        SessionInfoData.ID = ID;
        SessionInfoData.Name = Name;
        SessionInfoData.Age = Age;
        SessionInfoData.Gender = Gender;
        SessionInfoData.Condition = Condition;

    }

    public override string ToString()
    {
        string sessionInfoValues = string.Join(", ", typeof(SessionInfo).GetProperties().Select(p => p.GetValue(SessionInfoData)));
        string trialInfoValues = string.Join(", ", typeof(TrialInfo).GetProperties().Select(p => p.GetValue(TrialInfoData)));
        return $"{sessionInfoValues}, {trialInfoValues}";
    }

    public string GetHeaders()
    {
        string sessionInfoProperties = string.Join(", ", typeof(SessionInfo).GetProperties().Select(p => p.Name));
        string trialInfoProperties = string.Join(", ", typeof(TrialInfo).GetProperties().Select(p => p.Name));
        return $"{sessionInfoProperties}, {trialInfoProperties}";
    }


    /*    public override string ToString()
        {
            string dataString = string.Empty;
            PropertyInfo[] properties = typeof(DataEntry).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var value = property.GetValue(this); // you should add a null check here before doing value.ToString as it will break on null
                dataString += value.ToString() + ",";
            }
            return dataString;
        }
    */
    /*public override string ToString()
    {
        var properties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                   .Where(p => p.CanRead && p.GetIndexParameters().Length == 0)
                                   .Where(p => p.PropertyType == typeof(string));

        var nonEmptyProperties = properties.Where(p => !string.IsNullOrEmpty((string)p.GetValue(this)));


        return string.Join(",", nonEmptyProperties.Select(p => p.GetValue(this)));
            
 *//*       return String.Join(",",
                            this.ID, this.Name, this.Age, this.Gender, this.Condition,
                            this.CurrentTrialNumber, this.CurrentResponse, this.IsTestingStimulusHigherFreq,
                            this.TrialStartTime, this.S1Onset, this.S1Offset, this.S2Onset, this.S2Offset, this.ResponseMade,
                            this.TrialDuration, this.ResponseTime,
                            this.testingStimPosition, this.S1Vision, this.S1Touch, this.S2Vision, this.S2Touch
                            );*//*
    }*/
    /*    public string GetHeader()
        {
            var properties = this.GetType().GetProperties();
            string header = "";
            foreach (var property in properties)
            { header += "," + property.Name; }

            header = header.Remove(0, 1);
            return header;
        }
    */

}
