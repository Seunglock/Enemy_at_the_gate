using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp = 5f;
    public int goldReward = 5;

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