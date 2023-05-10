using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;


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
        Type type = this.GetType();
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

        string[] values = properties.Select(p => p.GetValue(this)?.ToString() ?? "").ToArray();
        return string.Join(", ", values);
    }

    public string GetHeaders()
    {
        Type type = this.GetType();
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

        string[] headers = properties.Select(p => p.Name).ToArray();
        return string.Join(", ", headers);
    }
}
