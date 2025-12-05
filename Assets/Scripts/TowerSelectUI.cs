using UnityEngine;
using UnityEngine.UI;
public class TowerSelectUI : MonoBehaviour
{
    [Header("UI 패널")]
    public GameObject panel;

    [Header("버튼 배치 설정 (RectTransform 연결)")]
    public RectTransform arrowBtn;
    public RectTransform mortarBtn;
    public RectTransform wizardBtn;
    public RectTransform poisonBtn;

    [Header("배치 좌표 설정")]
    public float startX = -355f;  // 시작 X 좌표
    public float startY = -177f;  // 시작 Y 좌표
    public float spacing = 120f;  // 버튼 사이의 간격 (버튼 크기에 맞춰 조절하세요)

    void Start()
    {
        // 게임 시작 시 버튼 위치 정렬 실행
        AlignButtons();
    }

    // 버튼을 지정된 좌표부터 일렬로 배치하는 함수
    void AlignButtons()
    {
        // 1. Arrow (첫 번째)
        if (arrowBtn != null)
            arrowBtn.anchoredPosition = new Vector2(startX, startY);

        // 2. Mortar (두 번째)
        if (mortarBtn != null)
            mortarBtn.anchoredPosition = new Vector2(startX + spacing, startY);

        // 3. Wizard (세 번째)
        if (wizardBtn != null)
            wizardBtn.anchoredPosition = new Vector2(startX + (spacing * 2), startY);

        // 4. Poison (네 번째)
        if (poisonBtn != null)
            poisonBtn.anchoredPosition = new Vector2(startX + (spacing * 3), startY);
    }

    // ---- UI 제어 (TowerPlacer에서 호출) ----

    public void Show()
    {
        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }

    // ---- 버튼 클릭 이벤트 (TowerPlacer와 연동) ----

    public void ClickArrow()
    {
        // TowerPlacer의 switch case "Arrow"와 매칭
        TowerPlacer.instance.SetSelectedTower("Arrow");
        Hide();
    }

    public void ClickMortar()
    {
        // TowerPlacer의 switch case "Mortar"와 매칭
        TowerPlacer.instance.SetSelectedTower("Mortar");
        Hide();
    }

    public void ClickWizard()
    {
        // TowerPlacer의 switch case "Wizard"와 매칭
        TowerPlacer.instance.SetSelectedTower("Wizard");
        Hide();
    }

    public void ClickPoison()
    {
        // TowerPlacer의 switch case "Poison"와 매칭
        TowerPlacer.instance.SetSelectedTower("Poison");
        Hide();
    }
}