using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvSpawner : MonoBehaviour
{
    [SerializeField] float SpawnLine = 8.650577f;

    [SerializeField] Transform SpawnPosition;

    [SerializeField] float MovementSpeed;

    [SerializeField] GameObject EnvPrefab;

    [SerializeField] public bool spawned = false;

    private ObstacleSpawner obstacleSpawner;

    private GameManager gm;

    private void Awake()
    {
        SpawnPosition = GameObject.FindGameObjectWithTag("EnvSpawn").transform;
        obstacleSpawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<ObstacleSpawner>();
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (gm.GameOver || gm.paused) return;

        if (!spawned && transform.position.z >= SpawnLine)
        {
            if (EnvPrefab == null) Debug.Log("NULL ENV PREFAB");
            spawned = true;
            GameObject o = Instantiate(EnvPrefab, SpawnPosition.position, Quaternion.identity);
            o.GetComponent<EnvSpawner>().spawned = false;
        }

        Vector3 updatePosition = transform.position;
        updatePosition.z += (MovementSpeed + obstacleSpawner.SpeedIncrease) * Time.deltaTime;
        transform.position = updatePosition;
    }
}
