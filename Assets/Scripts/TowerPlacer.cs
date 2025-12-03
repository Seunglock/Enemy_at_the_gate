/*using UnityEngine;

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
*/

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

    // 현재 선택된 타일 (이게 null이면 설치가 안 됨)
    [SerializeField] // 인스펙터에서 확인하려고 추가함
    private Tile selectedTile;
    private string selectedTower = "";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Debug.Log("[System] TowerPlacer 초기화 완료 (Instance 설정됨)");
        }
        else
        {
            Debug.LogWarning("[System] TowerPlacer가 두 개 이상 존재합니다! 중복된 오브젝트를 확인하세요.");
        }
    }

    // 타일 클릭 시 호출되어야 함
    public void OpenUI(Tile tile)
    {
        if (tile == null)
        {
            Debug.LogError(" [오류] OpenUI에 전달된 타일이 null입니다.");
            return;
        }

        selectedTile = tile;
        Debug.Log($"[System] 타일 선택됨: {tile.name} (이제 UI를 엽니다)");
        ui.Show();
    }

    // UI 버튼 클릭 시 호출됨
    public void SetSelectedTower(string towerName)
    {
        Debug.Log($"[System] 타워 선택 요청 받음: {towerName}");
        selectedTower = towerName;
        PlaceTower();
    }

    void PlaceTower()
    {
        // 1. 타일 선택 여부 확인 (가장 흔한 실수)
        if (selectedTile == null)
        { 
            Debug.LogError("[설치 실패] 선택된 타일(selectedTile)이 없습니다! \n해결법: UI 버튼을 누르기 전에 맵의 타일을 먼저 클릭해서 OpenUI()가 호출되게 해야 합니다.");
            return;
        }

        GameObject prefab = null;

        // 2. 이름에 맞는 프리팹 찾기
        switch (selectedTower)
        {
            case "Arrow": prefab = arrowPrefab; break;
            case "Mortar": prefab = mortarPrefab; break;
            case "Wizard": prefab = wizardPrefab; break;
            case "Poison": prefab = poisonPrefab; break;
            default:
                Debug.LogError($" [설치 실패] 알 수 없는 타워 이름입니다: {selectedTower}");
                return;
        }

        // 3. 프리팹 연결 여부 확인
        if (prefab == null)
        {
            Debug.LogError($" [설치 실패] '{selectedTower}' 타워의 프리팹이 Inspector에 연결되지 않았습니다! TowerPlacer 컴포넌트를 확인하세요.");
            return;
        }

        // 4. 설치 실행
        Instantiate(prefab, selectedTile.transform.position, Quaternion.identity);
        Debug.Log($" [성공] {selectedTower} 타워가 {selectedTile.name} 위치에 설치되었습니다!");

        // 5. 마무리
        selectedTile.isOccupied = true;
        selectedTile = null;
        selectedTower = "";
    }
}