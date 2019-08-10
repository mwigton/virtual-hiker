using UnityEngine;
using System.Collections;

public class ClientRuntimeManager : MonoBehaviour
{

    Pedometer pedometer = new Pedometer();

    void Awake()
    {
        SetAppSettings();
        pedometer.OnAwake();
        StartCoroutine(pedometer.PaceCounter(60));
    }

    void SetAppSettings()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    void Update()
    {
        pedometer.OnUpdate();
    }

}
