using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTower : MonoBehaviour
{
    [Header("Tower Settings")]
    public float attackRange = 8f;
    public float attackCooldown = 1f;
    public Transform firePoint;
    public GameObject arrowPrefab;
    public float damage = 1f;          // 기본 데미지 입력

    private float baseDamage;
    private float cooldownTimer = 0f;

    private List<Enemy> enemiesInRange = new List<Enemy>();

    void Start()
    {
        baseDamage = damage; // 기본 데미지 저장
    }

    void Update()
    {
        // ------------ 전역 강화 데미지 적용 ------------
        damage = baseDamage * SystemController.instance.towerDamageMultiplier;
        // ------------------------------------------------

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
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);

        Arrow arrowScript = arrow.GetComponent<Arrow>();
        arrowScript.SetTarget(target.transform);
        arrowScript.damage = damage;   // 강화된 데미지 적용
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
