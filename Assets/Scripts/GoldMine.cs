using UnityEngine;
using System.Collections;

public class GoldMine : MonoBehaviour
{
    public int goldPerCycle = 5;         // 5ÃÊ¿¡ ÇÑ ¹ø µé¾î¿À´Â °ñµå
    public float cycleTime = 5f;         // ¼ö±Þ ÁÖ±â (5ÃÊ)

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

            // °ñµå Áö±Þ
            SystemController.instance.AddGold(goldPerCycle);

            Debug.Log("±Ý±¤ °ñµå È¹µæ: +" + goldPerCycle);
        }
    }

    void OnDestroy()
    {
        // ±Ý±¤ÀÌ ÆÄ±«µÇ¸é ÀÚµ¿ ¼ö±Þ ¸ØÃã
        if (goldRoutine != null)
            StopCoroutine(goldRoutine);
    }
}
