using UnityEngine;
using UnityEngine.UI;

public class UIExpBar : MonoBehaviour
{
    [Header("UI References")]
    public Image mask; // 크기가 변하는 마스크 (게이지 바)

    private float originalSize;

    void Start()
    {
        // 바의 원래 크기(최대 길이)를 저장합니다.
        if (mask != null)
        {
            originalSize = mask.rectTransform.rect.width;
        }
    }
    //a
    void Update()
    {
        // SystemController가 없으면 실행하지 않음
        if (SystemController.instance == null) return;

        // 1. 현재 경험치 비율 계산 (0.0 ~ 1.0)
        float currentExp = (float)SystemController.instance.exp;
        float maxExp = (float)SystemController.instance.maxExp;

        float ratio = 0f;
        if (maxExp > 0)
        {
            ratio = Mathf.Clamp01(currentExp / maxExp);
        }

        // 2. 바 크기 조절 (Mask의 너비를 비율만큼 설정)
        if (mask != null)
        {
            mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * ratio);
        }
    }
}