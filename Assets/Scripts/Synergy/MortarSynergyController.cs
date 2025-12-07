using UnityEngine;

[RequireComponent(typeof(MortarTower))]
public class MortarSynergyController : MonoBehaviour
{
    private readonly TowerType myType = TowerType.Mortar;
    private MortarTower originalTower;
    private float baseAttackRange;

    void Start()
    {
        originalTower = GetComponent<MortarTower>();
        if (originalTower == null) return;
        baseAttackRange = originalTower.attackRange;

        if (SynergyManager.Instance != null)
        {
            SynergyManager.Instance.UpdateTowerCount(myType, 1);
        }
        RecalculateSynergyEffect();
    }

    void OnDestroy()
    {
        if (SynergyManager.Instance != null)
        {
            SynergyManager.Instance.UpdateTowerCount(myType, -1);
        }
    }

    public void RecalculateSynergyEffect()
    {
        if (originalTower == null) return;

        float totalSynergy = 0f;
        if (SynergyManager.Instance != null)
        {
            totalSynergy = SynergyManager.Instance.GetTotalSynergyEffect(myType);
        }

        float newAttackRange = baseAttackRange * (1f + totalSynergy);
        originalTower.attackRange = newAttackRange;

        Debug.Log($"[박격포 시너지] 갯수: {SynergyManager.Instance.GetTowerCount(myType)}, 총 범위 보너스: {totalSynergy:P0}, 실제 사거리: {newAttackRange:F2}");

        UpdateColliderRange(newAttackRange);
    }

    private void UpdateColliderRange(float newRange)
    {
        CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
        if (circleCollider != null)
        {
            circleCollider.radius = newRange;
        }
    }
}