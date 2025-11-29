using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isOccupied = false;

    private void OnMouseDown()
    {
        Debug.Log("Cicked");
        if (isOccupied) return;

        TowerPlacer.instance.OpenUI(this);
    }
}

