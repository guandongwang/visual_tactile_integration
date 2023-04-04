using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViveSR.anipal.Eye;
using System.Runtime.InteropServices;
using UnityEngine.Assertions;

public class EyeTracking : MonoBehaviour
{
    public Vector3 VectGazeDirection,
                   VectGazeOrigin;  

    private LineRenderer GazeRayRenderer;
    private static EyeData_v2 eyeData = new EyeData_v2();
    private bool eye_callback_registered = false;

    TestingSequence _testingSequnence;

    [SerializeField] private bool tryEyeTrackingCalibrate = false;



    void Start()
    {
        GameObject _device = GameObject.Find("Device");
        _testingSequnence = _device.GetComponent<TestingSequence>();


        EyeTrackingCalibration();

        if (!SRanipal_Eye_Framework.Instance.EnableEye)
        {
            enabled = false;
            return;
        }
        
    }

    private void Update()
    {
        if (_testingSequnence.IsEyeTrackingCalibrated == false && tryEyeTrackingCalibrate == true)
        {
            tryEyeTrackingCalibrate = false;
            EyeTrackingCalibration();
        }

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

        VectGazeDirection = eyeData.verbose_data.combined.eye_data.gaze_direction_normalized;
        VectGazeOrigin = eyeData.verbose_data.combined.eye_data.gaze_origin_mm;
        Debug.Log("Origin: " + GazeOriginCombinedLocal + ", Direction: " + GazeDirectionCombinedLocal);
    }

    void EyeTrackingCalibration()
    {
        if (SRanipal_Eye_v2.LaunchEyeCalibration() == true)
        {
            _testingSequnence.IsEyeTrackingCalibrated = true;
            EventManager.TriggerEvent("EyeTrackingCalibrated", null);
        }
        else
        {
            _testingSequnence.IsEyeTrackingCalibrated = false;
        }

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




