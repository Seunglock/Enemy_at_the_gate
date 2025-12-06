using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    public Slider VolumeSlider;
    public Slider SensitivitySlider;

    private void OnEnable()
    {
        Time.timeScale = 0f;

        if (VolumeSlider != null)
        {
            VolumeSlider.value = AudioListener.volume;
        }
        if (SensitivitySlider != null)
        {
            float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1.0f);
            SensitivitySlider.value = savedSensitivity;
        }
    }

    private void OnDisable()
    {
        if (SystemController.instance!=null)
        {
            Time.timeScale = SystemController.instance.currentSpeed;

        }
        else
        {
            Time.timeScale = 1f;

        }
        PlayerPrefs.Save();
    }

    public void ResumeGame()
    {
        gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        // 에디터에서는 플레이 모드 종료, 빌드 버전에서는 앱 종료
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void SetGlobalVolume(float volume)
    {
        AudioListener.volume = volume;
        // 나중에 PlayerPrefs 등을 이용해 저장 기능 추가 가능
        // PlayerPrefs.SetFloat("MasterVolume", volume);
    }
    public void SetMouseSensitivity(float sensitivity)
    {
        // PlayerPrefs를 통해 값을 저장해두면, 카메라 스크립트에서 이 값을 읽어서 쓸 수 있습니다.
        PlayerPrefs.SetFloat("MouseSensitivity", sensitivity);
    }

}
