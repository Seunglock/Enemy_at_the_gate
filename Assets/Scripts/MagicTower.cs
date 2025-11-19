using System.Collections.Generic;
using UnityEngine;

public class MagicTower : MonoBehaviour
{
    [Header("Tower Settings")]
    public float attackRange = 6f;          // 사거리
    public float attackCooldown = 1.2f;     // 공격 속도
    public float damage = 2f;               // 기본 데미지

    [Header("Projectile")]
    public Transform firePoint;
    public GameObject iceMagicPrefab;       // IceMagic 프리팹

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
        GameObject proj = Instantiate(iceMagicPrefab, firePoint.position, Quaternion.identity);

        IceMagic ice = proj.GetComponent<IceMagic>();
        ice.damage = damage;
        ice.SetTarget(target.transform);
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
            UnityEngine.Debug.Log("Magic stay");
            Enemy e = other.GetComponent<Enemy>();

            if (e != null && !enemiesInRange.Contains(e))
                enemiesInRange.Add(e);
        }
    }

}
