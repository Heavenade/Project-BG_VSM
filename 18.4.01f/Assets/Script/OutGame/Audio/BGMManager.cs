using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    static public BGMManager instance;

    /*싱글톤*/
    #region Singleton
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.transform.root.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.transform.root.gameObject);
            instance = this;
        }
        source = this.GetComponent<AudioSource>();
    }
    #endregion Singleton

    public AudioClip[] clips; //BGM 리스트
    private AudioSource source;

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    private float BGMVolume; //BGM 볼륨

    private int BGMnowplaying; //현재 재생중인 BGM

    void Start()
    {
        AutoSelectBGM(SceneManager.GetActiveScene(),LoadSceneMode.Single);//BGM 셀렉
        //PlayBGM(BGMnowplaying);//BGM 재생

        SceneManager.sceneLoaded += AutoSelectBGM;

    }

    /*씬 변경 시마다 BGM 적용*/
    public void AutoSelectBGM(Scene scene, LoadSceneMode loadSceneMode)
    {
        int preBGMnowPlaying = BGMnowplaying;
        int nextBGMnowPlaying = preBGMnowPlaying;

        string curscene = scene.name;


        /*BGM 변경 시*/
        if (preBGMnowPlaying != nextBGMnowPlaying)
        {
            BGMnowplaying = nextBGMnowPlaying;
            FadeOutBGM();
            //StopBGM();
            PlayBGM(BGMnowplaying);
            FadeInBGM();
        }           
    }

    /*BGM 재생*/
    public void PlayBGM(int _playMusicTrack)
    {
        source.volume = BGMVolume;
        source.clip = clips[_playMusicTrack];
        source.Play();
    }

    /*볼륨 조정*/
    public void SetBGMVolume(float _volume)
    {
        BGMVolume = _volume;
        source.volume = BGMVolume;
    }

    /*음악 일시 정지*/
    public void PauseBGM()
    {
        source.Pause();
    }

    /*음악 일시 정지 해제*/
    public void UnpauseBGM()
    {
        source.UnPause();
    }

    /*음악 재생 중단*/
    public void StopBGM()
    {
        source.Stop();
    }

    /*음악 페이드 아웃*/
    public void FadeOutBGM()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutBGMCoroutine());
    }
    IEnumerator FadeOutBGMCoroutine()
    {
        for (float i = BGMVolume; i >= 0f; i -= 0.01f)
        {
            source.volume = i;
            yield return waitTime;
        }
    }

    /*음악 페이드 인*/
    public void FadeInBGM()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInBGMCoroutine());
    }
    IEnumerator FadeInBGMCoroutine()
    {
        for (float i = 0f; i <= BGMVolume; i += 0.01f)
        {
            source.volume = i;
            yield return waitTime;
        }
    }
}
