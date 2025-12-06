using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator anim;

    public float hp = 5f;
    public int goldReward = 5;
    public float moveSpeed = 2f;

    private float maxHp;
    private float originalSpeed;
    private float currentSpeed;

    private Transform[] path;
    private int index = 0;

    private Vector3 originalScale;

    private Coroutine slowCoroutine;
    private Coroutine speedBoostCoroutine;

    void Start()
    {
        originalSpeed = moveSpeed;
        currentSpeed = moveSpeed;
        maxHp = hp;

        anim = GetComponent<Animator>();
        originalScale = transform.localScale;

        if (WaveManager.instance != null)
        {
            WaveManager.instance.RegisterEnemy(this);
        }
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if (path == null || index >= path.Length)
            return;

        Transform target = path[index];

        if (target.position.x < transform.position.x)
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        else
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);

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
        Die(false);
    }

    public void ApplySlow(float slowPercent, float duration)
    {
        if (slowCoroutine != null)
            StopCoroutine(slowCoroutine);

        slowCoroutine = StartCoroutine(SlowEffect(slowPercent, duration));
    }

    IEnumerator SlowEffect(float slowPercent, float duration)
    {
        currentSpeed = originalSpeed * (1f - slowPercent);
        yield return new WaitForSeconds(duration);
        currentSpeed = originalSpeed;
        slowCoroutine = null;
    }

    public void ApplyTemporarySpeedBoost(float multiplier, float duration)
    {
        if (speedBoostCoroutine != null)
            StopCoroutine(speedBoostCoroutine);

        speedBoostCoroutine = StartCoroutine(SpeedBoost(multiplier, duration));
    }

    IEnumerator SpeedBoost(float multiplier, float duration)
    {
        float before = currentSpeed;
        currentSpeed = originalSpeed * multiplier;
        yield return new WaitForSeconds(duration);
        currentSpeed = originalSpeed;
        speedBoostCoroutine = null;
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;

        if (hp <= 0)
            Die(true);
    }

    public void TakePercentDamage(float percent)
    {
        float damageAmount = maxHp * percent;
        hp -= damageAmount;

        if (hp <= 0)
            Die(true);
    }

    void Die(bool giveGold)
    {
        if (WaveManager.instance != null)
        {
            WaveManager.instance.UnregisterEnemy(this);
        }

        if (giveGold && SystemController.instance != null)
            SystemController.instance.AddGold(goldReward);

        Destroy(gameObject);
    }

    public void SetPath(Transform[] newPath)
    {
        path = newPath;
        index = 0;
    }
}
