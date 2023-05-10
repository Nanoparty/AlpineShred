using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] GameObject Star;
    [SerializeField] GameObject Health;

    [SerializeField] public float SpawnFrequency;
    [SerializeField] public float HealthFrequency;

    private Player player;

    private bool _canSpawn = true;

    private GameManager gm;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (_canSpawn && !gm.paused && !gm.GameOver)
        {
            _canSpawn = false;
            StartCoroutine(SpawnRandom());
        }
    }

    private IEnumerator SpawnRandom()
    {
        GameObject selected = null;

        int ran = Random.Range(0, 100);
        if (ran < HealthFrequency)
        {
            selected = Health;
        }
        else
        {
            selected = Star;
        }

        if (selected == null) yield return null;

        GameObject o = Instantiate(selected, transform.position, Quaternion.identity);
        float width = GetComponent<MeshRenderer>().bounds.size.x;
        Vector3 spawnPos = new Vector3(Random.Range(player.HorizontalLimits.x - 0.5f, player.HorizontalLimits.y + 0.5f), transform.position.y, transform.position.z);

        o.transform.position = spawnPos;

        yield return new WaitForSeconds(SpawnFrequency);
        _canSpawn = true;
    }
}
