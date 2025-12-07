using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpPanel : MonoBehaviour
{
    public RewardController rewardController; // Inspector에서 연결

    public GameObject[] rewardButtons; // 보상 선택 버튼 3개 배열
    public Text[] rewardTexts;         // 각 버튼의 텍스트 (옵션)
    public Image[] rewardIcons;

    // 현재 화면에 떠있는 보상 데이터 저장용
    private List<RewardData> currentOptions;

    // 패널이 켜질 때 (레벨업 순간)
    private void OnEnable()
    {
        // 게임 일시 정지
        Time.timeScale = 0f;

        // RewardController에게 랜덤 보상 요청
        if (rewardController != null && rewardButtons.Length > 0)
        {
            // 버튼 개수만큼 랜덤 보상을 가져옴
            currentOptions = rewardController.GetRandomRewards(rewardButtons.Length);

            // 가져온 보상 정보를 버튼 UI에 표시
            for (int i = 0; i < rewardButtons.Length; i++)
            {
                // 보상 데이터가 있으면 버튼 활성화 및 UI 갱신
                if (i < currentOptions.Count)
                {
                    rewardButtons[i].SetActive(true);
                    RewardData data = currentOptions[i];

                    // (1) 아이콘 이미지 적용
                    if (rewardIcons != null && i < rewardIcons.Length)
                    {
                        rewardIcons[i].sprite = data.icon;
                        rewardIcons[i].preserveAspect = true; // 비율 유지
                    }

                    // (2) 설명 텍스트 적용 (이름 + 설명)
                    if (rewardTexts != null && i < rewardTexts.Length)
                    {
                        rewardTexts[i].text = $"<b>{data.rewardName}</b>\n<size=18>{data.description}</size>";
                    }
                }
                else
                {
                    // 데이터 없으면 버튼 끄기
                    rewardButtons[i].SetActive(false);
                }
            }
        }
    }

    // 보상 버튼을 눌렀을 때 호출 (인덱스 0, 1, 2)
    // Inspector의 Button OnClick()에서 인자값을 0, 1, 2로 각각 설정해줘야 함
    public void OnSelectReward(int index)
    {
        if (rewardController != null && currentOptions != null && index < currentOptions.Count)
        {
            // 선택한 보상을 적용해달라고 요청
            rewardController.ApplyReward(currentOptions[index]);
        }

        Debug.Log($"보상 선택 완료! (Index: {index})");

        // 창 닫기
        gameObject.SetActive(false);
    }

    // 패널이 꺼질 때 (게임 재개)
    private void OnDisable()
    {
        // 배속 복구
        if (SystemController.instance != null)
        {
            Time.timeScale = SystemController.instance.currentSpeed;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
