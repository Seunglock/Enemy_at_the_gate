using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 20f;       // 키보드 이동 속도
    public float zoomSpeed = 5f;        // 줌 기본 속도
    public float minZoom = 3f;          // 최소 줌
    public float maxZoom = 15f;         // 최대 줌

    [Header("Mouse Settings")]
    public bool enableMouseDrag = true; // 마우스 드래그 이동 활성화 여부
    [Range(0.1f, 5f)]
    public float sensitivityMultiplier = 1.0f; // 현재 적용된 감도 (디버깅용 표시)

    private Camera cam;
    private Vector3 dragOrigin; // 마우스 드래그 시작 위치 저장

    void Start()
    {
        cam = GetComponent<Camera>();
        UpdateSensitivity(); // 시작할 때 감도 불러오기
    }

    void Update()
    {
        // 매 프레임 감도를 체크하는 것은 비효율적일 수 있으나, 
        // 설정창에서 바로 반영되는 것을 확인하기 위해 여기 둡니다.
        // 최적화를 원하면 일시정지가 풀릴 때만 호출하도록 구조를 변경할 수 있습니다.
        if (Time.timeScale > 0)
        {
            UpdateSensitivity();
        }

        MoveCamera();
        ZoomCamera();

        if (enableMouseDrag)
        {
            DragCamera();
        }
    }

    // PlayerPrefs에서 감도 값을 가져오는 함수
    void UpdateSensitivity()
    {
        // PausePanel에서 저장한 키("MouseSensitivity")와 동일해야 합니다.
        // 기본값은 1.0f입니다.
        sensitivityMultiplier = PlayerPrefs.GetFloat("MouseSensitivity", 1.0f);
    }

    void MoveCamera()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 키보드 이동은 감도보다는 moveSpeed의 영향을 받지만, 원한다면 곱해줄 수 있습니다.
        // 여기서는 키보드 이동은 일정하게 유지합니다.
        Vector3 move = new Vector3(h, v, 0).normalized;
        transform.position += move * moveSpeed * Time.deltaTime;
    }

    void ZoomCamera()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            // [감도 적용] 줌 속도에 감도를 곱해줍니다.
            float finalZoomSpeed = zoomSpeed * sensitivityMultiplier;

            cam.orthographicSize -= scroll * finalZoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
    }

    // [추가된 기능] 마우스 드래그로 카메라 이동
    void DragCamera()
    {
        // 마우스 휠 버튼(2) 또는 우클릭(1)을 누른 시점의 위치 기억
        if (Input.GetMouseButtonDown(2) || Input.GetMouseButtonDown(1))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        // 드래그 중일 때
        if (Input.GetMouseButton(2) || Input.GetMouseButton(1))
        {
            Vector3 currentPos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 difference = dragOrigin - currentPos;

            // [감도 적용] 마우스로 끄는 속도에 감도를 반영합니다.
            // 감도가 높으면 조금만 움직여도 화면이 많이 이동합니다.
            transform.position += difference * sensitivityMultiplier;
        }
    }
}