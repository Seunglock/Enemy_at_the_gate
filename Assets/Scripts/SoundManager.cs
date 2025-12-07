using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance; // 싱글톤

    [Header("BGM Player")]
    public AudioSource bgmPlayer; // 음악을 재생할 스피커 (AudioSource)

    [Header("BGM Clips")]
    public AudioClip defaultBGM; // 1 ~ 19라운드 음악
    public AudioClip wave20BGM;  // 20 ~ 39라운드 음악
    public AudioClip wave40BGM;  // 40 ~ 49라운드 음악
    public AudioClip wave50BGM;  // 50 ~ 끝까지 음악

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        // 게임 시작 시 기본 음악 재생
        PlayBGM(defaultBGM);
    }

    // WaveManager가 호출할 함수
    public void CheckWaveBGM(int wave)
    {
        if (wave == 20)
        {
            PlayBGM(wave20BGM);
        }
        else if (wave == 40)
        {
            PlayBGM(wave40BGM);
        }
        else if (wave == 50)
        {
            PlayBGM(wave50BGM);
        }
    }

    // 실제로 음악을 교체하고 재생하는 함수
    void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;
        if (bgmPlayer.clip == clip) return; // 이미 같은 음악이면 무시

        bgmPlayer.Stop();
        bgmPlayer.clip = clip;
        bgmPlayer.Play();
    }
}