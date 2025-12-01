using UnityEngine;
using System.Collections.Generic;

// 보상 정보를 담을 간단한 클래스
[System.Serializable]
public class RewardData
{
    public string rewardName; // 보상 이름 (예: "공격력 증가")
    public string description; // 설명
    public RewardType type;   // 보상 타입
    public float value;       // 증가 수치
}

public enum RewardType
{
    DamageUp,
    SpeedUp,
    HealthUp,
    GoldGain
}

public class RewardController : MonoBehaviour
{
    [Header("Reward Database")]
    public List<RewardData> allRewards; // Inspector에서 설정할 전체 보상 목록

    // 랜덤으로 count 개수만큼 보상을 뽑아서 반환하는 함수
    public List<RewardData> GetRandomRewards(int count)
    {
        List<RewardData> selectedRewards = new List<RewardData>();

        // 원본 리스트 보호를 위해 복사본 생성
        List<RewardData> pool = new List<RewardData>(allRewards);

        for (int i = 0; i < count; i++)
        {
            if (pool.Count > 0)
            {
                int randomIndex = Random.Range(0, pool.Count);
                selectedRewards.Add(pool[randomIndex]);
                pool.RemoveAt(randomIndex); // 중복 뽑기 방지
            }
        }
        return selectedRewards;
    }

    // 선택된 보상을 실제로 캐릭터나 시스템에 적용하는 함수
    public void ApplyReward(RewardData reward)
    {
        Debug.Log($"보상 적용됨: {reward.rewardName} (+{reward.value})");

        // 예시: SystemController나 PlayerStats에 접근하여 능력치 적용
        /*
        switch (reward.type)
        {
            case RewardType.DamageUp:
                // Player.instance.damage += reward.value;
                break;
            case RewardType.GoldGain:
                if (SystemController.instance != null)
                    SystemController.instance.AddGold((int)reward.value);
                break;
        }
        */
    }
}