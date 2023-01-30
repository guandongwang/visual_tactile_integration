using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*using Valve.VR;*/
using System.Text;
using System.Data;
using System.IO;
using UnityEngine.Events;
using UnityEngine.UI;

public class FrameDataRecorder : MonoBehaviour
{
    public List<int> stimulusSequence;

    public static DataTable blockDataTable = new DataTable();


    //Subject Info
    public int id;
    public string initial;
    public int age;
    public string gender;


    //timing info
    public int frameCount;
    public float timeElapsed;
    public int trialCount;

    public string folder;
    public string fileName;

   /* public bool blockIsRunning;*/

    MainSequence mainSequence;
    /*public bool blockIsCompleted;*/
    // Start is called before the first frame update
    void Start()
    {
        mainSequence = GetComponent<MainSequence>();
        mainSequence.onBlockStart.AddListener(startRecording);
        mainSequence.onBlockFinish.AddListener(finishRecording);


        stimulusSequence = GenerateStimulusSequence(5, 10);
        Debug.Log(string.Join(", ", stimulusSequence.ToArray()));

        blockDataTable.Columns.Add("ID", typeof(string));
        blockDataTable.Columns.Add("Name", typeof(string));
        blockDataTable.Columns.Add("Age", typeof(int));
        blockDataTable.Columns.Add("Gender", typeof(string));

        blockDataTable.Columns.Add("Frame", typeof(int));
        blockDataTable.Columns.Add("Time Elapsed", typeof(float));
        blockDataTable.Columns.Add("Current Trial Number", typeof(int));


    }

    // Update is called once per frame
    void Update()
    {
        if (MainSequence.blockIsRunning)
        {
            timeElapsed += Time.deltaTime;
            frameCount += 1;
            blockDataTable.Rows.Add(id, initial, age, gender, frameCount, timeElapsed, trialCount);
        }
    }

    void startRecording()
    {
        timeElapsed = 0;
        frameCount = 0;
        Debug.Log("start recording");
        fileName =id + "_" + initial + "_" + System.DateTime.Now.ToString("yyyy_MM_dd_(HH.mm.ss)") + ".csv";
    }

    void finishRecording()
    {
        folder = @"C:/Users/gwan5836/OneDrive - The University of Sydney (Staff)/2023/vr texture integration/raw data/" + id + "_" + initial;

        if (!System.IO.Directory.Exists(folder))
        {
            System.IO.Directory.CreateDirectory(folder);
        }
       
        string fullPath = folder + '/' + fileName;
        SaveToCSV(blockDataTable, fullPath);
        blockDataTable.Rows.Clear();


        Debug.Log("Save Data");
    }


    public List<int> GenerateStimulusSequence(int numberOfDisks, int numberOfRepetitions)
    {
        List<int> sequence = new List<int>();
        for (int i = 1; i <= numberOfDisks; i++)
        {
            for (int j = 0; j < numberOfRepetitions; j++)
            {
                sequence.Add(i);
            }
        }
        Shuffle(sequence);
        return sequence;
    }

    void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public static void SaveToCSV(DataTable dataTable, string filePath)
    {
        StringBuilder fileContent = new StringBuilder();

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
