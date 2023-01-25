using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSequence : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(experimentBlock());
        }

    }

    IEnumerator experimentBlock()
    {
        yield return new WaitForSeconds(5f);
        FrameDataRecorder.blockIsRunning = false;
    }
}
