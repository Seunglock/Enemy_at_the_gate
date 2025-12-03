using UnityEngine;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    public Spawner[] spawners;

    public float waveDuration = 30f;
    public int maxWave = 60;

    public int currentWave = 1;
    private float waveTimer = 0f;
    private bool waveRunning = false;

    private List<Enemy> aliveEnemies = new List<Enemy>();

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
        if (!waveRunning) return;

        waveTimer += Time.deltaTime;

        aliveEnemies.RemoveAll(e => e == null);

        if (aliveEnemies.Count == 0)
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

        foreach (Spawner sp in spawners)
            sp.gameObject.SetActive(true);

        foreach (Spawner sp in spawners)
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
