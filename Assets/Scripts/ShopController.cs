using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [Header("Shop Prices")]
    public int expCost = 40;
    public int buffCost = 300;
    public int ultimateCost = 500;
    public int barricadeCost = 100;

    [Header("Shop Values")]
    public int expAmount = 40;       // 구매 시 얻는 경험치
    public float buffDuration = 10f; // 버프 지속 시간
    public float ultimateDamage = 100f; // 광역 데미지

    public GameObject barricadePrefab;   // 소환할 바리케이드 프리팹
    public Transform barricadeSpawnPoint; // 소환될 "지정한 위치" (빈 오브젝트)
    public float barricadeLifeTime = 5f; // 유지시간
    private bool isBarricadeActive = false; //바리케이드 중복방지용

    public GameObject towerSelectPanel;     // 타워 선택 패널
    public GameObject[] uiImagesToHide;     // 패널이 켜질 때 숨길 UI 이미지들 (배열)

    public void SetTowerSelectMode(bool isOpen)
    {
        // 1. 타워 선택 패널 활성화/비활성화
        if (towerSelectPanel != null)
        {
            towerSelectPanel.SetActive(isOpen);
        }

        // 2. 특정 이미지들 반대로 설정 (패널이 켜지면 -> 이미지는 꺼짐)
        if (uiImagesToHide != null)
        {
            foreach (GameObject obj in uiImagesToHide)
            {
                if (obj != null)
                {
                    obj.SetActive(!isOpen); // isOpen의 반대 상태로 설정
                }
            }
        }
    }

    // 1. 경험치 구매
    public void BuyExp()
    {
        if (SystemController.instance.TrySpendGold(expCost))
        {
            SystemController.instance.AddExp(expAmount);
            Debug.Log("경험치 구매 완료!");
        }
    }

    // 2. 타워 버프 (공격력/공속 증가)
    public void BuyTowerBuff()
    {
        if (SystemController.instance.TrySpendGold(buffCost))
        {
            // 씬에 있는 태그가 "Tower"인 모든 오브젝트 찾기
            GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

            foreach (GameObject t in towers)
            {
                // 각 타워 스크립트에 접근해서 버프 함수 호출
                // (각 타워 스크립트에 ActivateBuff 함수가 있어야 함)

                // 예시: SendMessage를 쓰면 타워 종류 상관없이 함수만 있으면 호출됨
                t.SendMessage("ActivateBuff", buffDuration, SendMessageOptions.DontRequireReceiver);
            }
            Debug.Log($"모든 타워 버프 발동! ({buffDuration}초)");
        }
    }

    // 3. 광역 필살기 (화면 전체 적 타격)
    public void BuyUltimate()
    {
        if (SystemController.instance.TrySpendGold(ultimateCost))
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject e in enemies)
            {
                Enemy enemyScript = e.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(ultimateDamage);
                }
            }
            Debug.Log("광역 필살기 시전!");
        }
    }

    public void BuyBarricade()
    {
        // 1. 이미 바리케이드가 있다면 구매 불가 (선택사항)
        if (isBarricadeActive)
        {
            Debug.Log("이미 바리케이드가 설치되어 있습니다!");
            return;
        }

        // 2. 골드 지불 확인
        if (SystemController.instance.TrySpendGold(barricadeCost))
        {
            // 3. 지정된 위치(spawnPoint)가 없으면 에러 방지
            if (barricadeSpawnPoint == null)
            {
                Debug.LogError("바리케이드 소환 위치(Spawn Point)가 지정되지 않았습니다!");
                return;
            }

            // 4. 소환 및 타이머 시작
            StartCoroutine(BarricadeRoutine());
        }
    }

    // 바리케이드 생성 -> 대기 -> 삭제 코루틴
    IEnumerator BarricadeRoutine()
    {
        isBarricadeActive = true;

        // 생성
        GameObject barricade = Instantiate(barricadePrefab, barricadeSpawnPoint.position, Quaternion.identity);
        Debug.Log("바리케이드 설치됨!");

        // 5초 대기
        yield return new WaitForSeconds(barricadeLifeTime);

        // 삭제
        if (barricade != null)
        {
            Destroy(barricade);
        }

        isBarricadeActive = false;
        Debug.Log("바리케이드 사라짐!");
    }
}