using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
            other.GetComponent<Player>().score += Mathf.Max(10, gm.totalSeconds);
            Destroy(gameObject);
        }
    }
}
