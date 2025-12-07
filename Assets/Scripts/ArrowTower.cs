using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTower : MonoBehaviour
{
    [Header("Tower Settings")]
    public float attackRange = 8f;     
    public float attackCooldown = 1f;
    public float damage = 10f;

    [Header("Projectile")]
    public Transform firePoint;
    public GameObject arrowPrefab;

  
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
     
        damage = baseDamage * SystemController.instance.towerDamageMultiplier;

       
        float currentCooldown = baseCooldown * SystemController.instance.towerFireRateMultiplier;

 

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
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        arrowScript.SetTarget(target.transform);
        arrowScript.damage = damage;
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