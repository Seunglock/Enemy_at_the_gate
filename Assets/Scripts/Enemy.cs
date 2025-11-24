using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp = 5f;
    public int goldReward = 5;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;

        // Hit 애니메이션
        if (anim != null)
            anim.SetTrigger("Hit");

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
