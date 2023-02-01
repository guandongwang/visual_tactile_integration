using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*using Valve.VR;*/
using System.Text;
using System.Data;
using System.IO;
using UnityEngine.Events;
using UnityEngine.UI;

public class DataRecorder : MonoBehaviour
{
    public DataTable frameDataTable = new();
    public DataTable trialDataTable = new();

    public DataRow newRow;


    //Subject Info
    public int id;
    public string initial;
    public int age;
    public string gender;

    //timing info
    public int frameCount;
    public float timeElapsed;


    //Testing Info
    public int currTrialCount;
    public int currDiskNo;
    public string currResponse;
    

    public string folder;
    public string frameDataFileName;
    public string trialDataFileName;


    MainSequence mainSequence;
  
    // Start is called before the first frame update
    void Start()
    {
        mainSequence = GetComponent<MainSequence>();
        mainSequence.onBlockStart.AddListener(startRecording);
        mainSequence.onBlockFinish.AddListener(finishRecording);

        frameDataTable.Columns.Add("ID", typeof(string));
        frameDataTable.Columns.Add("Name", typeof(string));
        frameDataTable.Columns.Add("Age", typeof(int));
        frameDataTable.Columns.Add("Gender", typeof(string));

        frameDataTable.Columns.Add("Frame", typeof(int));
        frameDataTable.Columns.Add("Time Elapsed", typeof(float));

        frameDataTable.Columns.Add("Current Trial Number", typeof(int));
        frameDataTable.Columns.Add("Current Disk Number", typeof(int));
        frameDataTable.Columns.Add("Current Response", typeof(string));

        //Trial Data
        trialDataTable.Columns.Add("ID", typeof(string));
        trialDataTable.Columns.Add("Name", typeof(string));
        trialDataTable.Columns.Add("Age", typeof(int));
        trialDataTable.Columns.Add("Gender", typeof(string));

        trialDataTable.Columns.Add("Frame", typeof(int));
        trialDataTable.Columns.Add("Time Elapsed", typeof(float));

        trialDataTable.Columns.Add("Current Trial Number", typeof(int));
        trialDataTable.Columns.Add("First Stimulus", typeof(int));
        trialDataTable.Columns.Add("Second Stimulus", typeof(int));
        trialDataTable.Columns.Add("Current Response", typeof(string));


    }

    // Update is called once per frame
    void Update()
    {
        if (MainSequence.blockIsRunning)
        {
            timeElapsed += Time.deltaTime;
            frameCount += 1;

            frameDataTable.Rows.Add(id, initial, age, gender, frameCount, timeElapsed, currTrialCount, currDiskNo, currResponse);
        
        }
    }

    void startRecording()
    {
        timeElapsed = 0;
        frameCount = 0;
        Debug.Log("start recording");
        frameDataFileName ="frame_" + id + "_" + initial + "_" + System.DateTime.Now.ToString("yyyy_MM_dd_(HH.mm.ss)") + ".csv";
        trialDataFileName ="trial_" + id + "_" + initial + "_" + System.DateTime.Now.ToString("yyyy_MM_dd_(HH.mm.ss)") + ".csv";
    }

    void finishRecording()
    {
        folder = @"C:/Users/gwan5836/OneDrive - The University of Sydney (Staff)/2023/vr texture integration/raw data/" + id + "_" + initial;

        if (!System.IO.Directory.Exists(folder))
        {
            System.IO.Directory.CreateDirectory(folder);
        }
       
        SaveToCSV(frameDataTable, folder + '/' + frameDataFileName);
        SaveToCSV(trialDataTable, folder + '/' + trialDataFileName); ;
        frameDataTable.Rows.Clear();
        trialDataTable.Rows.Clear();


        Debug.Log("Save Data");
    }


    public static void SaveToCSV(DataTable dataTable, string filePath)
    {
        StringBuilder fileContent = new();

        // Add headers
        for (int i = 0; i < dataTable.Columns.Count; i++)
        {
            fileContent.Append(dataTable.Columns[i]);

            if (i < dataTable.Columns.Count - 1)
            {
                fileContent.Append(",");
            }
        }

        fileContent.AppendLine();

        // Add rows
        foreach (DataRow row in dataTable.Rows)
        {
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                fileContent.Append(row[i].ToString());

                if (i < dataTable.Columns.Count - 1)
                {
                    fileContent.Append(",");
                }
            }

            fileContent.AppendLine();
        }

        File.WriteAllText(filePath, fileContent.ToString());
    }

}
