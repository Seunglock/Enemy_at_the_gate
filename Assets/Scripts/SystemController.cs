using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SystemController : MonoBehaviour
{
    public static SystemController instance;

    
    public float currentSpeed = 1.0f; // 기본 스피드

    public int exp = 0;
    public int gold = 0;

    public GameObject pauseMenuPanel;
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

        // 초기 속도 설정
        currentSpeed = 1.0f;
        Time.timeScale = currentSpeed;

        UpdateSpeedText();
        UpdateGoldText(); // 초기 골드 텍스트 업데이트
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenuPanel != null)
            {
                pauseMenuPanel.SetActive(!pauseMenuPanel.activeSelf);
            }
        }
    }

    // 경험치 추가
    public void AddExp(int amount)
    {
        exp += amount;
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