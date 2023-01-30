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
            onBlockStart = new UnityEvent();

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
        yield return new WaitForSeconds(2.5f);
        /*frameDataRecorder.blockIsRunning = false;*/
        /*FrameDataRecorder.SaveToCSV(FrameDataRecorder.blockDataTable, fi);*/
        blockIsRunning = false;
        onBlockFinish.Invoke();
        Debug.Log("finish");
    }
}
