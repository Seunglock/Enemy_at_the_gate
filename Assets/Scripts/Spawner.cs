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

        // 웨이브에 해당 안 하면 비활성화
        bool active = IsActiveThisWave(wave);
        gameObject.SetActive(active);

        if (!active) return;

        // 해당 웨이브에 보스가 지정되어 있다면 즉시 소환
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

        // WaveManager가 웨이브 실행 중이 아닐 때는 스폰 금지
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
