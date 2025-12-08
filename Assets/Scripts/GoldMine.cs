using UnityEngine;
using System.Collections;

public class GoldMine : MonoBehaviour
{
    public int goldPerCycle = 5;         // 5초에 한 번 들어오는 골드
    public float cycleTime = 5f;         // 수급 주기 (5초)

    private Coroutine goldRoutine;

    void Start()
    {
        goldRoutine = StartCoroutine(GoldIncomeRoutine());
    }

    IEnumerator GoldIncomeRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(cycleTime);

            // 골드 지급
            SystemController.instance.AddGold(goldPerCycle);
        }
    }

    void OnDestroy()
    {
        // 금광이 파괴되면 자동 수급 멈춤
        if (goldRoutine != null)
            StopCoroutine(goldRoutine);
    }
}
