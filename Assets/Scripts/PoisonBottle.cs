using UnityEngine;

public class PoisonBottle : MonoBehaviour
{
    public float speed = 7f;
    public float percentDamage = 0.05f;  // 5% 체력 비례 데미지

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

        if (Vector2.Distance(transform.position, target.position) < 0.25f)
        {
            ApplyPercentDamage();
        }
    }

    void ApplyPercentDamage()
    {
        Enemy e = target.GetComponent<Enemy>();

        if (e != null)
            e.TakePercentDamage(percentDamage);

        Destroy(gameObject);
    }
}
