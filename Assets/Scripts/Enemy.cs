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

    private bool isBlocked = false;       // 현재 막혀있는지 확인
    private GameObject targetBarricade;   // 나를 막고 있는 바리케이드 오브젝트
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
        if (isBlocked && targetBarricade == null)
        {
            isBlocked = false; // 다시 이동 가능 상태로 변경
        }

        // 2. 막히지 않았을 때만 이동 함수 호출
        if (!isBlocked)
        {
            Move();
        }
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
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Barricade"))
        {
            // 1. 내가 지금 가려는 목표 방향 (벡터)
            if (path == null || index >= path.Length) return;
            Vector3 moveDir = (path[index].position - transform.position).normalized;

            // 2. 나 -> 바리케이드 쪽을 향하는 방향 (벡터)
            Vector3 toBarricade = (other.transform.position - transform.position).normalized;

            // 3. 두 방향이 얼마나 비슷한지 계산 (내적)
            // 결과가 0보다 크면: 바리케이드가 '내 앞' (시야각 180도 내)에 있음 -> 멈춤
            // 결과가 0보다 작으면: 바리케이드가 '내 뒤'에 있음 -> 지나친 것이므로 통과
            float dot = Vector3.Dot(moveDir, toBarricade);

            if (dot > 0)
            {
                isBlocked = true;
                targetBarricade = other.gameObject;
            }
            else
            {
                // 이미 지나쳤으면(뒤에 있으면) 족쇄를 풀어줌
                isBlocked = false;
                targetBarricade = null;
                if (anim != null) anim.speed = 1;
            }
        }
    }

    // [수정] 나갈 때도 마찬가지로 Trigger
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Barricade"))
        {
            isBlocked = false;
            targetBarricade = null;
            if (anim != null) anim.speed = 1;
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
