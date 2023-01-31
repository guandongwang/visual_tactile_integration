using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainSequence : MonoBehaviour
{

    public UnityEvent onBlockStart;
    public UnityEvent onBlockFinish;

    public UnityEvent onTrialStart;
    public UnityEvent onTrialFinish;

    DataRecorder dataRecorder;
    public static bool blockIsRunning;

    public List<int> stimulusSequence;

    public bool isResponseMade;
    void Start()
    {

        dataRecorder = GetComponent<DataRecorder>();

        if (onBlockStart == null)
        {
            onBlockStart = new UnityEvent();
        }

        if (onBlockFinish == null)
        {
            onBlockFinish = new UnityEvent();
        }

        //trial event
        if (onTrialStart == null)
        {
            onTrialStart = new UnityEvent();
        }

        if (onTrialFinish == null)
        {
            onTrialFinish = new UnityEvent();
        }

        onBlockStart.AddListener(BeginTestingBlock);

        blockIsRunning = false;


        stimulusSequence = GenerateStimulusSequence(5, 10);
        Debug.Log(string.Join(", ", stimulusSequence.ToArray()));

 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && onBlockStart != null && !blockIsRunning)
        {
            blockIsRunning = true;
            onBlockStart.Invoke();
            
        }
    }
    public List<int> GenerateStimulusSequence(int numberOfDisks, int numberOfRepetitions)
    {
        List<int> sequence = new List<int>();
        for (int i = 0; i < numberOfDisks; i++)
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

    void BeginTestingBlock()
    {
        Debug.Log("start");
 
        StartCoroutine(ExperimentBlock());

    }


    IEnumerator ExperimentBlock()
    {

        Debug.Log("start coroutine");

        dataRecorder.currTrialCount = 0;

        foreach (int stim in stimulusSequence)
        {
            Debug.Log("Trial: " + dataRecorder.currTrialCount);
            onTrialStart.Invoke();
            isResponseMade = false;

            dataRecorder.currTrialCount += 1;
            dataRecorder.currDiskNo = stim;

            
            while (!isResponseMade)
            {

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    Debug.Log("response Made");
                    isResponseMade = true;
                    dataRecorder.currResponse = "Left";
                    //add trial data when response is made
                    dataRecorder.trialDataTable.Rows.Add(dataRecorder.id, dataRecorder.initial, dataRecorder.age, dataRecorder.gender,
                    dataRecorder.frameCount, dataRecorder.timeElapsed,
                    dataRecorder.currTrialCount, dataRecorder.currDiskNo, dataRecorder.currResponse);
                    onTrialFinish.Invoke();
                }

                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    Debug.Log("response Made");
                    isResponseMade = true;
                    dataRecorder.currResponse = "Right";

                    dataRecorder.trialDataTable.Rows.Add(dataRecorder.id, dataRecorder.initial, dataRecorder.age, dataRecorder.gender,
                    dataRecorder.frameCount, dataRecorder.timeElapsed,
                    dataRecorder.currTrialCount, dataRecorder.currDiskNo, dataRecorder.currResponse);
                    onTrialFinish.Invoke();
                }

                


                yield return null;
            }
        }
        blockIsRunning = false;
        onBlockFinish.Invoke();
        Debug.Log("finish");
    }


}
