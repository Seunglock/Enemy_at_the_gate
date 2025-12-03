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


    void Start()
    {
        originalSpeed = moveSpeed;
        currentSpeed = moveSpeed;
        maxHp = hp;

        anim = GetComponent<Animator>();

        originalScale = transform.localScale;
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
        Destroy(gameObject);
    }

    public void ApplySlow(float slowPercent, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(SlowEffect(slowPercent, duration));
    }

    System.Collections.IEnumerator SlowEffect(float slowPercent, float duration)
    {
        currentSpeed = originalSpeed * (1f - slowPercent);
        yield return new WaitForSeconds(duration);
        currentSpeed = originalSpeed;
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;

        if (hp <= 0)
            Die();
    }

    public void TakePercentDamage(float percent)
    {
        float damageAmount = maxHp * percent;
        hp -= damageAmount;

        if (hp <= 0)
            Die();
    }

    void Die()
    {
        WaveManager.instance.UnregisterEnemy(this);

        if (SystemController.instance != null)
            SystemController.instance.AddGold(goldReward);

        Destroy(gameObject);
    }

    public void SetPath(Transform[] newPath)
    {
        path = newPath;
        index = 0;
    }
}
