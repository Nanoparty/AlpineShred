using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] public float speed = 5f;
    [SerializeField] public GameObject PickupEffect;

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
        if (other.CompareTag("Player"))
        {
            SoundManager.Instance.PlayHealth();
            Instantiate(PickupEffect, transform.position, Quaternion.identity);
            other.GetComponent<Player>().health++;
            Destroy(gameObject);
        }
    }
}
