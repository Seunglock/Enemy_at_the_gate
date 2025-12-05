using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTower : MonoBehaviour
{
    [Header("Tower Settings")]
    public float attackRange = 8f;     // 감지 범위
    public float attackCooldown = 1f;  // 공격 속도
    public Transform firePoint;        // 화살이 나갈 위치
    public GameObject arrowPrefab;     // 화살 프리팹
    public float damage = 1f;          // 화살 데미지

    private float originalCooldown;
    private float originalDamage;

    private float cooldownTimer = 0f;

    // 현재 감지된 적 리스트
    private List<Enemy> enemiesInRange = new List<Enemy>();

    void Start() // Start 추가
    {
        // 게임 시작 시 원래 능력치 저장
        originalCooldown = attackCooldown;
        originalDamage = damage;
    }

    //버프 받았을 시 반응할 수 있게 추가
    public void ActivateBuff(float duration)
    {
        StopCoroutine("BuffRoutine");
        StartCoroutine(BuffRoutine(duration));
    }
    //버프 설정
     IEnumerator BuffRoutine(float duration)
    {
        attackCooldown = originalCooldown * 0.5f;
        damage = originalCooldown * 2f;

        yield return new WaitForSeconds(duration);

        attackCooldown = originalCooldown;
        damage = originalDamage;

    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        // 가장 앞 적 찾기
        Enemy target = GetFrontEnemy();

        if (target != null && cooldownTimer <= 0f)
        {
            Shoot(target);
            cooldownTimer = attackCooldown;
        }
    }

    void Shoot(Enemy target)
    {
        UnityEngine.Debug.Log("Shoot");
        // 화살 생성
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);

        // 화살에 타겟 전달
        Arrow a = arrow.GetComponent<Arrow>();
        a.SetTarget(target.transform);
        a.damage = damage;
    }

    /// <summary>
    /// 리스트 중 가장 앞선(가장 많이 이동한) 적 반환
    /// 기준: Z축이 가장 큰 적
    /// </summary>
    Enemy GetFrontEnemy()
    {
        if (enemiesInRange.Count == 0) return null;

        Enemy front = enemiesInRange[0];

        foreach (Enemy e in enemiesInRange)
        {
            if (e != null && e.transform.position.z > front.transform.position.z)
            {
                front = e;
            }
        }
        return front;
    }

    // 감지 트리거에 적이 들어오면 리스트에 추가
    private void OnTriggerEnter2D(Collider2D other)
    {
        UnityEngine.Debug.Log("Enter");
        if (other.CompareTag("Enemy"))
        {
            Enemy e = other.GetComponent<Enemy>();
            if (e != null && !enemiesInRange.Contains(e))
            {
                enemiesInRange.Add(e);
            }
        }
    }

    // 감지 범위에서 나가면 리스트에서 제거
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy e = other.GetComponent<Enemy>();
            if (e != null && enemiesInRange.Contains(e))
            {
                enemiesInRange.Remove(e);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        UnityEngine.Debug.Log("stay");
        if (other.CompareTag("Enemy"))
        {
            Enemy e = other.GetComponent<Enemy>();
            if (e != null && !enemiesInRange.Contains(e))
                enemiesInRange.Add(e);
        }
    }
}