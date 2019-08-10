using UnityEngine;
using System.Collections;

public class HostRuntimeManager : MonoBehaviour
{
    public StepDetector stepManager;

    void Awake()
    {
        SetAppSettings();
        stepManager = StepDetector.singleton = new StepDetector();
    }

    void FixedUpdate()
    {
        stepManager.Update();
    }

    void LateUpdate()
    {
        stepManager.LateUpdate();
    }

    void SetAppSettings()
    {
        Screen.orientation = ScreenOrientation.Landscape;
    }

}
