using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            player.score++;
        }
    }
}
