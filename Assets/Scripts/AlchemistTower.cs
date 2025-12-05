using System.Collections.Generic;
using UnityEngine;

public class AlchemistTower : MonoBehaviour
{
    [Header("Tower Settings")]
    public float attackRange = 6f;
    public float attackCooldown = 1.5f;

    [Header("Damage Settings")]
    public float percentDamage = 0.05f;   // 적 체력 비례 데미지

    [Header("Projectile")]
    public Transform firePoint;
    public GameObject poisonBottlePrefab;

    private float basePercentDamage;      // 기본 비례 데미지 저장
    private float cooldownTimer = 0f;

    private List<Enemy> enemiesInRange = new List<Enemy>();


    void Start()
    {
        // 원본 저장
        basePercentDamage = percentDamage;
    }


    void Update()
    {
        // -------------------------------
        // 전역 강화 배율 적용
        // -------------------------------
        float multiplier = SystemController.instance.towerDamageMultiplier;
        percentDamage = basePercentDamage * multiplier;
        // -------------------------------

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

        pb.percentDamage = percentDamage;   // 강화된 비례 데미지
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
