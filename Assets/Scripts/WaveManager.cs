using UnityEngine;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    public Spawner[] spawners;

    public float waveDuration = 30f;
    public int maxWave = 60;

    public int currentWave = 1;
    float waveTimer = 0f;
    bool waveRunning = false;

    bool spawnedSomethingThisWave = false;

    List<Enemy> aliveEnemies = new List<Enemy>();

    public bool WaveRunning => waveRunning;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartWave();
    }

    void Update()
    {
        if (!waveRunning)
            return;

        waveTimer += Time.deltaTime;

        UnityEngine.Debug.Log("Wave: " + currentWave + " Time: " + waveTimer);

        aliveEnemies.RemoveAll(e => e == null);

        if (spawnedSomethingThisWave && aliveEnemies.Count == 0)
        {
            NextWave();
            return;
        }

        if (waveTimer >= waveDuration)
        {
            NextWave();
        }
    }

    public void RegisterEnemy(Enemy e)
    {
        aliveEnemies.Add(e);

        spawnedSomethingThisWave = true;

        float scale = 1f + (currentWave - 1) * 0.25f;
        e.hp *= scale;
    }

    public void UnregisterEnemy(Enemy e)
    {
        aliveEnemies.Remove(e);
    }

    void StartWave()
    {
        waveRunning = true;
        waveTimer = 0f;
        spawnedSomethingThisWave = false;

        foreach (Spawner sp in spawners)
            if (sp != null)
                sp.SetupForWave(currentWave);
    }

    void NextWave()
    {
        currentWave++;

        if (currentWave > maxWave)
        {
            waveRunning = false;
            return;
        }

        StartWave();
    }
}
