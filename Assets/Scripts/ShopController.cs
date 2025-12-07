using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public static ShopController instance;

    [Header("Shop Prices")]
    public int expCost = 40;
    public int buffCost = 300;
    public int ultimateCost = 500;
    public int barricadeCost = 100;

    [Header("Shop Values")]
    public int expAmount = 40;
    public float buffDuration = 10f;
    public float ultimateDamage = 100f;

    [Header("Barricade Settings")]
    public GameObject barricadePrefab;
    public BarricadePlacer[] barricadeSpots;
    public float barricadeLifeTime = 5f;

    private bool isPlacementMode = false;

    [Header("UI")]
    public GameObject towerSelectPanel;
    public GameObject[] uiImagesToHide;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    // ★ 여기가 문제가 생겼던 부분입니다. 이 함수 하나만 있어야 합니다.
    public void SetTowerSelectMode(bool isOpen)
    {
        // 1. 함수가 호출은 되는지 확인 (로그 확인용)
        Debug.Log($"[진단] SetTowerSelectMode 호출됨! 상태(isOpen): {isOpen}");

        // 2. 패널 연결 확인
        if (towerSelectPanel != null)
        {
            towerSelectPanel.SetActive(isOpen);
        }
        else
        {
            Debug.LogError(" [오류] Tower Select Panel이 인스펙터에 연결되지 않았습니다!");
        }

        // 3. 숨길 이미지 리스트 확인
        if (uiImagesToHide == null)
        {
            Debug.LogError(" [오류] uiImagesToHide 리스트 자체가 없습니다 (Null).");
            return;
        }

        if (uiImagesToHide.Length == 0)
        {
            Debug.LogError(" [오류] uiImagesToHide 리스트 사이즈가 0입니다. 인스펙터에서 Size를 늘리고 오브젝트를 넣으세요.");
            return;
        }

        // 4. 실제 숨김 처리
        for (int i = 0; i < uiImagesToHide.Length; i++)
        {
            GameObject obj = uiImagesToHide[i];

            if (obj == null)
            {
                Debug.LogError($"[오류] 리스트의 {i}번째 칸이 비어있습니다(None). 오브젝트를 드래그해서 넣으세요.");
            }
            else
            {
                // isOpen이 true(창이 열림)면 -> 활성상태는 false(숨김)가 되어야 함
                bool targetState = !isOpen;
                obj.SetActive(targetState);
                // Debug.Log($" [성공] {obj.name} 오브젝트를 {targetState} 상태로 변경했습니다.");
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

    // 2. 타워 버프
    public void BuyTowerBuff()
    {
        if (SystemController.instance.TrySpendGold(buffCost))
        {
            GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
            foreach (GameObject t in towers)
            {
                t.SendMessage("ActivateBuff", buffDuration, SendMessageOptions.DontRequireReceiver);
            }
            Debug.Log($"모든 타워 버프 발동! ({buffDuration}초)");
        }
    }

    // 3. 광역 필살기
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

    // 4. 바리케이드 구매 버튼
    public void BuyBarricade()
    {
        isPlacementMode = !isPlacementMode;

        if (barricadeSpots != null)
        {
            foreach (BarricadePlacer spot in barricadeSpots)
            {
                if (spot != null) spot.SetHighlight(isPlacementMode);
            }
        }

        if (isPlacementMode) Debug.Log("바리케이드 설치 모드: 반짝이는 위치를 클릭하세요.");
        else Debug.Log("설치 모드 취소");
    }

    // 5. 실제 설치 시도
    public void AttemptInstallBarricade(BarricadePlacer placer)
    {
        if (!isPlacementMode) return;

        if (SystemController.instance.TrySpendGold(barricadeCost))
        {
            placer.InstallBarricade(barricadePrefab, barricadeLifeTime);
            Debug.Log("바리케이드 설치 완료!");

            isPlacementMode = false;

            if (barricadeSpots != null)
            {
                foreach (BarricadePlacer spot in barricadeSpots)
                {
                    if (spot != null) spot.SetHighlight(false);
                }
            }
        }
        else
        {
            Debug.Log("골드가 부족합니다!");
            isPlacementMode = false;
            foreach (BarricadePlacer spot in barricadeSpots)
            {
                if (spot != null) spot.SetHighlight(false);
            }
        }
    }
    public bool IsPlacementMode()
    {
        return isPlacementMode;
    }
}