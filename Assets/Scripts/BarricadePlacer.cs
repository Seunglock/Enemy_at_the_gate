using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class BarricadePlacer : MonoBehaviour
{
    [Header("Settings")]
    public GameObject visualMarker; // 설치 가능 표시

    private GameObject currentBarricade;
    public bool isOccupied = false;

    private void Start()
    {
        if (visualMarker != null) visualMarker.SetActive(false);
    }

    // 설치 모드 표시 켜기/끄기
    public void SetHighlight(bool isOn)
    {
        if (isOccupied)
        {
            if (visualMarker != null) visualMarker.SetActive(false);
            return;
        }
        if (visualMarker != null) visualMarker.SetActive(isOn);
    }

    // ★ 핵심: 마우스 클릭 시 스스로 판단
    private void OnMouseDown()
    {
        // 1. UI 가리고 있으면 무시
        if (EventSystem.current.IsPointerOverGameObject()) return;

        // 2. 이미 설치됐으면 무시
        if (isOccupied) return;

        // 3. ★ "지금 설치 모드니?" 라고 ShopController에게 물어봄
        // 설치 모드가 아니면 아무 일도 안 일어남 (타워 창도 안 뜸)
        if (ShopController.instance != null && ShopController.instance.IsPlacementMode())
        {
            ShopController.instance.AttemptInstallBarricade(this);
        }
    }

    // 설치 실행
    public void InstallBarricade(GameObject prefab, float lifeTime)
    {
        isOccupied = true;
        if (visualMarker != null) visualMarker.SetActive(false);

        currentBarricade = Instantiate(prefab, transform.position, Quaternion.identity);
        StartCoroutine(DestroyRoutine(lifeTime));
    }

    IEnumerator DestroyRoutine(float time)
    {
        yield return new WaitForSeconds(time);
        if (currentBarricade != null) Destroy(currentBarricade);
        isOccupied = false;
    }
}