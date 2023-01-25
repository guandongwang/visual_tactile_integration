using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

public class ExperimentationSetup : MonoBehaviour
{
    public List<int> sequence;

    public DataTable blockDataTable = new DataTable();


    //Subject Info
    public int id;
    public string initial;
    public int age;
    public string gender;


    //timing info
    public int frameCount;
    public float timeElapsed;
    public int trialCount;

    public string fullPath;

    public bool blockIsRunning;
    public bool blockIsCompleted;
    // Start is called before the first frame update
    void Start()
    {
        sequence =  GenerateStimulusSequence(5, 10);
        Debug.Log(string.Join(", ", sequence.ToArray()));

        blockDataTable.Columns.Add("ID", typeof(string));
        blockDataTable.Columns.Add("Name", typeof(string));
        blockDataTable.Columns.Add("Age", typeof(int));
        blockDataTable.Columns.Add("Gender", typeof(string));

        blockDataTable.Columns.Add("Frame", typeof(string));
        blockDataTable.Columns.Add("Time Elapsed", typeof(int));
        blockDataTable.Columns.Add("Current Trial Number", typeof(string));
 

        string folder = @"D:/Guandong/data/" + 1 + "_" + "GW";
        string filename = System.DateTime.Now.ToString("yyyy_MM_dd_(HH.mm.ss)") + ".csv";

        string fullPath = folder + '/' + filename;

        if (!System.IO.Directory.Exists(folder))
        {
            System.IO.Directory.CreateDirectory(folder);
        }


        /*ExperimentationSetup.SaveToCSV(table, fullPath);*/
        blockIsRunning = false;
        blockIsCompleted = false;
    }

    // Update is called once per frame
    void Update()
    {
     
        if (Input.GetKeyDown(KeyCode.Space))
        { StartCoroutine(trial_loop()); }

        
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
