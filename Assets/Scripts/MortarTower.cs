using System.Collections.Generic;
using UnityEngine;

public class MortarTower : MonoBehaviour
{
    [Header("Tower Settings")]
    public float attackRange = 8f;
    public float attackCooldown = 2.5f;

    [Header("Damage Settings")]
    public float directDamage = 3f;      // 직격 데미지
    public float splashDamage = 1.5f;    // 스플래쉬 데미지
    public float splashRadius = 2f;      // 스플래쉬 범위

    [Header("Projectile")]
    public Transform firePoint;
    public GameObject mortarShellPrefab;

    private float cooldownTimer = 0f;
    private List<Enemy> enemiesInRange = new List<Enemy>();

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        Enemy target = GetFrontEnemy();

        if (target != null && cooldownTimer <= 0f)
        {
            Shoot(target);
            cooldownTimer = attackCooldown;
        }
    }

    void Shoot(Enemy target)
    {
        GameObject shell = Instantiate(mortarShellPrefab, firePoint.position, Quaternion.identity);

        MortarShell ms = shell.GetComponent<MortarShell>();

        // --- 타워에서 설정한 데미지 값을 투사체로 전달 ---
        ms.directDamage = directDamage;
        ms.splashDamage = splashDamage;
        ms.splashRadius = splashRadius;

        ms.SetTarget(target.transform);
    }

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
