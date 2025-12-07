using System.Collections.Generic;
using UnityEngine;

public class AlchemistTower : MonoBehaviour
{
    [Header("Tower Settings")]
    public float attackRange = 6f;      // 고정 사거리
    public float attackCooldown = 1.5f; // 기본 쿨타임

    [Header("Damage Settings")]
    public float percentDamage = 0.05f; // 적 체력 비례 데미지 (5%)

    [Header("Projectile")]
    public Transform firePoint;
    public GameObject poisonBottlePrefab;

    // ★ 원본 스탯 저장용
    private float basePercentDamage;
    private float baseCooldown;

    private float cooldownTimer = 0f;
    private List<Enemy> enemiesInRange = new List<Enemy>();

    void Start()
    {
        // 원본 저장
        basePercentDamage = percentDamage;
        baseCooldown = attackCooldown;
    }

    void Update()
    {
        // ---------------------------------------------------------
        // 증강체 적용 (데미지 & 공격속도)
        // ---------------------------------------------------------

        // 1. 퍼센트 데미지 강화
        // 예: 기본 0.05 * 1.2(20%증가) = 0.06 (6% 데미지)
        percentDamage = basePercentDamage * SystemController.instance.towerDamageMultiplier;

        // 2. 쿨타임 갱신 (SpeedUp 적용)
        float currentCooldown = baseCooldown * SystemController.instance.towerFireRateMultiplier;

        // ---------------------------------------------------------

        cooldownTimer -= Time.deltaTime;

        Enemy target = GetFrontEnemy();

        if (target != null && cooldownTimer <= 0f)
        {
            ThrowBottle(target);
            cooldownTimer = currentCooldown; // ★ 갱신된 쿨타임 적용
        }
    }

    void ThrowBottle(Enemy target)
    {
        GameObject bottle = Instantiate(poisonBottlePrefab, firePoint.position, Quaternion.identity);
        PoisonBottle pb = bottle.GetComponent<PoisonBottle>();

        pb.percentDamage = percentDamage;   // 강화된 비례 데미지 전달
        pb.SetTarget(target.transform);
    }

    // --- 아래는 기존과 동일 ---

    Enemy GetFrontEnemy()
    {
        if (enemiesInRange.Count == 0) return null;
        enemiesInRange.RemoveAll(e => e == null);

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
            if (e != null && !enemiesInRange.Contains(e))
                enemiesInRange.Add(e);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy e = other.GetComponent<Enemy>();
            if (e != null)
                enemiesInRange.Remove(e);
        }
    }
}