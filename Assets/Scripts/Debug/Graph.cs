using UnityEngine;
using System.Collections;
using Vectrosity;

public class Graph : MonoBehaviour
{
    public int bufferAmount = 100;
    public float increment = .1f;
    public float startPosition = -5f;
    public Color lineColor = Color.red;
    public float lineWidth = 2;

    CircularBuffer<Vector3> buffer;
    VectorLine line;
    Vector3 point;
    float x;

    [HideInInspector]
    public float dataInput;

    void Awake()
    {
        buffer = new CircularBuffer<Vector3>(bufferAmount);
        x = startPosition;
    }

    void Start()
    {
        for (int i = 0; i < buffer.Count; i++)
        {
            x += increment;
            point = new Vector3(x, 0);
            buffer.Add(point);
        }

        line = new VectorLine("Line", buffer.ToArray(), lineColor, null, lineWidth, LineType.Continuous);
        line.Draw3DAuto();

    }

    void FixedUpdate()
    {
        x += increment;

        //add the points to the buffer (old points get dequeued)
        point = new Vector3(x, dataInput);
        buffer.Add(point);

        //---------- Move the line object ---------------------
        Vector3 pos = line.vectorObject.transform.position;
        pos.x -= increment;
        line.vectorObject.transform.position = pos;

        //update the current points
        line.points3 = buffer.ToArray();
    }
}
