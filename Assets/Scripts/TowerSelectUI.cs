using UnityEngine;
using UnityEngine.UI;

public class TowerSelectUI : MonoBehaviour
{
    [Header("UI 패널")]
    public GameObject panel; // 인스펙터에서 실제 패널 오브젝트 연결 필수!

    // ---- UI 제어 (TowerPlacer에서 호출) ----

    public void Show()
    {
        // 1. 패널 켜기
        if (panel != null) panel.SetActive(true);

        // 2. ★ 중요: ShopController에게 "다른 UI들 숨겨줘!"라고 요청
        if (ShopController.instance != null)
        {
            ShopController.instance.SetTowerSelectMode(true);
        }
    }

    public void Hide()
    {
        // 1. 패널 끄기
        if (panel != null) panel.SetActive(false);

        // 2. ★ 중요: ShopController에게 "숨겼던 UI 다시 보여줘!"라고 요청
        if (ShopController.instance != null)
        {
            ShopController.instance.SetTowerSelectMode(false);
        }
    }

    // ---- 버튼 클릭 이벤트 (인스펙터의 버튼 OnClick에 연결) ----

    public void ClickArrow()
    {
        if (TowerPlacer.instance != null)
            TowerPlacer.instance.SetSelectedTower("Arrow");

        Hide(); // 선택했으니 창을 닫고, UI도 다시 보이게 함
    }

    public void ClickMortar()
    {
        if (TowerPlacer.instance != null)
            TowerPlacer.instance.SetSelectedTower("Mortar");

        Hide();
    }

    public void ClickWizard()
    {
        if (TowerPlacer.instance != null)
            TowerPlacer.instance.SetSelectedTower("Wizard");

        Hide();
    }

    public void ClickPoison()
    {
        if (TowerPlacer.instance != null)
            TowerPlacer.instance.SetSelectedTower("Poison");

        Hide();
    }
}