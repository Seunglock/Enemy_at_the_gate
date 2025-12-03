using UnityEngine;
using System.Collections;

// 需要 ArrowTower 脚本在同一个 GameObject 上
[RequireComponent(typeof(ArrowTower))]
public class ArrowSynergyController : MonoBehaviour
{
    // 定义这个塔的类型（用于SynergyManager）
    private readonly TowerType myType = TowerType.Arrow;

    // 引用 ArrowTower 组件，以便访问和修改其属性
    private ArrowTower originalTower;

    // 用于保存 ArrowTower 的基础冷却时间，防止协同效应叠加导致数值错误
    private float baseAttackCooldown;

    void Start()
    {
        // 获取原始的 ArrowTower 组件
        originalTower = GetComponent<ArrowTower>();

        // 检查组件是否存在
        if (originalTower == null)
        {
            Debug.LogError("ArrowSynergyController requires an ArrowTower component on the same GameObject.");
            return;
        }

        // 1. 记录基础冷却时间（您的原始值）
        // 您的原始代码中变量名为 attackCooldown
        baseAttackCooldown = originalTower.attackCooldown;

        // 2. 注册到 SynergyManager：告诉管理器我被建造了 (+1)
        if (SynergyManager.Instance != null)
        {
            SynergyManager.Instance.UpdateTowerCount(myType, 1);
        }

        // 3. 首次计算并应用协同效应
        RecalculateSynergyEffect();
    }

    void OnDestroy()
    {
        // 塔被销毁时，从管理器中移除计数 (-1)
        if (SynergyManager.Instance != null)
        {
            SynergyManager.Instance.UpdateTowerCount(myType, -1);
        }
    }

    /// <summary>
    /// 重新计算并应用协同效应加成
    /// </summary>
    public void RecalculateSynergyEffect()
    {
        if (originalTower == null) return;

        float totalSynergy = 0f;
        if (SynergyManager.Instance != null)
        {
            // 获取所有 Arrow Tower 提供的总攻击速度加成 (例如 0.21 或 21%)
            totalSynergy = SynergyManager.Instance.GetTotalSynergyEffect(myType);
        }

        // 攻击冷却时间 (attackCooldown) 越小越快。
        // 新冷却时间 = 基础冷却时间 / (1 + 攻击速度加成)

        float newAttackCooldown = baseAttackCooldown / (1f + totalSynergy);

        // 覆盖原始 ArrowTower 脚本的攻击冷却时间
        originalTower.attackCooldown = newAttackCooldown;

        Debug.Log($"[ArrowSynergy] 数量: {SynergyManager.Instance.GetTowerCount(myType)}, 总攻速加成: {totalSynergy:P0}, 实际冷却时间: {newAttackCooldown:F2}s");
    }
}