using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    public Slider VolumeSlider;
    public Slider SensitivitySlider;

    private void OnEnable()
    {
        Time.timeScale = 0f;

        if (VolumeSlider != null)
        {
            //저장된 볼륨값 불러오기
            // 처음엔 기본값대로
            float savedVol = PlayerPrefs.GetFloat("MasterVolume", 0.5f);

            //슬라이더 위치와 실제 볼륨에 적용
            VolumeSlider.value = savedVol;
            AudioListener.volume = savedVol;
        }

        if (SensitivitySlider != null)
        {
            float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1.0f);
            SensitivitySlider.value = savedSensitivity;
        }
    }

    private void OnDisable()
    {
        if (SystemController.instance != null)
        {
            Time.timeScale = SystemController.instance.currentSpeed;
        }
        else
        {
            Time.timeScale = 1f;
        }

        // 변경된 설정 저장
        PlayerPrefs.Save();
    }

    public void ResumeGame()
    {
        gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        // 종료 시 시간 정상화 필수
        Time.timeScale = 1f;
        SceneManager.LoadScene("Start_Menu");
    }

    // 슬라이더를 움직일 때 호출되는 함수
    public void SetGlobalVolume(float volume)
    {
        AudioListener.volume = volume;

        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void SetMouseSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("MouseSensitivity", sensitivity);
    }
}