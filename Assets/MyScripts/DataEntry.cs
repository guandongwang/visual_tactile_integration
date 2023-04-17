using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;
using ViveSR.anipal.Eye;

public class DataEntry
{
    public int ID { get; set; }
    public string Initial { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }
    public string Condition { get; set; }

    public DataEntry(int id, String initial, int age, string gender, string condition)

    {

        ID = id;
        Initial = initial;
        Age = age;
        Gender = gender;
        Condition = condition;

    }

    public string GetValues()
    {
        return string.Join(", ", typeof(DataEntry).GetProperties().Select(p => p.GetValue(this)));

    }

    public string GetHeaders()
    {

        return string.Join(", ", typeof(DataEntry).GetProperties().Select(p => p.Name));

    }
}
