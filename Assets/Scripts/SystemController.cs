using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SystemController : MonoBehaviour
{
    public static SystemController instance;

    public float currentSpeed = 1.0f;

    public int level = 1;
    public int exp = 0;
    public int maxExp = 50;
    public int gold = 200;

    public GameObject pauseMenuPanel;
    public GameObject levelUpPanel;
    public TextMeshProUGUI speedButtonText;
    public TextMeshProUGUI goldButtonText;

    // ------------------------------
    // 타워 강화 시스템
    // ------------------------------
    [Header("Tower Upgrade Settings")]
    public float towerDamageMultiplier = 1.0f;
    public float towerFireRateMultiplier = 1.0f;   // 공격속도 → 쿨타임 감소
    public float towerRangeMultiplier = 1.0f;

    public int upgradeCost = 100;
    public float upgradeStep = 0.2f;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
        if (levelUpPanel != null)
            levelUpPanel.SetActive(false);

        currentSpeed = 1.0f;
        Time.timeScale = currentSpeed;

        UpdateSpeedText();
        UpdateGoldText();
    }

    void Update()
    {
        if (levelUpPanel != null && levelUpPanel.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenuPanel != null)
            {
                pauseMenuPanel.SetActive(!pauseMenuPanel.activeSelf);
            }
        }
    }

    public void TogglePausePanel()
    {
        if (levelUpPanel != null && levelUpPanel.activeSelf) return;

        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(!pauseMenuPanel.activeSelf);
        }
    }

    public void AddExp(int amount)
    {
        exp += amount;
        CheckLevelUp();
    }

    void CheckLevelUp()
    {
        while (exp >= maxExp)
        {
            exp -= maxExp;
            level++;
            maxExp += 40;

            if (levelUpPanel != null)
            {
                levelUpPanel.SetActive(true);
            }
        }
    }

    public bool TrySpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            UpdateGoldText();
            return true;
        }
        else
        {
            Debug.Log($"골드 부족! (보유: {gold}, 필요: {amount})");
            return false;
        }
    }

    public void AddGold(int amount)
    {
        gold += amount;
        UpdateGoldText();
    }

    void UpdateGoldText()
    {
        if (goldButtonText != null)
        {
            goldButtonText.text = $"{gold:N0}";
        }
    }

    public void ChangeGameSpeed()
    {
        if (Mathf.Approximately(currentSpeed, 1.0f)) currentSpeed = 1.5f;
        else if (Mathf.Approximately(currentSpeed, 1.5f)) currentSpeed = 2.0f;
        else if (Mathf.Approximately(currentSpeed, 2.0f)) currentSpeed = 0.5f;
        else currentSpeed = 1.0f;

        if (pauseMenuPanel == null || !pauseMenuPanel.activeSelf)
        {
            Time.timeScale = currentSpeed;
        }

        UpdateSpeedText();
    }

    void UpdateSpeedText()
    {
        if (speedButtonText != null)
        {
            speedButtonText.text = $"x{currentSpeed:0.0}";
        }
    }


    // =======================================================
    //                  강화 기능 추가
    // =======================================================

    public void UpgradeTowerDamage()
    {
        if (!TrySpendGold(upgradeCost)) return;

        towerDamageMultiplier += upgradeStep;
        Debug.Log($"[강화] 타워 공격력 x{towerDamageMultiplier}");
        upgradeCost += 50;
    }

    public void UpgradeTowerFireRate()
    {
        if (!TrySpendGold(upgradeCost)) return;

        towerFireRateMultiplier -= 0.1f;
        if (towerFireRateMultiplier < 0.2f)
            towerFireRateMultiplier = 0.2f;

        Debug.Log($"[강화] 타워 공격속도 x{towerFireRateMultiplier}");
        upgradeCost += 50;
    }

    public void UpgradeTowerRange()
    {
        if (!TrySpendGold(upgradeCost)) return;

        towerRangeMultiplier += upgradeStep;

        Debug.Log($"[강화] 타워 사거리 x{towerRangeMultiplier}");
        upgradeCost += 50;
    }
}
