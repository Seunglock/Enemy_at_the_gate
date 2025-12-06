using System.Collections;
using UnityEngine;

public class PhantomBoss : MonoBehaviour
{
    public float auraRadius = 3f;
    public float speedBoostMultiplier = 1.3f;
    public float boostDuration = 2f;
    public float auraInterval = 3f;

    private Enemy selfEnemy;

    void Start()
    {
        selfEnemy = GetComponent<Enemy>();
        StartCoroutine(AuraRoutine());
    }

    IEnumerator AuraRoutine()
    {
        while (true)
        {
            ApplyAura();
            yield return new WaitForSeconds(auraInterval);
        }
    }

    void ApplyAura()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, auraRadius);

        foreach (Collider2D hit in hits)
        {
            if (!hit.CompareTag("Enemy"))
                continue;

            Enemy e = hit.GetComponent<Enemy>();
            if (e == null)
                continue;

            if (e == selfEnemy)
                continue;

            e.ApplyTemporarySpeedBoost(speedBoostMultiplier, boostDuration);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, auraRadius);
    }
}
