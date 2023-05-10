using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePickup : MonoBehaviour
{
    [SerializeField] public float speed = 5f;

    private float totalSpeed;

    private GameManager gm;
    private ObstacleSpawner obstacleSpawner;

    private void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        obstacleSpawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<ObstacleSpawner>();
    }

    private void Update()
    {
        if (gm.GameOver || gm.paused) return;

        Vector3 updatePos = transform.position;

        SetSpeed();

        updatePos.z += totalSpeed * Time.deltaTime;

        transform.position = updatePos;
    }

    public void SetSpeed()
    {
        totalSpeed = speed + obstacleSpawner.SpeedIncrease;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
        else if (other.CompareTag("Player"))
        {
            GameManager gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
            other.GetComponent<Player>().score += Mathf.Max(10, gm.totalSeconds);
            Destroy(gameObject);
        }
    }
}
