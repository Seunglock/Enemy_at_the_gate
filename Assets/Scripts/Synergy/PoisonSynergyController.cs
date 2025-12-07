using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(AlchemistTower))]
public class PoisonSynergyController : MonoBehaviour
{
    private readonly TowerType myType = TowerType.Poison;
    private AlchemistTower originalTower;
    private float basePercentDamage;

    void Start()
    {
        originalTower = GetComponent<AlchemistTower>();
        if (originalTower == null) return;

        basePercentDamage = originalTower.percentDamage;

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

        
        float newPercentDamage = basePercentDamage + totalSynergy;
        originalTower.percentDamage = newPercentDamage;

     
        UnityEngine.Debug.Log($"[독 시너지] 갯수: {SynergyManager.Instance.GetTowerCount(myType)}, 총 비례 피해 보너스: {totalSynergy:P0}, 실제 비례 피해: {newPercentDamage:P0}");
    }
}