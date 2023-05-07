using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public Vector3 Speed;
    private void Update()
    {
        transform.position = transform.position + Speed * Time.deltaTime;
    }
}
