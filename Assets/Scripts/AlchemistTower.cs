using System.Collections.Generic;
using UnityEngine;

public class AlchemistTower : MonoBehaviour
{
    [Header("Tower Settings")]
    public float attackRange = 6f;
    public float attackCooldown = 1.5f;

    [Header("Damage Settings")]
    public float percentDamage = 0.05f;   // 적 최대체력의 5% 데미지

    [Header("Projectile")]
    public Transform firePoint;
    public GameObject poisonBottlePrefab;

    private float cooldownTimer = 0f;
    private List<Enemy> enemiesInRange = new List<Enemy>();

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        Enemy target = GetFrontEnemy();

        if (target != null && cooldownTimer <= 0f)
        {
            ThrowBottle(target);
            cooldownTimer = attackCooldown;
        }
    }

    void ThrowBottle(Enemy target)
    {
        GameObject bottle = Instantiate(poisonBottlePrefab, firePoint.position, Quaternion.identity);

        PoisonBottle pb = bottle.GetComponent<PoisonBottle>();

        pb.percentDamage = percentDamage;
        pb.SetTarget(target.transform);
    }

    Enemy GetFrontEnemy()
    {
        if (enemiesInRange.Count == 0)
            return null;

        Enemy front = enemiesInRange[0];
        foreach (Enemy e in enemiesInRange)
        {
            if (e != null && e.transform.position.z > front.transform.position.z)
                front = e;
        }
        return front;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy e = other.GetComponent<Enemy>();
            if (e != null && !enemiesInRange.Contains(e))
                enemiesInRange.Add(e);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy e = other.GetComponent<Enemy>();
            if (e != null && enemiesInRange.Contains(e))
                enemiesInRange.Remove(e);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy e = other.GetComponent<Enemy>();
            if (e != null && !enemiesInRange.Contains(e))
                enemiesInRange.Add(e);
        }
    }
}
