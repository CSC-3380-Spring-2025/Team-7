using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawer : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject [] enemyPrefabs;
    public float spawnInterval = 5f; // Time in seconds between spawns


public void Start(){
    InvokeRepeating("SpawnEnemy", 0f, 5f); // Call SpawnEnemy every 3 seconds
    }

    void SpawnEnemy()
    {
            int randEnemy = Random.Range(0, enemyPrefabs.Length);
            int randSpawnPoint = Random.Range(0, spawnPoints.Length);

            Instantiate(enemyPrefabs[randEnemy], spawnPoints[randSpawnPoint].position, transform.rotation);
    } 
}
