using UnityEngine;

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
