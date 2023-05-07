using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject Prefab;
    public float InitialDelay;
    public float Frequency;

    private void Start()
    {
        InvokeRepeating("SpawnPrefab", InitialDelay, Frequency);
    }

    private void SpawnPrefab()
    {
        Instantiate(Prefab, transform.position, transform.rotation);
    }
}
