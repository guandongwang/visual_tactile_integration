using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainSequence : MonoBehaviour
{

    public UnityEvent onBlockStart;
    public UnityEvent onBlockFinish;

    FrameDataRecorder frameDataRecorder;
    public static bool blockIsRunning;


    void Start()
    {
        frameDataRecorder = GetComponent<FrameDataRecorder>();

        if (onBlockStart == null)
        {
            onBlockStart = new UnityEvent();
        }

        if (onBlockFinish == null)
        {
            onBlockFinish = new UnityEvent();
        }


        onBlockStart.AddListener(beginTestingBlock);

        blockIsRunning = false;
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
    void beginTestingBlock()
    {
        Debug.Log("start");
        StartCoroutine(experimentBlock());

    }


    IEnumerator experimentBlock()
    {
        Debug.Log("start coroutine");

        /*    for (frameDataRecorder.trialCount = 1; frameDataRecorder.trialCount < frameDataRecorder.stimulusSequence.Count + 1; frameDataRecorder.trialCount++)
            {
                Debug.Log(frameDataRecorder.stimulusSequence[frameDataRecorder.trialCount - 1]);
                yield return new WaitForSeconds(.5f);
            }*/
        frameDataRecorder.trialCount = 0;

        foreach (int stim in frameDataRecorder.stimulusSequence)
        {
            frameDataRecorder.trialCount += 1;
            yield return new WaitForSeconds(.5f); 
        }
        blockIsRunning = false;
        onBlockFinish.Invoke();
        Debug.Log("finish");
    }
}
