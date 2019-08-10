using System;
using UnityEngine;
using System.Collections;

public class StepDetector
{
    #region Variables

    public static StepDetector singleton;

    public event EventHandler Step;

    public float delta;

    static Vector3 oldAccel;
    float speedToMove;

    float lowLimit = 0.005f;            // level to fall to the low state
    float highLimit = 0.1f;             // level to go to high state (and detect step)
    //int stepCount = 0;                  // step counter - counts when comp state goes high

    float highFilter = 10.0f;           // noise filter control - reduces frequencies above fHigh
    float currentAccel;                 // noise filter
    float lowFilter = 0.1f;             // average gravity filter control - time constant about 1/fLow
    float avgAccel;                     // average gravity filter
    enum StepState { high, low }
    StepState stepState;

    #endregion

    public StepDetector()
    {
        avgAccel = Input.acceleration.magnitude; // initialize avg filter
    }
    

    public void Update()
    {
        if (SensorChange()) { OnSensorChanged(); }
    }

    public void LateUpdate()
    {
        oldAccel = Input.acceleration;
    }

    
    public static bool SensorChange()
    {
        if (oldAccel != Input.acceleration) return true;

        return false;
    }

    void OnSensorChanged()
   {
       // filter input.acceleration using Lerp
       currentAccel = Mathf.Lerp(currentAccel, Input.acceleration.magnitude, Time.deltaTime * highFilter);
       avgAccel = Mathf.Lerp(avgAccel, Input.acceleration.magnitude, Time.deltaTime * lowFilter);
       delta = currentAccel - avgAccel; // gets the acceleration pulses

       //Debug.Log(delta);

       if (stepState == StepState.low)
       {
           if (delta > highLimit)
           {
               stepState = StepState.high;
               Step(this, null);
           }
       }
       else
       {
           if (delta < lowLimit)
           {
               stepState = StepState.low;
           }
       }
   }

}
