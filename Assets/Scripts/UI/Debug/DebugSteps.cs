using UnityEngine;
using System.Collections;
using Vectrosity;

public class DebugSteps : MonoBehaviour
{
    public Graph[] axis;


    public void Awake()
    {
       StepDetector.singleton.Step += OnStep;
    }

    public void Start()
    {
        Graph[] graphs = GetComponents<Graph>();
        axis = new Graph[graphs.Length];

        for (int i = 0; i < axis.Length; i++)
        {
            axis[i] = graphs[i];
        }
    }

    void FixedUpdate()
    {
        axis[0].dataInput = Input.acceleration.magnitude * 2;
        ////axis[1].dataInput = 0;
        ////axis[2].dataInput = Input.acceleration.x + Input.acceleration.y + Input.acceleration.z;

    }


    void OnStep(object o, System.EventArgs args)
    {

    }

}
