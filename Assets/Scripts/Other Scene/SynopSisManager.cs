using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // TextMeshPro를 쓰기 위해 필수!

public class SynopSisManager : MonoBehaviour
{
    [Header("UI 연결")]
    [Tooltip("시놉시스 내용이 적힌 TextMeshPro 오브젝트를 연결하세요.")]
    public TextMeshProUGUI synopsisText;

    [Header("이동 설정")]
    [Tooltip("텍스트가 올라가는 속도입니다.")]
    public float scrollSpeed = 50f;

    [Tooltip("이동할 메인 게임 씬의 이름을 정확히 적어주세요.")]
    public string nextSceneName = "MainScene";

    [Header("위치 미세 조절")]
    [Tooltip("0이면 텍스트 머리가 화면 바닥에서 시작합니다. 숫자가 클수록 더 위에서 시작합니다.")]
    public float startOffset = 0f;

    // 내부 변수
    private RectTransform textRect;
    private float stopYPosition; // 텍스트가 이 높이를 넘으면 씬 전환

    void Start()
    {
        // 1. 게임 시간이 멈춰있을 경우를 대비해 시간 정상화 (가장 중요)
        Time.timeScale = 1f;

        // 2. 텍스트 연결 확인
        if (synopsisText == null)
        {
            Debug.LogError("Synopsis Text가 연결되지 않았습니다! Inspector를 확인해주세요.");
            return;
        }

        // 3. RectTransform 가져오기
        textRect = synopsisText.GetComponent<RectTransform>();

        // 4. 텍스트 높이 계산을 위해 강제 업데이트 (TMPro 버그 방지)
        synopsisText.ForceMeshUpdate();

        // 5. 종료 지점 계산 (부모 오브젝트, 즉 마스크나 캔버스의 높이)
        RectTransform parentRect = textRect.parent.GetComponent<RectTransform>();
        stopYPosition = parentRect.rect.height;

        // 6. 시작 위치 설정
        // Pivot(Y=0) 기준으로, -높이만큼 내리면 화면 바로 아래에 숨겨짐
        // 여기서 startOffset만큼 더해서 위로 올림
        float startY = -synopsisText.preferredHeight + startOffset;

        textRect.anchoredPosition = new Vector2(0, startY);
    }

    void Update()
    {
        // 1. ESC 키를 누르면 즉시 스킵
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoToNextScene();
        }

        // 2. 텍스트가 연결되어 있다면 위로 이동
        if (synopsisText != null)
        {
            // 위쪽 방향(Vector2.up) * 속도 * 시간
            textRect.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

            // 3. 텍스트의 바닥(Pivot Y=0)이 화면 꼭대기(stopYPosition)를 넘어가면 종료
            if (textRect.anchoredPosition.y > stopYPosition)
            {
                GoToNextScene();
            }
        }
    }

    // 다음 씬으로 넘어가는 함수
    void GoToNextScene()
    {
        Debug.Log("시놉시스 종료. 다음 씬으로 이동합니다: " + nextSceneName);
        SceneManager.LoadScene(nextSceneName);
    }
}