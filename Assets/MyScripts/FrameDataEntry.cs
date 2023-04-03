using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Reflection;
using System.Linq;
using ViveSR.anipal.Eye;
public class FrameDataEntry
{
    public int ID { get; set; }
    public string Initial { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }
    public string Condition { get; set; }



    public int Frame { get; set; }
    public float Time { get; set; }

    //Events
    public string TouchWheelMessage { get; set; }
    public string EventsLog { get; set; }

    public float HeadPositionX { get; set; }
    public float HeadPositionY { get; set; }
    public float HeadPositionZ { get; set; }

    public float HeadRotationX { get; set; }
    public float HeadRotationY { get; set; }
    public float HeadRotationZ { get; set; }

    public float TrackerPositionX { get; set; }
    public float TrackerPositionY { get; set; }
    public float TrackerPositionZ { get; set; }

    public float TrackerRotationX { get; set; }
    public float TrackerRotationY { get; set; }
    public float TrackerRotationZ { get; set; }

    public float VectGazeOriginX { get; set; }
    public float VectGazeOriginY { get; set; }
    public float VectGazeOriginZ { get; set; }

    public float VectGazeDirectionX { get; set; }
    public float VectGazeDirectionY { get; set; }
    public float VectGazeDirectionZ { get; set; }


    //constructer declaration
    public FrameDataEntry(int id, String initial, int age, string gender, string condition)

    {

        ID = id;
        Initial = initial;
        Age = age;
        Gender = gender;
        Condition = condition;

    }

    public string GetValues()
    {
        return string.Join(", ", typeof(FrameDataEntry).GetProperties().Select(p => p.GetValue(this)));

    }

    public string GetHeaders()
    {

        return string.Join(", ", typeof(FrameDataEntry).GetProperties().Select(p => p.Name));

    }





}
