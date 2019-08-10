using System;
using UnityEngine;
using System.Collections;

public class PedometerUIManager : MonoBehaviour
{
    [System.Serializable]
    public class UIReferences
    {
        public UILabel stepsPerMin;
        public UILabel stepCount;
    }
    public UIReferences uiReferences;

    Pedometer pedometer;

    public static PedometerUIManager singleton;

    void Awake()
    {
        if (singleton == null) singleton = this;

        pedometer = Pedometer.singleton;
        ClientNetworkManager.singleton.Step += OnSteps;
    }

    void OnSteps(object o, EventArgs args)
    {
        uiReferences.stepCount.text = pedometer.stepCount.ToString();
        //uiReferences.stepsPerMin.text = pedometer.stepsPerMin.ToString();
    }
}
