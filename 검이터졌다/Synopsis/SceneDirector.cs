using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public enum SceneName
{
    Title =0,
    SynopsisLoading,
    FirstStage,
    SecondStage,
    ThirdStage,
    LastStage
}

public class SceneDirector : MonoBehaviour
{
    #region Singleton
    private static SceneDirector instance;
    public static SceneDirector Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<SceneDirector>();
            return instance;
        }
    }

    #endregion

    [Header("시놉시스컷")]
    public PlayableDirector PD;
    public GameObject Sysnopsis;
    public GameObject SkipButton;
    public GameObject LoadingText;

    public int LoadSceneNumber;
    private double NextTime;
    private AsyncOperation operation;

    private void Awake()
    {
        //씬에 싱글톤 오브젝트가 된 다른 SceneDirector 오브젝트가 있다면 
        if (Instance != this)
        {
            //파괴
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        PD.Play();
        StartCoroutine("SetTime");
        NextTime = 1f;
    }

    void Start()
    {
        LoadNextScene();
    }

    public void LoadNextScene()
    {
        operation = SceneManager.LoadSceneAsync(((SceneName)LoadSceneNumber).ToString());
        operation.allowSceneActivation = false;
        StartCoroutine("IsDoneAsyncPogress");
    }

    IEnumerator IsDoneAsyncPogress()
    {

        while (!operation.isDone)
        {
            if(operation.progress >= 0.9f)
            {
                //Skip 버튼을 사용한다면. . . 
                SkipButton.SetActive(true);
                LoadingText.SetActive(false);
                
                StopCoroutine("IsDoneAsyncPogress");
            }
            yield return null;
        }
    }

    //시놉시스 닫기

    public void NextScene()
    {
        StopCoroutine("SetTime");
        if (SoundManager.Instance.BGM_Player.clip.name != BGM_AudioClip.Synop2.ToString())
        {
            SoundManager.Instance.PlayBGM("Synop2");
        }
        operation.allowSceneActivation = true;
    }

    #region 타임라인 제어 함수~

    IEnumerator SetTime()
    {
        yield return new WaitForSeconds(1/60f);
        PD.time = NextTime;
        StartCoroutine("SetTime");
    }


    //버튼으로 동작합니다.
    public void ShowNextCut()
    {
        StartCoroutine("FadeInScene");
    }

    public void ShowPreviousCut()
    {
        StartCoroutine("FadeOutScene");
    }

    IEnumerator FadeInScene()
    {
        for(int i=0; i<30; i++)
        {
            NextTime += 1 / 30f;
            yield return new WaitForSeconds(1/120f);
        }
    }
    IEnumerator FadeOutScene()
    {
        for (int i = 0; i < 30; i++)
        {
            NextTime -= 1 / 30f;
            yield return new WaitForSeconds(1 /120f);
        }
    }

    #endregion




}