using UnityEngine;
using System.Collections.Generic;

public class SynergyManager : MonoBehaviour
{
    public static SynergyManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private Dictionary<TowerType, int> towerCount = new Dictionary<TowerType, int>();

    private Dictionary<TowerType, float> totalSynergyEffect = new Dictionary<TowerType, float>();

    public void UpdateTowerCount(TowerType type, int change)
    {
        if (!towerCount.ContainsKey(type))
        {
            towerCount[type] = 0;
            totalSynergyEffect[type] = 0f;
        }

        towerCount[type] += change;
        if (towerCount[type] < 0) towerCount[type] = 0;

        CalculateSynergy(type);
    }

    private void CalculateSynergy(TowerType type)
    {
        float baseSynergyValue = GetBaseSynergyValue(type);

        totalSynergyEffect[type] = baseSynergyValue * towerCount[type];
    }


    private float GetBaseSynergyValue(TowerType type)
    {
        
        switch (type)
        {
            case TowerType.Arrow: return 0.07f;
            case TowerType.Mortar: return 0.01f;
            case TowerType.Ice: return 0.10f;
            case TowerType.Poison: return 0.07f;
            default: return 0f;
        }
    }

    public int GetTowerCount(TowerType type)
    {
        if (towerCount.ContainsKey(type))
        {
            return towerCount[type];
        }
        return 0;
    }

    public float GetTotalSynergyEffect(TowerType type)
    {
        if (totalSynergyEffect.ContainsKey(type))
        {
            return totalSynergyEffect[type];
        }
        return 0f;
    }
}