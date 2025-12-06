using UnityEngine;
using System.Collections.Generic;
using TMPro;
public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    public TextMeshProUGUI waveText;  // 웨이브 표시용 TMP 연결
    public TextMeshProUGUI scoreText; // 킬 수(점수) 표시용 TMP 연결

    public Spawner[] spawners;

    public float waveDuration = 30f;
    public int maxWave = 60;

    public int currentWave = 1;
    float waveTimer = 0f;
    bool waveRunning = false;

    bool spawnedSomethingThisWave = false;

    public int totalEnemiesDefeated = 0;

    List<Enemy> aliveEnemies = new List<Enemy>();

    public bool WaveRunning => waveRunning;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateWaveUI();
        UpdateScoreUI();
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

    public void AddKillCount()
    {
        totalEnemiesDefeated++;
        UpdateScoreUI(); // 점수판 갱신
    }

    void StartWave()
    {
        waveRunning = true;
        waveTimer = 0f;
        spawnedSomethingThisWave = false;
        UpdateWaveUI();
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
    void UpdateWaveUI()
    {
        if (waveText != null)
        {
            waveText.text = $"WAVE: {currentWave}";
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"KILLS: {totalEnemiesDefeated}";
        }
    }
}
