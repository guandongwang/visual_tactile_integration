using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;



public class DataRecorder : MonoBehaviour
{
    TestingSequence testingSequence;
    InfoInspector infoInspector;
    CreateStimulus createStimulus;
    EyeTracking eyeTracking;

    GameObject tracker;
    Camera mainCamera;

    public List<FrameDataEntry> FrameData;

   



    // Start is called before the first frame update
    void Start()
    {
        testingSequence = GetComponent<TestingSequence>();
        infoInspector = GetComponent<InfoInspector>();

        GameObject cameraRig = GameObject.Find("ViveCameraRig");
        mainCamera = cameraRig.GetComponentInChildren<Camera>();


        /*_device = GameObject.Find("Device");*/
        /*_tracker = _device.transform.parent.gameObject;*/
        tracker = GameObject.Find("Tracker1");

       
        testingSequence = GetComponent<TestingSequence>();

        GameObject srAnipal = GameObject.Find("SRanipal");
        eyeTracking = srAnipal.GetComponent<EyeTracking>();

        infoInspector = GetComponent<InfoInspector>();

        FrameData = new List<FrameDataEntry>();
    }


    // Update is called once per frame
    void Update()
    {
        if (infoInspector.IsBlockRunning)
        {
            FrameDataEntry frameDataEntry =
            new FrameDataEntry(infoInspector.id, infoInspector.initial, infoInspector.age,
            infoInspector.gender.ToString(), infoInspector.CurrentBlock);

            frameDataEntry.Frame = Time.frameCount;
            frameDataEntry.Time = Time.time;
            frameDataEntry.TrialNumber = infoInspector.CurrentTrial;
            frameDataEntry.Conditon = infoInspector.CurrentTrialCondition;
            
            frameDataEntry.TouchWheelMessage = infoInspector.TouchWheelMessage;
            frameDataEntry.EventsLog = infoInspector.CurrentEvent;
        

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
            frameDataEntry.EyeOpennessRight = eyeTracking.EyeOpennessRight;

            FrameData.Add(frameDataEntry);
        }

    }
    
    public static void SaveToCSV<T>(List<T> data, string dataType) where T : DataEntry
    {
        {
            string baseFolder = "C:\\Users\\gwan5836\\OneDrive - The University of Sydney (Staff)\\2023\\vr texture integration\\raw data\\";

            string subjectFolder = data[0].ID + "_" + data[0].Initial;

            string fileName = dataType + "_" + data[0].ID + "_" + data[0].Initial + "_" + data[0].Block + "_" + System.DateTime.Now.ToString("yyyy_MM_dd_(HH.mm.ss)") + ".csv";

            string folderPath = Path.Combine(baseFolder, subjectFolder);

            if (!System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }


            string filePath = Path.Combine(folderPath, fileName);

            string headers = data[0].GetHeaders();

            StringBuilder dataFile = new();
            dataFile.AppendLine(headers);

            foreach (var entry in data)
            {
                dataFile.AppendLine(entry.GetValues());
            }

            File.WriteAllText(filePath, dataFile.ToString());
            Debug.Log(dataType + " Data File Saved");
        }
    }


}
