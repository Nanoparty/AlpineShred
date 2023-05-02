using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> Obstacles;

    [SerializeField] public float SpawnFrequency;
    public float SpeedIncrease;

    [SerializeField] Transform ObstacleContainer;

    private Player player;

    private bool _canSpawn = true;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        if (_canSpawn)
        {
            _canSpawn = false;
            StartCoroutine(SpawnRandom());
        }
    }

    private IEnumerator SpawnRandom()
    {
        if (Obstacles.Count == 0) Debug.Log("NO OBSTACLES");
        GameObject selected = Obstacles[Random.Range(0, Obstacles.Count)];
        if (selected == null) yield return null;

        GameObject o = Instantiate(selected, transform.position, Quaternion.identity);
        float width = GetComponent<MeshRenderer>().bounds.size.x;
        Debug.Log("Width:" + width);
        Vector3 spawnPos = new Vector3(Random.Range(player.HorizontalLimits.x, player.HorizontalLimits.y), transform.position.y, transform.position.z);
        o.transform.position = spawnPos;
        o.transform.SetParent(ObstacleContainer, true);
        o.GetComponent<Obstacle>().SetSpeed(SpeedIncrease);

        yield return new WaitForSeconds(SpawnFrequency);
        _canSpawn = true;
    }
}
