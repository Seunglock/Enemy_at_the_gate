using System.Collections.Generic;
using UnityEngine;

public class MortarTower : MonoBehaviour
{
    [Header("Tower Settings")]
    public float attackRange = 8f;        // 고정 사거리
    public float attackCooldown = 2.5f;   // 기본 쿨타임

    [Header("Damage Settings")]
    public float directDamage = 30f;      // 직격 데미지
    public float splashDamage = 15f;      // 스플래시 데미지
    public float splashRadius = 2f;       // 스플래시 범위 (고정)

    [Header("Projectile")]
    public Transform firePoint;
    public GameObject mortarShellPrefab;

    // ★ 원본 스탯 저장용 변수
    private float baseCooldown;
    private float baseDirectDamage;
    private float baseSplashDamage;

    private float cooldownTimer = 0f;
    private List<Enemy> enemiesInRange = new List<Enemy>();

    void Start()
    {
        // 게임 시작 시점의 기본값 저장
        baseCooldown = attackCooldown;
        baseDirectDamage = directDamage;
        baseSplashDamage = splashDamage;
    }

    void Update()
    {
        // ---------------------------------------------------------
        // 증강체 적용 (데미지 & 공격속도)
        // ---------------------------------------------------------

        // 1. 데미지 갱신 (직격 & 스플래시 모두 적용)
        float damageMult = SystemController.instance.towerDamageMultiplier;
        directDamage = baseDirectDamage * damageMult;
        splashDamage = baseSplashDamage * damageMult;

        // 2. 쿨타임 갱신 (SpeedUp 적용)
        // 실제 적용될 쿨타임 계산
        float currentCooldown = baseCooldown * SystemController.instance.towerFireRateMultiplier;

        // ---------------------------------------------------------

        cooldownTimer -= Time.deltaTime;

        Enemy target = GetFrontEnemy();

        // attackCooldown 대신 계산된 currentCooldown 사용
        if (target != null && cooldownTimer <= 0f)
        {
            Shoot(target);
            cooldownTimer = currentCooldown; // ★ 갱신된 쿨타임 적용
        }
    }

    void Shoot(Enemy target)
    {
        GameObject shell = Instantiate(mortarShellPrefab, firePoint.position, Quaternion.identity);
        MortarShell ms = shell.GetComponent<MortarShell>();

        // 강화된 데미지 수치 전달
        ms.directDamage = directDamage;
        ms.splashDamage = splashDamage;
        ms.splashRadius = splashRadius; // 범위는 고정

        ms.SetTarget(target.transform);
    }

    // --- 아래는 기존과 동일 ---

    Enemy GetFrontEnemy()
    {
        if (enemiesInRange.Count == 0) return null;
        // null인 적(죽은 적) 청소
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