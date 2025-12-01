using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    public Slider VolumeSlider;

    private void OnEnable()
    {
        Time.timeScale = 0f;

        if (VolumeSlider != null)
        {
            VolumeSlider.value = AudioListener.volume;
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

   
}
