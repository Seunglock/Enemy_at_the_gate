using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private Animator anim;
    public float hp = 5f;
    public int expReward = 5;
    public float moveSpeed = 5f;

    private float maxHp;
    private float originalSpeed;
    private float currentSpeed;


    void Start()
    {
        originalSpeed = moveSpeed; // Enemy?êÏÑú ?∞Îäî ?¥Îèô ?çÎèÑ Î≥Ä??
        currentSpeed = moveSpeed;
        maxHp = hp;

        anim = GetComponent<Animator>();
    }

    public void ApplySlow(float slowPercent, float duration)
    {
        StopAllCoroutines();  // ?¨Î°ú??Ï§ëÏ≤© Î∞©Ï?
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

            Destroy(gameObject);

            SystemController.instance.AddExp(expReward);

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
            SystemController.instance.AddGold(expReward);
        }
        Destroy(gameObject);
    }
}
