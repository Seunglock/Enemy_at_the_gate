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

    // 죽었는지 체크하는 플래그 (중복 호출 방지)
    private bool isDead = false;

    void Start()
    {
        originalSpeed = moveSpeed;
        currentSpeed = moveSpeed;
        maxHp = hp;

        anim = GetComponent<Animator>();
        originalScale = transform.localScale;

        if (WaveManager.instance != null)
            WaveManager.instance.RegisterEnemy(this);
    }

    void Update()
    {
        // 이미 죽은 상태면 이동 로직 실행 안 함
        if (isDead) return;

        Move();
    }

    void Move()
    {
        if (path == null || index >= path.Length)
            return;

        Transform target = path[index];

        // 방향 전환 (좌우 반전)
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
                ReachGoal();
        }
    }

    void ReachGoal()
    {
        // 목적지 도착은 '처치'가 아니므로 false 전달
        // (나중에 플레이어 라이프 깎는 로직을 여기에 추가하면 됩니다)
        Die(false);
    }

    public void ApplySlow(float slowPercent, float duration)
    {
        if (isDead) return;
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
        if (isDead) return;

        hp -= damage;

        if (hp <= 0)
        {
            // 플레이어에 의해 죽었으므로 true 전달
            Die(true);
        }
    }

    public void TakePercentDamage(float percent)
    {
        if (isDead) return;

        float damageAmount = maxHp * percent;
        hp -= damageAmount;

        if (hp <= 0)
        {
            Die(true);
        }
    }

    /// <summary>
    /// 적 사망/제거 처리
    /// </summary>
    /// <param name="isKill">플레이어가 처치했으면 true, 도망친거면 false</param>
    void Die(bool isKill)
    {
        if (isDead) return; // 중복 실행 방지
        isDead = true;

        // 1. WaveManager에서 적 리스트 제거 (공통)
        if (WaveManager.instance != null)
        {
            WaveManager.instance.UnregisterEnemy(this);

            // [추가된 기능] 처치된 경우에만 킬 카운트(점수) 증가
            if (isKill)
            {
                WaveManager.instance.AddKillCount();
            }
        }

        // 2. 골드 지급 (처치된 경우에만)
        if (isKill && SystemController.instance != null)
        {
            SystemController.instance.AddGold(goldReward);
        }

        // 3. (선택사항) 사망 애니메이션이 있다면 여기서 실행 후 Destroy 지연
        // anim.SetTrigger("Die");

        Destroy(gameObject);
    }

    public void SetPath(Transform[] newPath)
    {
        path = newPath;
        index = 0;
    }
}