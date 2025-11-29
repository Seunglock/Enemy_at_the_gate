using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    public static TowerPlacer instance;

    [Header("UI")]
    public TowerSelectUI ui;

    [Header("Tower Prefabs")]
    public GameObject arrowPrefab;
    public GameObject mortarPrefab;
    public GameObject wizardPrefab;
    public GameObject poisonPrefab;

    private Tile selectedTile;
    private string selectedTower = "";

    void Awake()
    {
        instance = this;
    }

    // 타일 클릭 시 호출
    public void OpenUI(Tile tile)
    {
        selectedTile = tile;
        ui.Show();
    }

    // 어떤 타워를 선택했는지 받음
    public void SetSelectedTower(string towerName)
    {
        selectedTower = towerName;
        PlaceTower();
    }

    void PlaceTower()
    {
        if (selectedTile == null) return;

        GameObject prefab = null;

        switch (selectedTower)
        {
            case "Arrow":
                prefab = arrowPrefab;
                break;

            case "Mortar":
                prefab = mortarPrefab;
                break;

            case "Wizard":
                prefab = wizardPrefab;
                break;

            case "Poison":
                prefab = poisonPrefab;
                break;
        }

        if (prefab == null)
        {
            Debug.LogError("타워 프리팹이 연결되지 않음!");
            return;
        }

        // 타워 설치
        Instantiate(prefab, selectedTile.transform.position, Quaternion.identity);

        // 타일 점유 처리
        selectedTile.isOccupied = true;

        // 선택 초기화
        selectedTile = null;
        selectedTower = "";
    }
}
