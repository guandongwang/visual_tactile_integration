using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;

public class Stimulus 
{
    public string Condition { get; set; }
    public int StimPairNumber { get; set; }
    public string S1Vision { get; set; }
    public string S2Vision { get; set; }
    public string S1Touch { get; set; }
    public string S2Touch { get; set; }
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

