using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowTrail : MonoBehaviour
{
    public float lifetime = 5f;

    public float minimumVertexDistance = 0.1f;

    public Vector3 velocity;

    LineRenderer line;

    List<Vector3> points;

    Queue<float> spawnTimes = new Queue<float>();

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.useWorldSpace = true;
        points = new List<Vector3>() { transform.position };
        line.SetPositions(points.ToArray());
    }

    private void AddPoint(Vector3 position)
    {
        points.Insert(1, position);
        spawnTimes.Enqueue(Time.time);
    }

    private void RemovePoint()
    {
        spawnTimes.Dequeue();
        points.RemoveAt(points.Count - 1);
    }

    private void Update()
    {
        while (spawnTimes.Count > 0 && spawnTimes.Peek() + lifetime < Time.time)
        {
            RemovePoint();
        }

        Vector3 diff = -velocity * Time.deltaTime;
        for (int i = 1; i < points.Count; i++)
        {
            points[i] += diff;
        }

        if (points.Count < 2 || Vector3.Distance(transform.position, points[1]) > minimumVertexDistance)
        {
            AddPoint(transform.position);
        }

        points[0] = transform.position;

        line.positionCount = points.Count;
        line.SetPositions(points.ToArray());
    }
}
