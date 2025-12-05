using System;
using System.Diagnostics;
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

    void Update()
    {
        timer += Time.deltaTime;

        // 1) 일반 몬스터는 ~~bossSpawned와 상관없이~~ 계속 스폰
        if (timer >= spawnInterval)
        {
            SpawnRandomEnemy();
            timer = 0f;
        }

        // 2) 보스 스폰은 딱 한 번
        if (!bossSpawned && Time.timeSinceLevelLoad >= bossSpawnTime)
        {
            SpawnBoss();
            bossSpawned = true;
        }
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
        {
            UnityEngine.Debug.LogWarning("Boss Enemy is NULL!!");
            return;   // 보스 null이어도 일반 몬스터 스폰은 계속됨
        }

        Spawn(bossEnemy);
    }

    void Spawn(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);
        Enemy e = obj.GetComponent<Enemy>();
        e.SetPath(WaypointManager.instance.GetPath(pathIndex));
    }
}
