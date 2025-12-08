using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Start 버튼에 연결할 함수
    public void OnStartClick()
    {
        //naration 실행
        SceneManager.LoadScene("Naration");
    }

    // Exit 버튼에 연결할 함수
    public void OnExitClick()
    {
        #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}