using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using HTC.UnityPlugin.Vive;
using System.IO;
using System.Text;

public class FrameDataRecorder : MonoBehaviour
{
    GameObject cameraRig;
    GameObject device;
    GameObject tracker;
    Camera mainCamera;

    public string EventLog;

    DataRecorder dataRecorder;
    EyeTracking eyeTracking;
    TestingSequence testingSequence;

    public List<FrameDataEntry> FrameData;

    public string CurrEvent;
    public string PrevEvent;

    public string test;


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
        cameraRig = GameObject.Find("ViveCameraRig");
        mainCamera = cameraRig.GetComponentInChildren<Camera>();
        dataRecorder = GetComponent<DataRecorder>();
        testingSequence = GetComponent<TestingSequence>();

        device = GameObject.Find("Device");
        tracker = device.transform.parent.gameObject;

        GameObject srAnipal = GameObject.Find("SRanipal");
        eyeTracking = srAnipal.GetComponent<EyeTracking>();

        FrameData = new List<FrameDataEntry>();

        testingSequence.onBlockFinish.AddListener(SaveToCSV);

    }

    // Update is called once per frame
    void Update()
    {
        
        FrameDataEntry _frameDataEntry = new FrameDataEntry(dataRecorder.id, dataRecorder.initial, dataRecorder.age, dataRecorder.gender, dataRecorder.condition);

        _frameDataEntry.Frame = Time.frameCount;
        _frameDataEntry.Time = Time.time;

        _frameDataEntry.TouchWheelMessage = "";


        _frameDataEntry.EventsLog = "";
   /*     if (CurrEvent != PrevEvent)
        {
            PrevEvent = CurrEvent;
            _frameDataEntry.EventsLog = "\"" + CurrEvent + "\"";
        }
        else
        {
            _frameDataEntry.EventsLog = "";
        }
*/
        //Head
        _frameDataEntry.HeadPositionX = mainCamera.transform.position.x;
        _frameDataEntry.HeadPositionY = mainCamera.transform.position.y;
        _frameDataEntry.HeadPositionZ = mainCamera.transform.position.z;

        _frameDataEntry.HeadRotationX = mainCamera.transform.eulerAngles.x;
        _frameDataEntry.HeadRotationY = mainCamera.transform.eulerAngles.y;
        _frameDataEntry.HeadRotationZ = mainCamera.transform.eulerAngles.z;

        //Tracker
        _frameDataEntry.TrackerPositionX = tracker.transform.position.x;
        _frameDataEntry.TrackerPositionY = tracker.transform.position.y;
        _frameDataEntry.TrackerPositionZ = tracker.transform.position.z;

        _frameDataEntry.GazeOriginCombinedLocalX = eyeTracking.GazeOriginCombinedLocal.x;
        _frameDataEntry.GazeOriginCombinedLocalX = eyeTracking.GazeOriginCombinedLocal.y;
        _frameDataEntry.GazeOriginCombinedLocalX = eyeTracking.GazeOriginCombinedLocal.z;

        _frameDataEntry.GazeDirectionCombinedLocalX = eyeTracking.GazeDirectionCombinedLocal.x;
        _frameDataEntry.GazeDirectionCombinedLocalX = eyeTracking.GazeDirectionCombinedLocal.y;
        _frameDataEntry.GazeDirectionCombinedLocalX = eyeTracking.GazeDirectionCombinedLocal.z;

        _frameDataEntry.VectGazeOriginX = eyeTracking.vectGazeOrigin.x;
        _frameDataEntry.VectGazeOriginY = eyeTracking.vectGazeOrigin.y;
        _frameDataEntry.VectGazeOriginZ = eyeTracking.vectGazeOrigin.z;

        _frameDataEntry.VectGazeDirectionX = eyeTracking.vectGazeDirection.x;
        _frameDataEntry.VectGazeDirectionY = eyeTracking.vectGazeDirection.y;
        _frameDataEntry.VectGazeDirectionZ = eyeTracking.vectGazeDirection.z;

        FrameData.Add(_frameDataEntry);
    }

    void SaveToCSV()
    {
        string _folderPath = @"C:/Users/gwan5836/OneDrive - The University of Sydney (Staff)/2023/vr texture integration/raw data/" + FrameData[0].ID + "_" + FrameData[0].Initial;
        string _frameDataFileName = "frame_" + FrameData[0].ID + "_" + FrameData[0].Initial + "_" + FrameData[0].Condition + "_" + System.DateTime.Now.ToString("yyyy_MM_dd_(HH.mm.ss)") + ".csv";

        if (!System.IO.Directory.Exists(_folderPath))
        {
            System.IO.Directory.CreateDirectory(_folderPath);
        }


        string _filePath = _folderPath + '/' + _frameDataFileName;

        string headers = FrameData[0].GetHeaders();

        StringBuilder dataFile = new();
        dataFile.AppendLine(headers);

        foreach (FrameDataEntry entry in FrameData)
        {
            dataFile.AppendLine(entry.GetValues());
        }

        File.WriteAllText(_filePath, dataFile.ToString());
        Debug.Log("Frame Data Saved");
    }
}
