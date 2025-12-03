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

    //타워 가격
    public int arrowPrice = 50;
    public int mortarPrice = 70;
    public int wizardPrice = 90;
    public int poisonPrice = 100;

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
        int cost = 0;

        switch (selectedTower)
        {
            case "Arrow":
                prefab = arrowPrefab;
                cost = arrowPrice;
                break;

            case "Mortar":
                prefab = mortarPrefab;
                cost = mortarPrice;
                break;

            case "Wizard":
                prefab = wizardPrefab;
                cost = wizardPrice;
                break;

            case "Poison":
                prefab = poisonPrefab;
                cost = poisonPrice;
                break;
        }

        if (prefab == null)
        {
            Debug.LogError("타워 프리팹이 연결되지 않음!");
            return;
        }

        if (SystemController.instance == null || !SystemController.instance.TrySpendGold(cost))
        {
            Debug.Log("건설 실패: 골드가 부족합니다.");
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
