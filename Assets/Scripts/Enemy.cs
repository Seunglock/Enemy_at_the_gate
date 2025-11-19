using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp = 5f;
    public int goldReward = 5;
    public float moveSpeed = 5f;

    private float originalSpeed;
    private float currentSpeed;


    void Start()
    {
        originalSpeed = moveSpeed; // 너가 Enemy에서 쓰는 이동 속도 변수
        currentSpeed = moveSpeed;
    }

    public void ApplySlow(float slowPercent, float duration)
    {
        StopAllCoroutines();  // 슬로우 중첩 방지
        StartCoroutine(SlowEffect(slowPercent, duration));
    }

    IEnumerator SlowEffect(float slowPercent, float duration)
    {
        currentSpeed = originalSpeed * (1f - slowPercent);
        yield return new WaitForSeconds(duration);
        currentSpeed = originalSpeed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enemy Trigger Enter: " + other.name);
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Destroy(gameObject);

            SystemController.instance.AddGold(goldReward);
        }
    }
}