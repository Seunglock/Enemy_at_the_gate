using UnityEngine;
using UnityEngine.EventSystems;

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

    void DetectTileClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.GetComponent<BarricadePlacer>() != null)
            {
                return;
            }

            // 클릭한 물체에 'Tile' 스크립트가 없다면 무시
            Tile tileScript = hit.transform.GetComponent<Tile>();

            if (tileScript != null)
            {
                if (tileScript.isOccupied) return; // 이미 타워 있으면 무시

                OpenUI(tileScript); // 순수 타일일 때만 UI 오픈
            }
        }
    }

    // 타일 클릭 시 호출
    public void OpenUI(Tile tile)
    {
        selectedTile = tile;
        ui.Show();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 1. UI 클릭 방지
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;

            // ★ [추가] 2. 바리케이드 설치 모드라면? -> 타워 로직 작동 금지!
            if (ShopController.instance != null && ShopController.instance.IsPlacementMode())
            {
                // "지금은 바리케이드 설치 중이니까 타워 창 안 띄울게" 하고 종료
                return;
            }

            // 3. 타일 감지 시작
            DetectTileClick();
        }
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
