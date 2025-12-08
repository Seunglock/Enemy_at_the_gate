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

    [Header("Prices")]
    public int arrowPrice = 50;
    public int mortarPrice = 70;
    public int wizardPrice = 90;
    public int poisonPrice = 100;

    // 내부 변수
    private Tile selectedTile;
    private string selectedTower = "";

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        // 마우스 클릭 감지
        if (Input.GetMouseButtonDown(0))
        {
            
            if (EventSystem.current.IsPointerOverGameObject()) return;

            if (ShopController.instance != null && ShopController.instance.IsPlacementMode()) return;

            DetectTileClick2D();
        }
    }

    void DetectTileClick2D()
    {
        // 마우스 위치를 월드 좌표로 변환
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        // 레이저 발사 (2D Physics)
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        if (hit.collider != null)
        {
           
            Debug.Log("클릭된 물체: " + hit.collider.name);

            // 타일 스크립트 가져오기
            Tile tileScript = hit.collider.GetComponent<Tile>();

            if (tileScript != null)
            {
                if (tileScript.isOccupied)
                {
                    Debug.Log("이미 건물이 있습니다.");
                    return;
                }

                // 타일 정상 인식 -> UI 열기
                OpenUI(tileScript);
            }
        }
    }

    public void OpenUI(Tile tile)
    {
        selectedTile = tile;
        if (ui != null) ui.Show();
    }

    // TowerSelectUI에서 호출하는 함수
    public void SetSelectedTower(string towerName)
    {
        selectedTower = towerName;
        PlaceTower(); // 타워 이름 설정 후 바로 건설 시도
    }

    void PlaceTower()
    {
        if (selectedTile == null) return;

        GameObject prefab = null;
        int cost = 0;

        // 이름에 따라 프리팹과 가격 설정
        switch (selectedTower)
        {
            case "Arrow": prefab = arrowPrefab; cost = arrowPrice; break;
            case "Mortar": prefab = mortarPrefab; cost = mortarPrice; break;
            case "Wizard": prefab = wizardPrefab; cost = wizardPrice; break;
            case "Poison": prefab = poisonPrefab; cost = poisonPrice; break;
        }

        if (prefab == null)
        {
            Debug.LogError("타워 프리팹이 설정되지 않았습니다!");
            return;
        }

        // 돈 확인 (SystemController가 있을 때만)
        if (SystemController.instance != null)
        {
            if (!SystemController.instance.TrySpendGold(cost))
            {
                Debug.Log("골드가 부족합니다.");
                return;
            }
        }

        // 타워 생성
        Instantiate(prefab, selectedTile.transform.position, Quaternion.identity);

        // 타일 상태 업데이트
        selectedTile.isOccupied = true;

        // 선택 초기화
        selectedTile = null;
        selectedTower = "";
    }
}