using System;
using UnityEngine;
using System.Collections;

public class Pedometer
{
    #region Variables

    public static Pedometer singleton;

    int _stepCount;
    public int stepCount
    {
        get { return _stepCount; }

        set { DebugOutput.LogError("Trying to set stepCount outside of class :" + value); }
    }
    public float stepsPerMin;

    public float lastStepDuration;
    float lastStepTime;


    float paceTimeCounter;

    #endregion

    public void OnAwake()
    {
        if (singleton == null) singleton = this;
        ClientNetworkManager.singleton.Step += OnStep;
    }

    void OnStep(object o, EventArgs args)
    {
        lastStepTime = Time.time;

        paceTimeCounter += lastStepDuration;


        if (lastStepDuration > .2f || stepCount == 0)
        {
            _stepCount++;
        }

    }

    public void OnUpdate()
    {
        if (stepCount > 0)
        {
            lastStepDuration = Time.time - lastStepTime;
        }
    }

    public IEnumerator PaceCounter(int paceIntervel)
    {
        int _stepPaceCount = 0;
        //float startTime = Time.time;
        while (true)
        {
            yield return new WaitForSeconds(paceIntervel);

            int stepPace = stepCount - _stepPaceCount;
            switch (paceIntervel)
            {
                case 60:
                    stepsPerMin = stepPace;
                    UILabel _stepsPerMin = PedometerUIManager.singleton.uiReferences.stepsPerMin;

                    if (stepsPerMin > 0) { _stepsPerMin.text = stepsPerMin.ToString(); }
                    else { _stepsPerMin.text = "-"; }

                    break;
            }
            _stepPaceCount = stepCount;
        }
    }
}
