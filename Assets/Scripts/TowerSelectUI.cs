using UnityEngine;
using UnityEngine.UI;

public class TowerSelectUI : MonoBehaviour
{
    public GameObject panel; //상점 패널 연결
    
    //tower placer 호출
    public void Show()
    {
        // 패널 켜기
        if (panel != null) panel.SetActive(true);

        if (ShopController.instance != null)
        {
            ShopController.instance.SetTowerSelectMode(true);
        }
    }

    public void Hide()
    {
        
        if (panel != null) panel.SetActive(false);

        //패널 복구
        if (ShopController.instance != null)
        {
            ShopController.instance.SetTowerSelectMode(false);
        }
    }

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