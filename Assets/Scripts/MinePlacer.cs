using UnityEngine;

public class MinePlacer : MonoBehaviour
{
    [Header("Settings")]
    public GameObject goldMinePrefab; // 실제 생성될 금광 프리팹
    public int buildCost = 100;       // 건설 비용

    // 마우스로 이 오브젝트를 클릭했을 때 실행됨
    private void OnMouseDown()
    {
        // 1. 이미 금광이 건설된 상태인지 체크 (혹은 UI와 겹침 방지)
        // (EventSystem.current.IsPointerOverGameObject() 체크가 필요할 수도 있음)

        // 2. 골드가 충분한지 확인하고 지불 시도
        if (SystemController.instance.TrySpendGold(buildCost))
        {
            BuildMine();
        }
        else
        {
            Debug.Log($"골드가 부족합니다! (필요: {buildCost}, 보유: {SystemController.instance.gold})");
            // 여기에 "돈이 부족해!" 같은 UI 텍스트를 띄워주면 더 좋습니다.
        }
    }

    void BuildMine()
    {
        // 3. 현재 위치에 금광 생성
        Instantiate(goldMinePrefab, transform.position, Quaternion.identity);

        Debug.Log("금광 건설 완료!");

        // 4. 건설 부지(이 오브젝트)는 이제 필요 없으니 삭제
        Destroy(gameObject);
    }
}