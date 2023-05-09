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
    GameObject tracker;
    Camera mainCamera;

    DataRecorder dataRecorder;
    EyeTracking eyeTracking;
    TestingSequence testingSequence;
    InfoInspector infoInspector;

    public string EventLog;

    public List<FrameDataEntry> FrameData;

    private string prevEvent;
    private string prevMessage;


    void OnEnable()
    {

        EventManager.StartListening("OnBlockFinished", SaveToCSV);
    }

    void OnDisable()
    {
        EventManager.StopListening("OnBlockFinished", SaveToCSV);
    }



    // Start is called before the first frame update
    void Start()
    {
        GameObject cameraRig = GameObject.Find("ViveCameraRig");
        mainCamera = cameraRig.GetComponentInChildren<Camera>();


        /*_device = GameObject.Find("Device");*/
        /*_tracker = _device.transform.parent.gameObject;*/
        tracker = GameObject.Find("Tracker1");

        dataRecorder = GetComponent<DataRecorder>();
        testingSequence = GetComponent<TestingSequence>();

        GameObject srAnipal = GameObject.Find("SRanipal");
        eyeTracking = srAnipal.GetComponent<EyeTracking>();

        infoInspector = GetComponent<InfoInspector>();


        FrameData = new List<FrameDataEntry>();
 
        prevEvent = "";
        prevMessage = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (infoInspector.IsBlockRunning)
        {
            FrameDataEntry frameDataEntry = 
            new FrameDataEntry(infoInspector.id, infoInspector.initial, infoInspector.age, 
            infoInspector.gender.ToString(), infoInspector.condition.ToString());

            frameDataEntry.Frame = Time.frameCount;
            frameDataEntry.Time = Time.time;
            frameDataEntry.TrialNumber = infoInspector.CurrentTrial;

            if (infoInspector.TouchWheelMessage == prevMessage)
            {
                frameDataEntry.TouchWheelMessage = "";
            }
            else
            {
                prevMessage = infoInspector.TouchWheelMessage;
                frameDataEntry.TouchWheelMessage = prevMessage;
            }

            if (infoInspector.CurrentEvent == prevEvent)
            {
                frameDataEntry.EventsLog = "";
            }
            else
            {
                prevEvent = infoInspector.CurrentEvent;
                frameDataEntry.EventsLog = prevEvent;
            }

            //Head
            frameDataEntry.HeadPositionX = mainCamera.transform.position.x;
            frameDataEntry.HeadPositionY = mainCamera.transform.position.y;
            frameDataEntry.HeadPositionZ = mainCamera.transform.position.z;

            frameDataEntry.HeadRotationX = mainCamera.transform.eulerAngles.x;
            frameDataEntry.HeadRotationY = mainCamera.transform.eulerAngles.y;
            frameDataEntry.HeadRotationZ = mainCamera.transform.eulerAngles.z;

            //Tracker
            frameDataEntry.TrackerPositionX = tracker.transform.position.x;
            frameDataEntry.TrackerPositionY = tracker.transform.position.y;
            frameDataEntry.TrackerPositionZ = tracker.transform.position.z;

            frameDataEntry.TrackerRotationX = tracker.transform.eulerAngles.x;
            frameDataEntry.TrackerRotationY = tracker.transform.eulerAngles.y;
            frameDataEntry.TrackerRotationZ = tracker.transform.eulerAngles.z;

            frameDataEntry.VectGazeOriginX = eyeTracking.VectGazeOrigin.x;
            frameDataEntry.VectGazeOriginY = eyeTracking.VectGazeOrigin.y;
            frameDataEntry.VectGazeOriginZ = eyeTracking.VectGazeOrigin.z;

            frameDataEntry.VectGazeDirectionX = eyeTracking.VectGazeDirection.x;
            frameDataEntry.VectGazeDirectionY = eyeTracking.VectGazeDirection.y;
            frameDataEntry.VectGazeDirectionZ = eyeTracking.VectGazeDirection.z;

            frameDataEntry.EyeOpennessLeft = eyeTracking.EyeOpennessLeft;
            frameDataEntry.EyeOpennessRight= eyeTracking.EyeOpennessRight;
            FrameData.Add(frameDataEntry);
        }
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

        FrameData = new List<FrameDataEntry>();
    }

}
