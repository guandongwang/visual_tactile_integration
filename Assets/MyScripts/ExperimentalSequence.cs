using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentalSequence : MonoBehaviour
{
    StimulusGeneration stimulusGeneration;

    public int BlockNo;
    public int TrialNo;
    // Start is called before the first frame update
    void Start()
    {
        stimulusGeneration = GetComponent<StimulusGeneration>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator ExperimentBlock()
    {
        Debug.Log("Block " + BlockNo + " Start" );

    }

}
