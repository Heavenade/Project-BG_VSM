using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SettingManager : MonoBehaviour
{
    /*싱글톤*/
    #region Singleton
    public static SettingManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.transform.root.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion Singleton

    /*자막 재생속도 상수*/
    public const float pv_1 = 0.1f;
    public const float pv_2 = 0.02f;
    public const float pv_3 = 0.005f;

    /*환경설정 변수 */
    private int bgmvolume;//(0 - 120)
    private int effectvolume; //(0 - 120)
    private int playvelocity;//(0-2)

    private int bgmstack;//0-12
    private int effectstack;//0-12

    /*환경설정 UI 패널*/
    public GameObject SettingPanel;

    /*UI 게임 오브젝트*/
    public Slider BGMSlider;
    public Slider EffectSlider;

    public GameObject BGMStack;
    public GameObject EffectStack;

    public GameObject BGMIcon;
    public GameObject EffectIcon;


    /*씬 구분용*/
    public const string MainScene = "OutGame";
    public const string GameScene = "Ingame";


    void Start()
    {
        //이전 설정 불러오기 없으면 기본 설정 적용하기 
        GetPrevSetting();
        SetCurSetting();
        SetUISetting();
    }


    /************************
     * Setting Panel UI Control
     ************************/

    public void ShowSettingPanel()
    {
        string scenename = SceneManager.GetActiveScene().name.ToString();
        if (scenename == MainScene)
        {
            Debug.Log("아웃게임 씬 설정");
            if (OutGameUIManager.instance.IsSettingAvailable())
            {
                OutGameUIManager.instance.SetSettingOpenBool(true);
                OutGameUIManager.instance.MainPanel.SetActive(false);//함수화 필요
                SettingPanel.SetActive(true);
            }
            return;
        }
        Debug.Log("인게임 씬 설정");
        SettingPanel.SetActive(true);
    }
    private IEnumerator SettingPanelControl()
    {
        yield return new WaitForSeconds(OutGameUIManager.UI_DELAY);
        OutGameUIManager.instance.SetSettingOpenBool(false);
    }
    public void HideSettingPanel()
    {
        string scenename = SceneManager.GetActiveScene().name.ToString();
        if (scenename == MainScene)
        {
            SettingPanel.SetActive(false);
            OutGameUIManager.instance.MainPanel.SetActive(true);//함수화 필요
            StartCoroutine(SettingPanelControl());
            return;
        }
        else
        {
            //UIController.instance.BtnBackToGame();
            SettingPanel.SetActive(false);
        }
    }

    /*자막 재생 속도 조정*/
    public void UpdatePlaySlow()
    {
        playvelocity = 0;
        Debug.Log("play velocity : slow");

        SetCurSetting();
    }
    public void UpdatePlayMid()
    {
        playvelocity = 1;
        Debug.Log("play velocity : mid");

        SetCurSetting();
    }
    public void UpdatePlayFast()
    {
        playvelocity = 2;
        Debug.Log("play velocity : fast");

        SetCurSetting();
    }


    /************************
     * Audio Volume Control
     ************************/
    /*설정 변경 시 자동 적용 - EventSystem On value changed 사용*/
    public void UpdateBGMVolume()
    {
        int rawvolume = Mathf.RoundToInt(BGMSlider.value);
        bgmstack = SliderValueToStack_BGM(rawvolume);
        SetStackToVolume_BGM(bgmstack);
        //Debug.Log("BGM Volume: " + bgmvolume+ " BGM Stack: "+bgmstack);

        SetUISetting();
        SetCurSetting();
    }
    public void UpdateEffectVolume()
    {
        int rawvolume = Mathf.RoundToInt(EffectSlider.value);
        effectstack = SliderValueToStack_Effect(rawvolume);
        SetStackToVolume_Effect(effectstack);
        //Debug.Log("Effect Volume: " + effectvolume + " Effect Stack: " + effectstack);

        SetUISetting();
        SetCurSetting();
    }

    private int SliderValueToStack_BGM(int volume_)
    {
        int volumestack;
        if (volume_ == 0)
        {
            volumestack = 0;
        }
        else
        {
            volumestack = volume_ / 10 + 1;
            if (volumestack >= 13)
                volumestack = 12;
        }
        return volumestack;
    }
    private int SliderValueToStack_Effect(int volume_)
    {
        int volumestack;
        if (volume_ == 0)
        {
            volumestack = 0;
        }
        else
        {
            volumestack = volume_ / 10 + 1;
            if (volumestack >= 13)
                volumestack = 12;
        }
        return volumestack;
    }

    private void SetStackToVolume_BGM(int stack_)
    {
        bgmvolume = stack_ * 10;
        if(bgmvolume > 100)
            Debug.Log("경고! BGM 볼륨이 과도하게 큽니다.");
    }
    private void SetStackToVolume_Effect(int stack_)
    {
        effectvolume = stack_ * 10;
        if (effectvolume > 100)
            Debug.Log("경고! Effect 볼륨이 과도하게 큽니다.");
    }

    private void SetStackUI_BGM()
    { 
        for (int i = 1; i <= 12; i++)
        {
            if(i <= bgmstack)
                BGMStack.transform.GetChild(i-1).gameObject.SetActive(true);
            else
                BGMStack.transform.GetChild(i-1).gameObject.SetActive(false);
        }
    }
    private void SetStackUI_Effect()
    {
        for (int i = 1; i <= 12; i++)
        {
            if (i <= effectstack)
                EffectStack.transform.GetChild(i-1).gameObject.SetActive(true);
            else
                EffectStack.transform.GetChild(i-1).gameObject.SetActive(false);
        }
    }

    private void SetIconUI_BGM()
    {
        if (bgmstack <= 0)
        {
            BGMIcon.transform.GetChild(0).gameObject.SetActive(true);
            BGMIcon.transform.GetChild(1).gameObject.SetActive(false);
            BGMIcon.transform.GetChild(2).gameObject.SetActive(false);
        }
        else if (bgmstack > 10)
        {
            BGMIcon.transform.GetChild(0).gameObject.SetActive(false);
            BGMIcon.transform.GetChild(1).gameObject.SetActive(false);
            BGMIcon.transform.GetChild(2).gameObject.SetActive(true);
            
        }
        else
        {
            BGMIcon.transform.GetChild(0).gameObject.SetActive(false);
            BGMIcon.transform.GetChild(1).gameObject.SetActive(true);
            BGMIcon.transform.GetChild(2).gameObject.SetActive(false);
        }
    }
    private void SetIconUI_Effect()
    {
        if (effectstack <= 0)
        {
            EffectIcon.transform.GetChild(0).gameObject.SetActive(true);
            EffectIcon.transform.GetChild(1).gameObject.SetActive(false);
            EffectIcon.transform.GetChild(2).gameObject.SetActive(false);
        }
        else if (effectstack > 10)
        {
            EffectIcon.transform.GetChild(0).gameObject.SetActive(false);
            EffectIcon.transform.GetChild(1).gameObject.SetActive(false);
            EffectIcon.transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            EffectIcon.transform.GetChild(0).gameObject.SetActive(false);
            EffectIcon.transform.GetChild(1).gameObject.SetActive(true);
            EffectIcon.transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    /************************
     * 저장 및 적용
     ************************/

    /*PlayerPref로부터 이전 설정 불러오기 없으면 기본 설정 적용하기 */
    public void GetPrevSetting()
    {
        /*Load or Init*/
        if (PlayerPrefs.HasKey("BGMVolume"))
        {
            bgmvolume = PlayerPrefs.GetInt("BGMVolume");
            
        }
        else
        {
            SetInitSetting_bgmvolume();
        }
        if (PlayerPrefs.HasKey("EffectVolume"))
        {
            effectvolume = PlayerPrefs.GetInt("EffectVolume");
        }
        else
        {
            SetInitSetting_effectvolume();
        }
        if (PlayerPrefs.HasKey("PlayVelocity"))
        {
            playvelocity = PlayerPrefs.GetInt("PlayVelocity");
        }
        else
        {
            SetInitSetting_playvelocity();
        }

        /*UI에 적용*/
        SetUISetting();
    }

    /*현재 설정값을 실제 데이터 PlayerPref에 저장*/
    public void SaveCurSetting()
    {
        /*Save - 암호화 X*/
        PlayerPrefs.SetFloat("BGMVolume", bgmvolume);
        PlayerPrefs.SetFloat("EffectVolume", effectvolume);
        PlayerPrefs.SetFloat("PlayVelocity", playvelocity);
    }

    /*현재 설정값을 실제 게임에 적용*/
    public void SetCurSetting()
    {
        /*볼륨*/
        BGMManager.instance.SetBGMVolume(bgmvolume*1.0f);
        EffectManager.instance.SetEffectVolume(effectvolume*1.0f);
    }

    /*현재 설정값을 시각적인 UI에 적용*/
    public void SetUISetting()
    {
        //스택과 비례해 바 UI를 조정
        SetStackUI_BGM();
        SetIconUI_BGM();
        SetStackUI_Effect();
        SetIconUI_Effect();
    }

    /*설정 초기화*/
    private void SetInitSetting_bgmvolume()
    {
        bgmstack = 5;
        bgmvolume = 50;
    }
    private void SetInitSetting_effectvolume()
    {
        effectstack = 5;
        effectvolume = 50;
    }
    private void SetInitSetting_playvelocity()
    {
        playvelocity = 1;//mid     
    }

    /*패널 여닫을 때 불러오기*/
    public void StartSetting()
    {
        GetPrevSetting();
        SetUISetting();
    }
    public void EndSetting()
    {      
        SetCurSetting();
        SaveCurSetting();
    }
}
