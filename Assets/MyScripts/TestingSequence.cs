using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestingSequence : MonoBehaviour
{

    public int BlockCount;
    public int TrialCount;

    public UnityEvent onBlockStart;
    public UnityEvent onBlockFinish;

    public UnityEvent onTrialStart;
    public UnityEvent onTrialFinish;

    // Start is called before the first frame update
    void Start()
    {
        

        if (onBlockStart == null)
        {
            onBlockStart = new UnityEvent();
        }

        if (onBlockFinish == null)
        {
            onBlockFinish = new UnityEvent();
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && onBlockStart != null)
        {
  
            onBlockStart.Invoke();

        }

     /*   IEnumerator ExperimentBlock()
        {
            Debug.Log("Block " + BlockCount + " Start");

            trialData = stimulusGeneration.TrialDataFileSetup();

            
                }*/

    }

}
