using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlanetStage : MonoBehaviour
{
    /*싱글톤*/
    public static PlanetStage instance;
    #region Singleton
    private void Awake()
    {
        if (instance == null)
        {
            //DontDestroyOnLoad(this.transform.root.gameObject);
            instance = this;

        }
        else
        {
            //Destroy(this.gameObject);
        }
    }
    #endregion Singleton    

    private List<int[]> PlanetStageDataList;

    public GameObject PlanetStagePanel;//스테이지 패널
    public GameObject StageIconPanel;//스테이지 패널 중 아이콘 패널 부분.
    public GameObject EntryRejectPopupPanel;//진입거부 팝업
    public GameObject EnterBtn;//진입 버튼


    bool isstageclicked = false;//stageicon 클릭
    int openedplanet = -1;//현재 행성
    int clickedstage = -1;//클릭된 스테이지



    void Start()
    {
        InitPlanetStageContent();
    }

    //행성 스테이지 UI의 스테이지 데이터 로드 - DB에서 정보 불러오기 (현재는 데이터 따로 없음, 임시 스크립트 연결)
    public void LoadPlanetStageData()
    {
        PlanetStageDataList = PlayerData.instance.GetStageData();
    }

    //행성 스테이지 UI의 스테이지 이미지 정보 초기화
    private void InitPlanetStageContent()
    {
        isstageclicked = false;
        ApplyPlanetStageContent();
    }
    private void ApplyPlanetStageContent()
    {
        //데이터 불러오기
        LoadPlanetStageData();

        //Planet Stage
        for (int i = 0; i < 3; i++)//Planet
        {
            GameObject cplanet_stage = StageIconPanel.transform.GetChild(i).gameObject;

            for (int j = 0; j < 5; j++)//Stage
            {
                GameObject cur_stage = cplanet_stage.transform.GetChild(j).gameObject;
                GameObject icon = cur_stage.transform.Find("Icon").gameObject;

                GameObject starframe = cur_stage.transform.Find("StarFrame").gameObject;
                GameObject star_0 = starframe.transform.Find("Star_0").gameObject;
                GameObject star_1 = starframe.transform.Find("Star_1").gameObject;
                GameObject star_2 = starframe.transform.Find("Star_2").gameObject;
                GameObject star_3 = starframe.transform.Find("Star_3").gameObject;

                if (PlanetStageDataList[i][j] >= 0)
                {
                    //스테이지 아이콘 ON
                    icon.SetActive(true);

                    starframe.gameObject.SetActive(true);

                    star_0.gameObject.SetActive(false);
                    star_0.gameObject.SetActive(false);
                    star_0.gameObject.SetActive(false);
                    star_0.gameObject.SetActive(false);

                    //별 갯수에 맞게 show
                    if (PlanetStageDataList[i][j] == 0)
                    {
                        star_0.gameObject.SetActive(true);
                    }
                    else if (PlanetStageDataList[i][j] == 1)
                    {
                        star_1.gameObject.SetActive(true);
                    }
                    else if (PlanetStageDataList[i][j] == 2)
                    {
                        star_2.gameObject.SetActive(true);
                    }
                    else if (PlanetStageDataList[i][j] == 3)
                    {
                        star_3.gameObject.SetActive(true);
                    }
                }
                else 
                {
                    icon.SetActive(false);
                    starframe.gameObject.SetActive(false);
                }
            }
        }
    }

    // 스테이지 이어하기
    public void GoNextStage(int planet_num)
    {
        for (int i = 0; i < 5; i++)//Stage
        {
            if (PlanetStageDataList[planet_num][i] < 0)//플레이 불가 스테이지
            {
                //이전 스테이지인 [planet_num][i-1] 연결 후 끝내기
                StartIngame(planet_num, i - 1);
                return;
            }
        }
        //마지막 스테이지 연결
        StartIngame(planet_num, 4);
    }

    // 스테이지 선택하기
    public void ShowPlanetStage(int stage_num)
    {
        //스테이지 UI 키기
        ShowPlanetStageContent(stage_num);

        int ui_num = 0;
        if (stage_num == 1 || stage_num == 4)
            ui_num = 0;
        else if (stage_num == 2 || stage_num == 5)
            ui_num = 1;
        else if (stage_num == 3 || stage_num == 6)
            ui_num = 2;

        GameObject curPlanetUI = PlanetStagePanel.transform.GetChild(0).transform.GetChild(ui_num).gameObject;
        curPlanetUI.SetActive(true);
    }
    public void HidePlanetStage(int stage_num)
    {
        //스테이지 UI 끄기
        HidePlanetStageContent(stage_num);

        int ui_num = 0;
        if (stage_num == 1 || stage_num == 4)
            ui_num = 0;
        else if (stage_num == 2 || stage_num == 5)
            ui_num = 1;
        else if (stage_num == 3 || stage_num == 6)
            ui_num = 2;

        GameObject curPlanetUI = PlanetStagePanel.transform.GetChild(0).transform.GetChild(ui_num).gameObject;
        curPlanetUI.SetActive(false);
    }
    public void ShowPlanetStageContent(int stage_num)
    {
        //스테이지 UI 켜기
        int ui_num = 0;
        if (stage_num == 1 || stage_num == 4)
            ui_num = 0;
        else if (stage_num == 2 || stage_num == 5)
            ui_num = 1;
        else if (stage_num == 3 || stage_num == 6)
            ui_num = 2;

        GameObject curStageUI = StageIconPanel.transform.GetChild(ui_num).gameObject;
        curStageUI.SetActive(true);
    }
    public void HidePlanetStageContent(int stage_num)
    {
        //스테이지 UI 끄기
        int ui_num = 0;
        if (stage_num == 1 || stage_num == 4)
            ui_num = 0;
        else if (stage_num == 2 || stage_num == 5)
            ui_num = 1;
        else if (stage_num == 3 || stage_num == 6)
            ui_num = 2;

        HideStageEntry();

        GameObject curStageUI = StageIconPanel.transform.GetChild(ui_num).gameObject;
        curStageUI.SetActive(false);
    }

    //스테이지 아이콘 클릭시   
    public void ShowStageEntry_P1(int stage)
    {
        if (!isstageclicked && PlanetStageDataList[0][stage] >= 0)
        {
            GameObject cplanet = StageIconPanel.transform.GetChild(0).gameObject;
            GameObject cstage = cplanet.transform.GetChild(stage).gameObject;

            cstage.transform.localScale = new Vector3(1.25f, 1.25f, 1f);
            isstageclicked = true;
            openedplanet = 0;
            clickedstage = stage;

            //입장버튼 스폰
            Vector3 iconpos = cstage.transform.position;
            Vector3 btnpos = new Vector3(iconpos.x,iconpos.y - 300f, 0f);
            EnterBtn.transform.position = btnpos;
            EnterBtn.SetActive(true);
        }
        else if(PlanetStageDataList[0][stage] == -1)
        {
            EntryRejectPopupPanel.SetActive(true);//팝업
        }
    }
    public void ShowStageEntry_P2(int stage)
    {
        if (!isstageclicked && PlanetStageDataList[1][stage] >= 0)
        {
            GameObject cplanet = StageIconPanel.transform.GetChild(1).gameObject;
            GameObject cstage = cplanet.transform.GetChild(stage).gameObject;

            cstage.transform.localScale = new Vector3(1.25f, 1.25f, 1f);
            isstageclicked = true;
            openedplanet = 1;
            clickedstage = stage;

            //입장버튼 스폰
            Vector3 iconpos = cstage.transform.position;
            Vector3 btnpos = new Vector3(iconpos.x, iconpos.y - 300f, 0f);
            EnterBtn.transform.position = btnpos;
            EnterBtn.SetActive(true);
        }
        else if (PlanetStageDataList[0][stage] == -1)
        {
            EntryRejectPopupPanel.SetActive(true);//팝업
        }
    }
    public void ShowStageEntry_P3(int stage)
    {
        if (!isstageclicked && PlanetStageDataList[2][stage] >= 0)
        {
            GameObject cplanet = StageIconPanel.transform.GetChild(2).gameObject;
            GameObject cstage = cplanet.transform.GetChild(stage).gameObject;

            cstage.transform.localScale = new Vector3(1.25f, 1.25f, 1f);
            isstageclicked = true;
            openedplanet = 2;
            clickedstage = stage;

            //입장버튼 스폰
            Vector3 iconpos = cstage.transform.position;
            Vector3 btnpos = new Vector3(iconpos.x, iconpos.y - 300f, 0f);
            EnterBtn.transform.position = btnpos;
            EnterBtn.SetActive(true);
        }
        else if (PlanetStageDataList[0][stage] == -1)
        {
            EntryRejectPopupPanel.SetActive(true);//팝업
        }
    }

    public void HideStageEntry()
    {
        if (isstageclicked)
        {
            GameObject cplanet_stage = StageIconPanel.transform.GetChild(openedplanet).gameObject;
            GameObject cstage = cplanet_stage.transform.GetChild(clickedstage).gameObject;

            //스케일 줄이기
            cstage.transform.localScale = new Vector3(1f, 1f, 1f);

            //입장버튼 디스폰
            EnterBtn.SetActive(false);

            isstageclicked = false;
            openedplanet = -1;
            clickedstage = -1;
        }
        else
        {
            EntryRejectPopupPanel.SetActive(false);//팝업
        }
    }
    public void StageEnter()
    {
        if (clickedstage == -1)
        {
            Debug.Log("스테이지 UI 오류 발생. clickedstage is -1");
        }
        else
        {
            //Debug.Log("Planet "+ openedplanet+" 입장. stage:" + clickedstage);
            StartIngame(openedplanet, clickedstage);
        }
    }

    //스테이지 진입
    private void StartIngame(int selectplanet, int selectstage)
    {
        //PlayerManager에 스테이지와 행성 전달
        PlayManager.instance.InitPlayManager();
        PlayManager.instance.SetNowStageInfo(selectplanet, selectstage);

        //씬 이동 - 스테이지 별로
        GameSceneManager.instance.MoveToIngame_Stage(selectplanet, selectstage);
    }   
}
