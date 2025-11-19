using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 1f;

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
        // 1) 회전: 화살촉을 적 방향으로 보냄
        // --------------------------
        Vector3 dir = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // --------------------------
        // 2) 이동: 적 방향으로 전진
        // --------------------------
        transform.position += transform.up * speed * Time.deltaTime;

        // --------------------------
        // 3) 적과 충돌하면 데미지
        // --------------------------
        if (Vector2.Distance(transform.position, target.position) < 0.3f)
        {
            target.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
