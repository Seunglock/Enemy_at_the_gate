using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMagic : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 1f;

    [Header("Splash Settings")]
    public float splashRadius = 1.5f;     // 스플래시 범위
    public float splashDamage = 0.5f;     // 스플래시 데미지

    [Header("Slow Settings")]
    public float slowPercent = 0.5f;      // 50% 느려짐
    public float slowDuration = 2f;       // 2초 동안

    private Transform target;

    public void SetTarget(Transform t)
    {
        target = t;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // --------------------------
        // 1) 회전
        // --------------------------
        Vector3 dir = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // --------------------------
        // 2) 이동
        // --------------------------
        transform.position += transform.up * speed * Time.deltaTime;

        // --------------------------
        // 3) 적 도달 체크
        // --------------------------
        if (Vector2.Distance(transform.position, target.position) < 0.3f)
        {
            HitTarget();
        }
    }

    void HitTarget()
    {
        // 1) 메인 타겟에게 데미지
        Enemy mainEnemy = target.GetComponent<Enemy>();
        if (mainEnemy != null)
        {
            mainEnemy.TakeDamage(damage);
            mainEnemy.ApplySlow(slowPercent, slowDuration);
        }

        // 2) 스플래시 데미지
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, splashRadius);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Enemy e = hit.GetComponent<Enemy>();

                if (e != null && e != mainEnemy)
                {
                    e.TakeDamage(splashDamage);
                    e.ApplySlow(slowPercent, slowDuration);
                }
            }
        }

        Destroy(gameObject);
    }

    // 시각적 debug용
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, splashRadius);
    }
}
