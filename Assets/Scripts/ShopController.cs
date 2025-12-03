using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [Header("Shop Prices")]
    public int expCost = 40;
    public int buffCost = 300;
    public int ultimateCost = 500;

    [Header("Shop Values")]
    public int expAmount = 40;       // 구매 시 얻는 경험치
    public float buffDuration = 10f; // 버프 지속 시간
    public float ultimateDamage = 100f; // 광역 데미지

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
}