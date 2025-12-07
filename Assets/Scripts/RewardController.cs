using UnityEngine;
using System.Collections.Generic;

public enum RewardType
{
    DamageUp,
    SpeedUp,
    // RangeUp 삭제됨
    GoldGain
}

[System.Serializable]
public class RewardData
{
    public string rewardName;
    public string description;
    public RewardType type;
    public float value;

    public Sprite icon;
}

public class RewardController : MonoBehaviour
{
    public List<RewardData> allRewards;

    public List<RewardData> GetRandomRewards(int count)
    {
        List<RewardData> selectedRewards = new List<RewardData>();
        List<RewardData> pool = new List<RewardData>(allRewards);

        for (int i = 0; i < count; i++)
        {
            if (pool.Count > 0)
            {
                int randomIndex = Random.Range(0, pool.Count);
                selectedRewards.Add(pool[randomIndex]);
                pool.RemoveAt(randomIndex);
            }
        }
        return selectedRewards;
    }

    public void ApplyReward(RewardData reward)
    {
        if (SystemController.instance == null) return;

        Debug.Log($"보상 적용: {reward.rewardName}");

        switch (reward.type)
        {
            case RewardType.DamageUp:
                SystemController.instance.towerDamageMultiplier += reward.value;
                break;

            case RewardType.SpeedUp:
                SystemController.instance.towerFireRateMultiplier -= reward.value;
                if (SystemController.instance.towerFireRateMultiplier < 0.2f)
                    SystemController.instance.towerFireRateMultiplier = 0.2f;
                break;

            case RewardType.GoldGain:
                SystemController.instance.AddGold((int)reward.value);
                break;
        }
    }
}