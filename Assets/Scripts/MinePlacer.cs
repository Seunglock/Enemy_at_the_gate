using UnityEngine;

public class MinePlacer : MonoBehaviour
{
    public GameObject goldMinePrefab; // 실제 생성될 금광 프리팹
    public int buildCost = 100;       // 건설 비용

    // 마우스로 이 오브젝트를 클릭했을 때 실행됨
    private void OnMouseDown()
    {

        // 골드가 충분한지 확인하고 지불 시도
        if (SystemController.instance.TrySpendGold(buildCost))
        {
            BuildMine();
        }
        else
        {
            Debug.Log($"골드가 부족합니다! (필요: {buildCost}, 보유: {SystemController.instance.gold})");
        }
    }

    void BuildMine()
    {
        // 현재 위치에 금광 생성
        Instantiate(goldMinePrefab, transform.position, Quaternion.identity);

        // 건설 부지(이 오브젝트)는 이제 필요 없으니 삭제
        Destroy(gameObject);
    }
}