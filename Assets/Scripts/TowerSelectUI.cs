/*using UnityEngine;

public class TowerSelectUI : MonoBehaviour
{
    public GameObject panel;

    // UI 켜기
    public void Show()
    {
        panel.SetActive(true);
    }

    // UI 끄기
    public void Hide()
    {
        panel.SetActive(false);
    }

    // ---- 4개의 타워 버튼 ----
    public void ClickArrow()
    {
        Debug.Log("CLicked ARrow");
        TowerPlacer.instance.SetSelectedTower("Arrow");
        Hide();
    }

    public void ClickMortar()
    {
        TowerPlacer.instance.SetSelectedTower("Mortar");
        Hide();
    }

    public void ClickWizard()
    {
        TowerPlacer.instance.SetSelectedTower("Wizard");
        Hide();
    }

    public void ClickPoison()
    {
        TowerPlacer.instance.SetSelectedTower("Poison");
        Hide();
    }
}
*/

using UnityEngine;

public class TowerSelectUI : MonoBehaviour
{
    public GameObject panel;

    public void Show()
    {
        Debug.Log("[UI] Show() 호출됨 - 패널을 켭니다.");
        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }

    // ---- 4개의 타워 버튼 ----

    public void ClickArrow()
    {
        Debug.Log("[UI] 화살(Arrow) 버튼 클릭됨");
        // 싱글톤 안전 장치
        if (CheckInstance())
        {
            TowerPlacer.instance.SetSelectedTower("Arrow");
            Hide();
        }
    }

    public void ClickMortar()
    {
        Debug.Log("[UI] 박격포(Mortar) 버튼 클릭됨");
        if (CheckInstance())
        {
            TowerPlacer.instance.SetSelectedTower("Mortar");
            Hide();
        }
    }

    public void ClickWizard()
    {
        Debug.Log("[UI] 마법사(Wizard) 버튼 클릭됨");
        if (CheckInstance())
        {
            TowerPlacer.instance.SetSelectedTower("Wizard");
            Hide();
        }
    }

    public void ClickPoison()
    {
        Debug.Log("[UI] 독(Poison) 버튼 클릭됨");
        if (CheckInstance())
        {
            TowerPlacer.instance.SetSelectedTower("Poison");
            Hide();
        }
    }

    // TowerPlacer가 존재하는지 확인하는 함수
    private bool CheckInstance()
    {
        if (TowerPlacer.instance == null)
        {
            Debug.LogError(" [치명적 오류] TowerPlacer.instance가 null입니다! 씬에 TowerPlacerSystem 오브젝트가 있는지 확인하세요.");
            return false;
        }
        return true;
    }
}