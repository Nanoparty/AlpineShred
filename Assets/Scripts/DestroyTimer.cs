using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    public float time = 5f;

    private void Start()
    {
        Destroy(this.gameObject, time);
    }
}