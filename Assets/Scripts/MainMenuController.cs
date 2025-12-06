using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Start 버튼에 연결할 함수
    public void OnStartClick()
    {
        // "Naration"이라는 이름의 씬을 로드합니다.
        // 주의: 씬 이름 철자가 정확해야 합니다.
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