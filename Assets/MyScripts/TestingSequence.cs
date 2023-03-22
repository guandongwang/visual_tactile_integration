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

    DataRecorder dataRecorder;

    // Start is called before the first frame update
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

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && onBlockStart != null)
        {
            onBlockStart.Invoke();
            StartCoroutine(ExperimentBlock());
        }

    }


    IEnumerator ExperimentBlock()
    {
        Debug.Log("Block " + BlockCount + " Start");

        int TrialCount = 1;

        foreach (TrialDataEntry entry in dataRecorder.trialData)
        {
            Debug.Log("Trial " + TrialCount + " Start");
            entry.TrialInfoData.CurrentTrialNumber = TrialCount;
            TrialCount++;
            yield return new WaitForSeconds(.1f);
            Debug.Log(entry.GetValues());

        }


    }

}
