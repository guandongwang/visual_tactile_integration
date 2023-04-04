using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using HTC.UnityPlugin.Vive;
using System.IO;
using System.Text;

public class FrameDataRecorder : MonoBehaviour
{
    /*GameObject _device;*/
    GameObject _tracker;
    Camera _mainCamera;

    DataRecorder _dataRecorder;
    EyeTracking _eyeTracking;
    TestingSequence _testingSequence;

    public string EventLog;

    public List<FrameDataEntry> FrameData;

    public string CurrEvent;
    public string PrevEvent;


    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        CurrEvent = logString;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject cameraRig = GameObject.Find("ViveCameraRig");
        _mainCamera = cameraRig.GetComponentInChildren<Camera>();


        /*_device = GameObject.Find("Device");*/
        /*_tracker = _device.transform.parent.gameObject;*/
        _tracker = GameObject.Find("Tracker1");

        _dataRecorder = GetComponent<DataRecorder>();
        _testingSequence = GetComponent<TestingSequence>();

        GameObject srAnipal = GameObject.Find("SRanipal");
        _eyeTracking = srAnipal.GetComponent<EyeTracking>();


        FrameData = new List<FrameDataEntry>();

        _testingSequence.onBlockFinish.AddListener(SaveToCSV);

    }

    // Update is called once per frame
    void Update()
    {
        
        FrameDataEntry frameDataEntry = new FrameDataEntry(_dataRecorder.id, _dataRecorder.initial, _dataRecorder.age, _dataRecorder.gender, _dataRecorder.condition);

        frameDataEntry.Frame = Time.frameCount;
        frameDataEntry.Time = Time.time;

        frameDataEntry.TouchWheelMessage = "";


        /* _frameDataEntry.EventsLog = "";*/
        if (CurrEvent != PrevEvent)
        {
            PrevEvent = CurrEvent;
            frameDataEntry.EventsLog = CurrEvent;
        }
        else
        {
            frameDataEntry.EventsLog = "";
        }

        //Head
        frameDataEntry.HeadPositionX = _mainCamera.transform.position.x;
        frameDataEntry.HeadPositionY = _mainCamera.transform.position.y;
        frameDataEntry.HeadPositionZ = _mainCamera.transform.position.z;

        frameDataEntry.HeadRotationX = _mainCamera.transform.eulerAngles.x;
        frameDataEntry.HeadRotationY = _mainCamera.transform.eulerAngles.y;
        frameDataEntry.HeadRotationZ = _mainCamera.transform.eulerAngles.z;

        //Tracker
        frameDataEntry.TrackerPositionX = _tracker.transform.position.x;
        frameDataEntry.TrackerPositionY = _tracker.transform.position.y;
        frameDataEntry.TrackerPositionZ = _tracker.transform.position.z;

        frameDataEntry.VectGazeOriginX = _eyeTracking.VectGazeOrigin.x;
        frameDataEntry.VectGazeOriginY = _eyeTracking.VectGazeOrigin.y;
        frameDataEntry.VectGazeOriginZ = _eyeTracking.VectGazeOrigin.z;

        frameDataEntry.VectGazeDirectionX = _eyeTracking.VectGazeDirection.x;
        frameDataEntry.VectGazeDirectionY = _eyeTracking.VectGazeDirection.y;
        frameDataEntry.VectGazeDirectionZ = _eyeTracking.VectGazeDirection.z;

        FrameData.Add(frameDataEntry);
    }

    void SaveToCSV()
    {
        string folderPath = @"C:/Users/gwan5836/OneDrive - The University of Sydney (Staff)/2023/vr texture integration/raw data/" + FrameData[0].ID + "_" + FrameData[0].Initial;
        string frameDataFileName = "frame_" + FrameData[0].ID + "_" + FrameData[0].Initial + "_" + FrameData[0].Condition + "_" + System.DateTime.Now.ToString("yyyy_MM_dd_(HH.mm.ss)") + ".csv";

        if (!System.IO.Directory.Exists(folderPath))
        {
            System.IO.Directory.CreateDirectory(folderPath);
        }


        string filePath = folderPath + '/' + frameDataFileName;

        string headers = FrameData[0].GetHeaders();

        StringBuilder dataFile = new();
        dataFile.AppendLine(headers);

        foreach (FrameDataEntry entry in FrameData)
        {
            dataFile.AppendLine(entry.GetValues());
        }

        File.WriteAllText(filePath, dataFile.ToString());
        Debug.Log("Frame data saved");
    }
}
