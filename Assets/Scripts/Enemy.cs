using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private Animator anim;
    public float hp = 5f;
    public int goldReward = 5;
    public float moveSpeed = 5f;

    private float maxHp;
    private float originalSpeed;
    private float currentSpeed;


    void Start()
    {
        originalSpeed = moveSpeed; // Enemy에서 쓰는 이동 속도 변수
        currentSpeed = moveSpeed;
        maxHp = hp;

        anim = GetComponent<Animator>();
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

    public void TakeDamage(float damage)
    {
        hp -= damage;
        Debug.Log("Enemy health: " + name +":"+ hp);
        if (hp <= 0)
        {
            Die();
        }
    }

    public void TakePercentDamage(float percent)
    {
        float damageAmount = maxHp * percent;
        hp -= damageAmount;
        Debug.Log("Enemy health: " + name + ":" + hp);
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
