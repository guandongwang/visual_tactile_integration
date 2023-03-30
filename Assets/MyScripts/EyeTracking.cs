using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViveSR.anipal.Eye;
using System.Runtime.InteropServices;
using UnityEngine.Assertions;

public class EyeTracking : MonoBehaviour
{
    public Vector3 GazeOriginCombinedLocal,
                   GazeDirectionCombinedLocal,
                   vectGazeDirection,
                   vectGazeOrigin;  

    [SerializeField] private LineRenderer GazeRayRenderer;
    private static EyeData_v2 eyeData = new EyeData_v2();
    private bool eye_callback_registered = false;

    TestingSequence _testingSequnence;
    void Start()
    {
        GameObject _device = GameObject.Find("Device");
        _testingSequnence = _device.GetComponent<TestingSequence>();


        if (SRanipal_Eye_v2.LaunchEyeCalibration() == true)
        {
            _testingSequnence.IsEyeTrackingCalibrated = true;
        }
        else
        {
            /*  _testingSequnence.IsEyeTrackingCalibrated = true;*/
            _testingSequnence.IsEyeTrackingCalibrated = false;
        }


        if (!SRanipal_Eye_Framework.Instance.EnableEye)
        {
            enabled = false;
            return;
        }
        
    }

    private void Update()
    {

        if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING &&
                         SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.NOT_SUPPORT) return;

        if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == true && eye_callback_registered == false)
        {
            SRanipal_Eye_v2.WrapperRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
            eye_callback_registered = true;
        }
        else if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == false && eye_callback_registered == true)
        {
            SRanipal_Eye_v2.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
            eye_callback_registered = false;
        }

        Vector3 GazeOriginCombinedLocal, GazeDirectionCombinedLocal;

        //not needed if i use cam's way
        if (eye_callback_registered)
        {
            if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.COMBINE, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal, eyeData)) { }
            else if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.LEFT, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal, eyeData)) { }
            else if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.RIGHT, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal, eyeData)) { }
            else return;
        }
        else
        {
            if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.COMBINE, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal)) { }
            else if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.LEFT, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal)) { }
            else if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.RIGHT, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal)) { }
            else return;
        }

        vectGazeDirection = eyeData.verbose_data.combined.eye_data.gaze_direction_normalized;
        vectGazeOrigin = eyeData.verbose_data.combined.eye_data.gaze_origin_mm;
        Debug.Log("Origin: " + GazeOriginCombinedLocal + ", Direction: " + GazeDirectionCombinedLocal);
    }
    private void Release()
    {
        if (eye_callback_registered == true)
        {
            SRanipal_Eye_v2.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
            eye_callback_registered = false;
        }
    }
    private static void EyeCallback(ref EyeData_v2 eye_data)
    {
        eyeData = eye_data;
       
    }
}




