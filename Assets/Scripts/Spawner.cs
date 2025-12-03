using System;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] normalEnemies;
    public GameObject bossEnemy;

    public float spawnInterval = 2f;
    public float bossSpawnTime = 30f;

    public int pathIndex;

    private float timer = 0f;
    private bool bossSpawned = false;

    private bool activeThisWave = true;

    void Update()
    {
        if (!activeThisWave)
            return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnRandomEnemy();
            timer = 0f;
        }

        if (!bossSpawned && Time.timeSinceLevelLoad >= bossSpawnTime)
        {
            SpawnBoss();
            bossSpawned = true;
        }
    }

    public void SetupForWave(int wave)
    {
        activeThisWave = true;

        timer = 0f;
        bossSpawned = false;

        if (wave % 10 == 0)
            bossSpawnTime = 2f;
        else
            bossSpawnTime = 9999f;
    }

    void SpawnRandomEnemy()
    {
        if (normalEnemies == null || normalEnemies.Length == 0)
            return;

        int idx = UnityEngine.Random.Range(0, normalEnemies.Length);
        Spawn(normalEnemies[idx]);
    }

    void SpawnBoss()
    {
        if (bossEnemy == null)
            return;

        Spawn(bossEnemy);
    }

    void Spawn(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);

        Enemy e = obj.GetComponent<Enemy>();
        e.SetPath(WaypointManager.instance.GetPath(pathIndex));
    }
}
