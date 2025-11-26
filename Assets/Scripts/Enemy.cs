using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator anim;

    [Header("Stats")]
    public float hp = 5f;
    public int goldReward = 5;
    public float moveSpeed = 5f;

    private float maxHp;
    private float originalSpeed;
    private float currentSpeed;

    private Transform[] path;
    private int index = 0;

    void Start()
    {
        originalSpeed = moveSpeed;
        currentSpeed = moveSpeed;
        maxHp = hp;

        anim = GetComponent<Animator>();

        path = WaypointManager.instance.waypoints;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if (path == null || index >= path.Length) return;

        Transform target = path[index];

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            currentSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            index++;

            if (index >= path.Length)
            {
                ReachGoal();
            }
        }
    }

    void ReachGoal()
    {
        // 여기에 성 체력 감소 등 구현해도 됨
        Destroy(gameObject);
    }

    public void ApplySlow(float slowPercent, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(SlowEffect(slowPercent, duration));
    }

    IEnumerator SlowEffect(float slowPercent, float duration)
    {
        currentSpeed = originalSpeed * (1f - slowPercent);
        yield return new WaitForSeconds(duration);
        currentSpeed = originalSpeed;
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        UnityEngine.Debug.Log("Enemy health: " + name + ":" + hp);

        if (hp <= 0)
        {
            Die();
        }
    }

    public void TakePercentDamage(float percent)
    {
        float damageAmount = maxHp * percent;
        hp -= damageAmount;

        UnityEngine.Debug.Log("Enemy health: " + name + ":" + hp);

        if (hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (SystemController.instance != null)
        {
            SystemController.instance.AddGold(goldReward);
        }

        Destroy(gameObject);
    }
}
