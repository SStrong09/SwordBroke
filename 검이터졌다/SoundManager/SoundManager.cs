using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum BGM_AudioClip
{
    Synop1,
    Synop2
}

public enum SFX_BattleAudioCilp
{

}

public enum SFX_EnvironmentAudioCilp
{
    Wind,
    Drop,
    Laugh,
    Lightning
}

public enum SFX_UIAudioCilp
{
    Book,
    PianoDing
}

public enum SoundBundleType
{
    BGM,
    SFX_Battle,
    SFX_Environment,
    SFX_UI,
}


public class SoundManager : MonoBehaviour
{
    #region Singleton
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<SoundManager>();
            return instance;
        }
    }
    #endregion

    [Header("사운드 번들")]
    /// <summary>
    ///     번들 목록
    ///     BGM,
    ///     SFX_Battle,
    ///     SFX_Environment,
    ///     SFX_UI
    /// </summary>
    public SoundBundle[] SoundBundles;
    

    [Header("브금 플레이어")]
    public AudioSource BGM_Player;

    [Header("효과음 플레이어")]

    [SerializeField] List<AudioSource> SFX_Player;
    [SerializeField] List<AudioSource> SFX_Player_NowPlaying;


    [Header("믹서")]
    public AudioMixerGroup BGM_Mixer;
    public AudioMixerGroup SFX_Mixer;

    public float AudioPlayerPoolSize=20;

    //to Json  UI Slider 
    float volme;

    private void Awake()
    {
        //씬에 싱글톤 오브젝트가 된 다른 SoundManager 오브젝트가 있다면 
        if (Instance != this)
        {
            //파괴
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        SFX_Player = new List<AudioSource>();
        SFX_Player_NowPlaying = new List<AudioSource>();
        BGM_Player.volume = 0;
        BGM_Player.outputAudioMixerGroup = BGM_Mixer;
    }

    #region 볼륨제어 함수

    IEnumerator SetBGMVolume_FadeIn()
    {
        yield return new WaitUntil(() => BGM_Player.volume != 1);
        BGM_Player.volume += Time.deltaTime * 0.4f;

        if (BGM_Player.volume > 0.9f)
        {
            StopCoroutine("SetBGMVolume_FadeIn");
        }
        StartCoroutine("SetBGMVolume_FadeIn");
    }

    IEnumerator SetBGMVolume_FadeOut()
    {
        yield return new WaitUntil(() => BGM_Player.volume != 1);
        BGM_Player.volume -= Time.deltaTime * 0.4f;

        if(BGM_Player.volume == 0)
        {
            StopCoroutine("SetBGMVolume_FadeOut");
        }
        StartCoroutine("SetBGMVolume_FadeOut");
    }


    public void BGMVolume_MUTE()
    {
        BGM_Player.volume = 0;
    }

    #endregion



    #region 오디오 재생관련 함수

    public void PlayBGM(string BGMname)
    {
        BGM_Player.clip = SoundBundles[0].Sounds[(int)BGMname.ToEnum<BGM_AudioClip>()].clip;
        BGM_Player.Play();
    }

    public void PlaySound(SoundBundleType BundleNumber, string SoundName)
    {
        // 사용 할수있는 오디오 플레이어 컴포넌트가 없을때 . . .
        // 새로운 오디오소스 컴포넌트를 추가해준다.
        if (SFX_Player.Count == 0)
        {
            // 오디오 플레이어가 풀사이즈보다 작을때 . . .
            if (SFX_Player.Count + SFX_Player_NowPlaying.Count < AudioPlayerPoolSize)
            {
                AudioSource AudioPlayer = gameObject.AddComponent<AudioSource>();

                switch (BundleNumber)
                {
                    // 전투 효과음일 경우
                    case SoundBundleType.SFX_Battle:
                        AudioPlayer.clip = SoundBundles[(int)BundleNumber].Sounds[(int)SoundName.ToEnum<SFX_BattleAudioCilp>()].clip;
                        AudioPlayer.volume = SoundBundles[(int)BundleNumber].Sounds[(int)SoundName.ToEnum<SFX_BattleAudioCilp>()].Volume;
                        break;
                    // 환경 효과음일 경우
                    case SoundBundleType.SFX_Environment:
                        AudioPlayer.clip = SoundBundles[(int)BundleNumber].Sounds[(int)SoundName.ToEnum<SFX_EnvironmentAudioCilp>()].clip;
                        AudioPlayer.volume = SoundBundles[(int)BundleNumber].Sounds[(int)SoundName.ToEnum<SFX_EnvironmentAudioCilp>()].Volume;
                        break;
                    // UI 효과음일 경우
                    case SoundBundleType.SFX_UI:
                        AudioPlayer.clip = SoundBundles[(int)BundleNumber].Sounds[(int)SoundName.ToEnum<SFX_UIAudioCilp>()].clip;
                        AudioPlayer.volume = SoundBundles[(int)BundleNumber].Sounds[(int)SoundName.ToEnum<SFX_UIAudioCilp>()].Volume;
                        break;
                }
                StartCoroutine(SortPlayerList(AudioPlayer));
            }
        }
        // 오디오 플레이어 컴포넌트가 존재할때 . . .
        // 이미 있는 오디오 플레이어 컴포넌트를 사용한다. . . 
        else
        {
            switch (BundleNumber)
            {
                // 전투 효과음일 경우
                case SoundBundleType.SFX_Battle:
                    SFX_Player[0].clip = SoundBundles[(int)BundleNumber].Sounds[(int)SoundName.ToEnum<SFX_BattleAudioCilp>()].clip;
                    SFX_Player[0].volume = SoundBundles[(int)BundleNumber].Sounds[(int)SoundName.ToEnum<SFX_BattleAudioCilp>()].Volume;
                    
                    break;
                // 환경 효과음일 경우
                case SoundBundleType.SFX_Environment:
                    SFX_Player[0].clip = SoundBundles[(int)BundleNumber].Sounds[(int)SoundName.ToEnum<SFX_EnvironmentAudioCilp>()].clip;
                    SFX_Player[0].volume = SoundBundles[(int)BundleNumber].Sounds[(int)SoundName.ToEnum<SFX_EnvironmentAudioCilp>()].Volume;
                    break;
                // UI 효과음일 경우
                case SoundBundleType.SFX_UI:
                    SFX_Player[0].clip = SoundBundles[(int)BundleNumber].Sounds[(int)SoundName.ToEnum<SFX_UIAudioCilp>()].clip;
                    SFX_Player[0].volume = SoundBundles[(int)BundleNumber].Sounds[(int)SoundName.ToEnum<SFX_UIAudioCilp>()].Volume;
                    break;
            }
            StartCoroutine(SortPlayerList(SFX_Player[0]));
        }
    }


    IEnumerator SortPlayerList(AudioSource AudioPlayer)
    {
        AudioPlayer.outputAudioMixerGroup = SFX_Mixer;
        AudioPlayer.Play();
        if(SFX_Player.Count != 0)
        {
            SFX_Player.Remove(AudioPlayer);
        }
        SFX_Player_NowPlaying.Add(AudioPlayer);
        yield return new WaitUntil(() => AudioPlayer.isPlaying == false);
        SFX_Player.Add(AudioPlayer);
        SFX_Player_NowPlaying.Remove(AudioPlayer);
    }

    #endregion
}
