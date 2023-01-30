using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainSequence : MonoBehaviour
{
    UnityEvent blockStartEvent;
    
    /*public static bool blockIsRunning;*/
    // Start is called before the first frame update
  
    void Start()
    {
        if (blockStartEvent == null)
            blockStartEvent = new UnityEvent();

        blockStartEvent.AddListener(blockStart);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && blockStartEvent != null)
        {
            blockStartEvent.Invoke();
        }
    }
    void blockStart() 
    {
        FrameDataRecorder.blockIsRunning = true;
        Debug.Log("start");
        StartCoroutine(experimentBlock());
        
    }


    IEnumerator experimentBlock()
    {
        yield return new WaitForSeconds(2.5f);
        FrameDataRecorder.blockIsRunning = false;
        FrameDataRecorder.SaveToCSV(FrameDataRecorder.blockDataTable);
        Debug.Log("finish");
    }
}
