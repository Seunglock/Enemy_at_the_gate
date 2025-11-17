using System.Collections;
using System.Collections.Generic;
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

        // 타겟 방향으로 이동
        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        // 일정 거리 이하이면 타격
        if (Vector3.Distance(transform.position, target.position) < 0.3f)
        {
            target.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}