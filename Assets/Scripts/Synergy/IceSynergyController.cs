using UnityEngine;

[RequireComponent(typeof(MagicTower))]
public class IceSynergyController : MonoBehaviour
{
    private readonly TowerType myType = TowerType.Ice;
    private MagicTower originalTower;
    private float currentTotalSlowPercent = 0f;

    private float baseSlowPercent = 0.5f;

    void Start()
    {
        originalTower = GetComponent<MagicTower>();
        if (originalTower == null) return;

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

        float synergyValue = 0f;
        if (SynergyManager.Instance != null)
        {
            synergyValue = SynergyManager.Instance.GetTotalSynergyEffect(myType);
        }

        currentTotalSlowPercent = baseSlowPercent + synergyValue;

        Debug.Log($"[¾óÀ½ ½Ã³ÊÁö] °¹¼ö: {SynergyManager.Instance.GetTowerCount(myType)}, ÃÑ µÐÈ­ º¸³Ê½º: {synergyValue:P0}, ½ÇÁ¦ µÐÈ­À²: {currentTotalSlowPercent:P0}");
    }

    public void ApplySynergyToProjectile(IceMagic iceProjectile)
    {
        if (iceProjectile != null)
        {
            iceProjectile.slowPercent = currentTotalSlowPercent;
        }
    }
}