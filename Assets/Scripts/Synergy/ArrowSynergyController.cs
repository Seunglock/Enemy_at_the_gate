using UnityEngine;

[RequireComponent(typeof(ArrowTower))]
public class ArrowSynergyController : MonoBehaviour
{
    private readonly TowerType myType = TowerType.Arrow;
    private ArrowTower originalTower;
    private float baseAttackCooldown;

    void Start()
    {
        originalTower = GetComponent<ArrowTower>();
        if (originalTower == null) return;
        baseAttackCooldown = originalTower.attackCooldown;

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

        float newAttackCooldown = baseAttackCooldown / (1f + totalSynergy);
        originalTower.attackCooldown = newAttackCooldown;

        Debug.Log($"[화살 시너지] 갯수: {SynergyManager.Instance.GetTowerCount(myType)}, 총 공속 보너스: {totalSynergy:P0}, 실제 쿨타임: {newAttackCooldown:F2}s");
    }
}