using System.Collections.Generic;
using UnityEngine;

public class MagicTower : MonoBehaviour
{
    [Header("Tower Settings")]
    public float attackRange = 6f; // 고정 사거리
    public float attackCooldown = 1.2f;
    public float damage = 2f;

    [Header("Projectile")]
    public Transform firePoint;
    public GameObject iceMagicPrefab;

    private float baseCooldown;
    private float baseDamage;

    private float cooldownTimer = 0f;
    private List<Enemy> enemiesInRange = new List<Enemy>();

    void Start()
    {
        baseCooldown = attackCooldown;
        baseDamage = damage;
    }

    void Update()
    {
        // ---------------------------------------------------------
        // 증강체 적용 (사거리 제외)
        // ---------------------------------------------------------

        // 1. 데미지 갱신
        damage = baseDamage * SystemController.instance.towerDamageMultiplier;

        // 2. 쿨타임 갱신
        float currentCooldown = baseCooldown * SystemController.instance.towerFireRateMultiplier;

        // (사거리 갱신 코드 삭제됨)

        // ---------------------------------------------------------

        cooldownTimer -= Time.deltaTime;

        Enemy target = GetFrontEnemy();

        if (target != null && cooldownTimer <= 0f)
        {
            Shoot(target);
            cooldownTimer = currentCooldown;
        }
    }

    void Shoot(Enemy target)
    {
        GameObject proj = Instantiate(iceMagicPrefab, firePoint.position, Quaternion.identity);
        IceMagic ice = proj.GetComponent<IceMagic>();
        ice.damage = damage;
        ice.SetTarget(target.transform);
    }

    // ... (나머지 기존 유지) ...
    Enemy GetFrontEnemy()
    {
        if (enemiesInRange.Count == 0) return null;
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
            if (e != null && !enemiesInRange.Contains(e)) enemiesInRange.Add(e);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy e = other.GetComponent<Enemy>();
            if (e != null && enemiesInRange.Contains(e)) enemiesInRange.Remove(e);
        }
    }
}