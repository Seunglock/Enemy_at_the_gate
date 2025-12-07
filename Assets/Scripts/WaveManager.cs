using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    public TextMeshProUGUI waveText;
    public TextMeshProUGUI scoreText;

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
        UpdateScoreUI();
    }

    void StartWave()
    {
        waveRunning = true;
        waveTimer = 0f;
        spawnedSomethingThisWave = false;

        UpdateWaveUI();

        if (SoundManager.instance != null)
        {
            SoundManager.instance.CheckWaveBGM(currentWave);
        }

        foreach (Spawner sp in spawners)
            if (sp != null)
                sp.SetupForWave(currentWave);

        
        UnityEngine.Debug.Log($"[WAVE START] Wave {currentWave} 시작");
    }

    void NextWave()
    {
        currentWave++;

        if (currentWave > maxWave)
        {
            waveRunning = false;
            UnityEngine.Debug.Log("[GAME] 모든 웨이브 종료");
            return;
        }

        UnityEngine.Debug.Log($"[WAVE CLEAR] 다음 웨이브: {currentWave}");

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
