using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class FrameDataRecorder : MonoBehaviour
{

    private struct Data
    {
        public int id;
        public string name;
        public int age;
        public string gender;
     
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Valve.VR.OpenVR.Chaperone.ForceBoundsVisible(true);
    }
}
