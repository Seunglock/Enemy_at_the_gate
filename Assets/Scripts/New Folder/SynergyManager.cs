// SynergyManager.cs (完整替换版)
using UnityEngine;
using System.Collections.Generic;

public class SynergyManager : MonoBehaviour
{
    // ------------------------------------
    // 1. 单例模式
    // ------------------------------------
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
            // 确保在场景加载时不被销毁，以便持续追踪协同效应
            // 建议：如果您只在一个场景中游戏，可以移除这行：DontDestroyOnLoad(gameObject); 
            // DontDestroyOnLoad(gameObject); 
        }
    }

    // ------------------------------------
    // 2. 核心数据：计数器和效果值
    // ------------------------------------
    // 存储每种类型塔的数量
    private Dictionary<TowerType, int> towerCount = new Dictionary<TowerType, int>();

    // 存储每种类型塔的*总*协同效应数值 (例如：3个Arrow Tower -> 0.21)
    private Dictionary<TowerType, float> totalSynergyEffect = new Dictionary<TowerType, float>();

    // ------------------------------------
    // 3. 核心方法：更新计数和效果
    // ------------------------------------

    /// <summary>
    /// 当建造 (+1) 或出售 (-1) 塔时调用此方法。
    /// </summary>
    public void UpdateTowerCount(TowerType type, int change)
    {
        // 初始化字典项（如果尚未存在）
        if (!towerCount.ContainsKey(type))
        {
            towerCount[type] = 0;
            totalSynergyEffect[type] = 0f;
        }

        // 1. 更新数量
        towerCount[type] += change;
        if (towerCount[type] < 0) towerCount[type] = 0; // 防止数量为负

        // 2. 重新计算协同效应
        CalculateSynergy(type);

        // 注意：这里没有 Debug.Log，因为 ArrowSynergyController 中有更详细的日志。
    }

    // 重新计算某个类型的总协同效应
    private void CalculateSynergy(TowerType type)
    {
        // 基础协同效应数值（每个塔类型增加的单次百分比）
        float baseSynergyValue = GetBaseSynergyValue(type);

        // 总协同效应 = 基础值 * 数量
        totalSynergyEffect[type] = baseSynergyValue * towerCount[type];

        // *** 通知所有该类型塔更新属性（可选的高级优化）***
        // 如果您希望当建造/出售一个塔时，所有同类塔都立即更新，这里需要一个事件系统。
        // 但目前的最简实现，我们依赖每个 ArrowSynergyController 自己去读取最新值。
    }

    // 辅助方法：返回每个塔类型的基础协同效应值
    private float GetBaseSynergyValue(TowerType type)
    {
        // 华沙塔: 攻击速度 7% / 朴击泡塔: 攻击范围 1% / 欧冷塔: 减速 10% / 毒塔: 体力比伤害 7%
        switch (type)
        {
            case TowerType.Arrow: return 0.07f;
            case TowerType.Mortar: return 0.01f;
            case TowerType.Ice: return 0.10f;
            case TowerType.Poison: return 0.07f;
            default: return 0f;
        }
    }

    // ------------------------------------
    // 4. 公共查询方法 (新增和已有的)
    // ------------------------------------

    /// <summary>
    /// 供外部查询某个类型塔的数量 【新增的方法】
    /// </summary>
    public int GetTowerCount(TowerType type)
    {
        if (towerCount.ContainsKey(type))
        {
            return towerCount[type];
        }
        return 0;
    }

    /// <summary>
    /// 供防御塔对象查询总协同效应数值。
    /// </summary>
    public float GetTotalSynergyEffect(TowerType type)
    {
        if (totalSynergyEffect.ContainsKey(type))
        {
            return totalSynergyEffect[type];
        }
        return 0f;
    }
}