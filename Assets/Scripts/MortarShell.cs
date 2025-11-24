using UnityEngine;

public class MortarShell : MonoBehaviour
{
    public float speed = 5f;

    public float directDamage = 3f;
    public float splashDamage = 1.5f;
    public float splashRadius = 2f;

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

        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        if (Vector2.Distance(transform.position, target.position) < 0.3f)
        {
            Explode();
        }
    }

    void Explode()
    {
        // --------------------------
        // 1) 범위 안의 적 모두 찾기
        // --------------------------
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, splashRadius);

        // --------------------------
        // 2) 데미지 처리 (메인도 포함)
        // --------------------------
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Enemy e = hit.GetComponent<Enemy>();

                if (e != null)
                {
                    if (e.transform == target)
                        e.TakeDamage(directDamage);   // 직격 대상
                    else
                        e.TakeDamage(splashDamage);   // 주변 대상
                }
            }
        }

        // --------------------------
        // 3) 마지막에 포탄 제거
        // --------------------------
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, splashRadius);
    }
}
