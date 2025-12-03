using UnityEngine;
using System.Collections;

public class TowerSelectUI : MonoBehaviour
{
    public GameObject panel;

    public float inputBlockTime = 0.5f;
    private bool inputBlocked = false;

    IEnumerator BlockInputRoutine()
    {
        inputBlocked = true;
        yield return new WaitForSeconds(inputBlockTime);
        inputBlocked = false;
    }

    public void Show()
    {
        panel.SetActive(true);
        StartCoroutine(BlockInputRoutine());
    }

    public void Hide()
    {
        panel.SetActive(false);
    }

    private bool CanClick()
    {
        return !inputBlocked;
    }

    public void ClickArrow()
    {
        if (!CanClick()) return;
        TowerPlacer.instance.SetSelectedTower("Arrow");
        Hide();
    }

    public void ClickMortar()
    {
        if (!CanClick()) return;
        TowerPlacer.instance.SetSelectedTower("Mortar");
        Hide();
    }

    public void ClickWizard()
    {
        if (!CanClick()) return;
        TowerPlacer.instance.SetSelectedTower("Wizard");
        Hide();
    }

    public void ClickPoison()
    {
        if (!CanClick()) return;
        TowerPlacer.instance.SetSelectedTower("Poison");
        Hide();
    }
}
