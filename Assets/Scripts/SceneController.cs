using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    // 1. 노말 엔딩 (51 웨이브 도달 시)
    public void LoadNormalEnd()
    {
        Debug.Log("축하합니다! 51 웨이브 도달. 노말 엔딩!");
        SceneManager.LoadScene("NormalEndScene");
    }

    // 2. 배드 엔딩 (죽었을 시)
    public void LoadBadEnd()
    {
        Debug.Log("게임 오버... 배드 엔딩.");
        SceneManager.LoadScene("BadEndScene");
    }
}
