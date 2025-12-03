using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SystemController : MonoBehaviour
{
    public static SystemController instance;

    
    public float currentSpeed = 1.0f; // 기본 스피드

    public int level = 1;
    public int exp = 0;
    public int maxExp = 50;
    public int gold = 200;

    public GameObject pauseMenuPanel;
    public GameObject levelUpPanel;
    public Text speedButtonText;
    public Text goldButtonText;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // 시작 시 패널 끄기
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
        if (levelUpPanel != null) levelUpPanel.SetActive(false);

        // 초기 속도 설정
        currentSpeed = 1.0f;
        Time.timeScale = currentSpeed;

        UpdateSpeedText();
        UpdateGoldText(); // 초기 골드 텍스트 업데이트
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
        // 레벨업 패널이 켜져있으면 일시정지 패널을 열지 않음 (중복 방지)
        if (levelUpPanel != null && levelUpPanel.activeSelf) return;

        if (pauseMenuPanel != null)
        {
            // 켜져있으면 끄고, 꺼져있으면 켭니다.
            // 패널이 켜질 때 패널에 붙은 스크립트(PausePanel)가 Time.timeScale을 0으로 만듭니다.
            pauseMenuPanel.SetActive(!pauseMenuPanel.activeSelf);
        }
    }


    // 경험치 추가
    public void AddExp(int amount)
    {
        exp += amount;
        CheckLevelUp();
    }
    void CheckLevelUp()
    {
        // 경험치가 목표치보다 많거나 같으면 레벨업
        while (exp >= maxExp)
        {
            exp -= maxExp;      // 남은 경험치 이월
            level++;            // 레벨 증가
            maxExp += 40;       // 다음 필요 경험치 증가 (50 -> 90 -> 130...)

            // 여기서 LevelUpPanel을 켜줍니다! (연동 핵심)
            if (levelUpPanel != null)
            {
                levelUpPanel.SetActive(true);
                // LevelUpPanel이 켜지면 그 스크립트(LevelUpPanelController)의 OnEnable이 실행되어
                // 자동으로 게임이 멈추고 보상 목록이 뜹니다.
            }
        }
    }

    public bool TrySpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            UpdateGoldText();
            return true; // 구매 성공
        }
        else
        {
            Debug.Log($"골드가 부족합니다! (보유: {gold}, 필요: {amount})");
            return false; // 구매 실패
        }
    }

    // 골드 추가
    public void AddGold(int amount)
    {
        gold += amount;
        UpdateGoldText();
    }

    // 골드 UI 업데이트
    void UpdateGoldText()
    {
        if (goldButtonText != null)
        {
            goldButtonText.text = $"{gold:N0}";
        }
    }

    // 배속 변경 버튼 기능
    public void ChangeGameSpeed()
    {
        // 속도 순환 로직
        if (Mathf.Approximately(currentSpeed, 1.0f))
        {
            currentSpeed = 1.5f;
        }
        else if (Mathf.Approximately(currentSpeed, 1.5f))
        {
            currentSpeed = 2.0f;
        }
        else if (Mathf.Approximately(currentSpeed, 2.0f))
        {
            currentSpeed = 0.5f;
        }
        else
        {
            currentSpeed = 1.0f;
        }

        bool isPaused = (pauseMenuPanel != null && pauseMenuPanel.activeSelf);
        bool isLevelUp = (levelUpPanel != null && levelUpPanel.activeSelf);

        if (pauseMenuPanel == null || !pauseMenuPanel.activeSelf)
        {
            Time.timeScale = currentSpeed;
        }

        UpdateSpeedText();
    }

    // 배속 UI 업데이트
    void UpdateSpeedText()
    {
        if (speedButtonText != null)
        {
            speedButtonText.text = $"x{currentSpeed:0.0}";
        }
    }
}