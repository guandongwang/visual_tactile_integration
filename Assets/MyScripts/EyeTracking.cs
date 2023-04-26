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

    public float EyeOpennessLeft;
    public float EyeOpennessRight;

    private LineRenderer GazeRayRenderer;
    private static EyeData_v2 eyeData = new EyeData_v2();
    private bool eye_callback_registered = false;

    InfoInspector infoInspector;

    void Start()
    {

        GameObject script = GameObject.Find("Scripts");
        infoInspector = script.GetComponent<InfoInspector>();
       /* testingSequnence = script.GetComponent<TestingSequence>();*/


        if (!SRanipal_Eye_Framework.Instance.EnableEye)
        {
            enabled = false;
            return;
        }
        
    }

    private void Update()
    {
      
            if (infoInspector.TryEyeCalibration)
            {
            infoInspector.TryEyeCalibration = false;
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
            EyeOpennessLeft = eyeData.verbose_data.left.eye_openness;
            EyeOpennessRight = eyeData.verbose_data.right.eye_openness;
        /* Debug.Log("Origin: " + GazeOriginCombinedLocal + ", Direction: " + GazeDirectionCombinedLocal);*/
    }
   

    void EyeTrackingCalibration()
    {
        if(SRanipal_Eye_v2.LaunchEyeCalibration())
        {
        infoInspector.IsEyeTrackingCalibrated = true;
        infoInspector.CurrentEvent = "Eye tracking calibration succeed";
        }
         else
        {
            infoInspector.IsEyeTrackingCalibrated = false;
            infoInspector.CurrentEvent = "Eye tracking calibration failed";
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




