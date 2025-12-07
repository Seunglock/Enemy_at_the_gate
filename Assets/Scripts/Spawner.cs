using UnityEngine;

[System.Serializable]
public class WaveRange
{
    public int startWave;
    public int endWave;
}

[System.Serializable]
public class BossRule
{
    public int wave;
    public GameObject bossPrefab;
}

public class Spawner : MonoBehaviour
{
    public GameObject[] normalEnemies;
    public float spawnInterval = 2f;
    public int pathIndex;

    public WaveRange[] activeRanges;
    public BossRule[] bossRules;

    float timer = 0f;

    public bool IsActiveThisWave(int wave)
    {
        foreach (var r in activeRanges)
        {
            if (wave >= r.startWave && wave <= r.endWave)
                return true;
        }
        return false;
    }

    public void SetupForWave(int wave)
    {
        timer = 0f;

        bool active = IsActiveThisWave(wave);
        gameObject.SetActive(active);

        if (!active) return;

        foreach (var rule in bossRules)
        {
            if (rule.wave == wave && rule.bossPrefab != null)
            {
                Spawn(rule.bossPrefab);
            }
        }
    }

    void Update()
    {
        if (!gameObject.activeSelf) return;

        if (!WaveManager.instance.WaveRunning) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnRandomEnemy();
            timer = 0f;
        }
    }

    void SpawnRandomEnemy()
    {
        if (normalEnemies == null || normalEnemies.Length == 0)
            return;

        int idx = UnityEngine.Random.Range(0, normalEnemies.Length);
        Spawn(normalEnemies[idx]);
    }

    void Spawn(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);

        Enemy e = obj.GetComponent<Enemy>();
        e.SetPath(WaypointManager.instance.GetPath(pathIndex));

        WaveManager.instance.RegisterEnemy(e);
    }
}
