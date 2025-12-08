using System.Collections;
using UnityEngine;
using TMPro; // TextMeshPro 필수

public class EndingManager : MonoBehaviour
{
    [Header("UI 연결")]
    [Tooltip("엔딩 크레딧 내용이 적힌 TextMeshPro 오브젝트")]
    public TextMeshProUGUI endingText;

    [Header("설정")]
    [Tooltip("텍스트가 올라가는 속도")]
    public float scrollSpeed = 50f;

    [Tooltip("텍스트가 다 올라간 후, 게임이 종료되기 전까지 대기하는 시간(초)")]
    public float waitBeforeQuit = 5f;

    [Header("위치 미세 조절")]
    public float startOffset = 0f;

    // 내부 변수
    private RectTransform textRect;
    private float stopYPosition;
    private bool isEndingSequenceStarted = false; // 종료 시퀀스가 시작됐는지 확인

    void Start()
    {
        //시간 정상화
        Time.timeScale = 1f;

        if (endingText == null)
        {
            Debug.LogError("Ending Text가 연결되지 않았습니다!");
            return;
        }

        //초기화 및 높이 계산
        textRect = endingText.GetComponent<RectTransform>();
        endingText.ForceMeshUpdate();

        //Mask의 높이를 종료 지점으로 설정
        RectTransform parentRect = textRect.parent.GetComponent<RectTransform>();
        stopYPosition = parentRect.rect.height;

        //시작 위치 설정
        float startY = -endingText.preferredHeight + startOffset;
        textRect.anchoredPosition = new Vector2(0, startY);
    }

    void Update()
    {
        //이미 종료 카운트다운 중이면 아무것도 안 함
        if (isEndingSequenceStarted) return;

        //즉시 종료
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC 눌림: 즉시 종료합니다.");
            QuitGame();
        }

        //텍스트 스크롤
        if (endingText != null)
        {
            textRect.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

            // 텍스트 바닥이 화면 꼭대기를 넘어갔는지 확인
            if (textRect.anchoredPosition.y > stopYPosition)
            {
                // 5초 대기 후 종료하는 코루틴 시작
                StartCoroutine(QuitAfterDelay());
            }
        }
    }

    // 5초 기다렸다가 종료하는 함수
    IEnumerator QuitAfterDelay()
    {
        isEndingSequenceStarted = true; // 중복 실행 방지

        Debug.Log($"엔딩 크레딧 완료. {waitBeforeQuit}초 후에 게임을 종료합니다...");

        // 설정한 시간만큼 대기
        yield return new WaitForSeconds(waitBeforeQuit);

        QuitGame();
    }

    // 실제 게임 종료 함수
    void QuitGame()
    {
        Debug.Log("게임 종료 (Application.Quit)");

#if UNITY_EDITOR
        // 유니티 에디터에서는 게임 종료가 안 되므로 플레이 모드를 끔
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // 실제 빌드된 게임 종료
            Application.Quit();
#endif
    }
}