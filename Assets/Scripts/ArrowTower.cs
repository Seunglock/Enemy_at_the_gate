using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTower : MonoBehaviour
{
    [Header("Tower Settings")]
    public float attackRange = 8f;     // 고정 사거리
    public float attackCooldown = 1f;
    public float damage = 10f;

    [Header("Projectile")]
    public Transform firePoint;
    public GameObject arrowPrefab;

    // 원본 스탯 저장 (쿨타임, 데미지 계산용)
    private float baseCooldown;
    private float baseDamage;

    private float cooldownTimer = 0f;
    private List<Enemy> enemiesInRange = new List<Enemy>();

    void Start()
    {
        // 게임 시작 시점의 기본값 저장
        baseCooldown = attackCooldown;
        baseDamage = damage;

        // 사거리(Collider Radius)는 인스펙터 설정값 그대로 유지되므로 코드 제어 불필요
    }

    void Update()
    {
        // ---------------------------------------------------------
        // 증강체 적용 (사거리 제외)
        // ---------------------------------------------------------

        // 1. 데미지 갱신
        damage = baseDamage * SystemController.instance.towerDamageMultiplier;

        // 2. 쿨타임 갱신 (공격속도)
        float currentCooldown = baseCooldown * SystemController.instance.towerFireRateMultiplier;

        // (사거리 갱신 코드 삭제됨)

        // ---------------------------------------------------------

        cooldownTimer -= Time.deltaTime;

        Enemy target = GetFrontEnemy();

        if (target != null && cooldownTimer <= 0f)
        {
            Shoot(target);
            cooldownTimer = currentCooldown; // 갱신된 쿨타임 적용
        }
    }

    void Shoot(Enemy target)
    {
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        arrowScript.SetTarget(target.transform);
        arrowScript.damage = damage;
    }

    // ... (나머지 GetFrontEnemy, OnTrigger 함수들은 그대로) ...
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
            enemiesInRange.Remove(e);
        }
    }
}