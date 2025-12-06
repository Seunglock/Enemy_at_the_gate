using System.Collections;
using UnityEngine;

public class ElderSkeletonBoss : MonoBehaviour
{
    public float chargeMultiplier = 3f;
    public float chargeDuration = 0.4f;
    public float chargeCooldown = 5f;

    private Enemy selfEnemy;

    void Start()
    {
        selfEnemy = GetComponent<Enemy>();
        StartCoroutine(ChargeRoutine());
    }

    IEnumerator ChargeRoutine()
    {
        while (true)
        {
            selfEnemy.ApplyTemporarySpeedBoost(chargeMultiplier, chargeDuration);
            yield return new WaitForSeconds(chargeCooldown);
        }
    }
}
