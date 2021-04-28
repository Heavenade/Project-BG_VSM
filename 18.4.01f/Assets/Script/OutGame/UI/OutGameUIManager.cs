using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OutGameUIManager : MonoBehaviour
{
    public static OutGameUIManager instance;
    #region Singleton
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
            InitBool();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion Singleton

    /*카메라*/
    public Camera MainCamera; //코드에서 가져오는 것으로 수정
    private Transform maintr;

    /*UI Delay*/
    public static float UI_DELAY = 0.2f;

    /*For Drags*/
    Vector2 downPos = Vector2.zero;
    Vector2 upPos = Vector2.zero;
    float prevDistance = 0f;

    /*GameObjects*/

    //코드에서 OutGameWorld를 가져와 코드로 참조하도록

    //메인
    public GameObject BaalStar;

    //수집완료 행성
    public GameObject C_Planet_1;
    public GameObject C_Planet_2;
    public GameObject C_Planet_3;

    //수집미완료 행성
    public GameObject NC_Planet_1;
    public GameObject NC_Planet_2;
    public GameObject NC_Planet_3;

    //카메라 좌표값
    private static Vector3 CamPos_EnemySys = new Vector3(17.9f, 1f, -10f);
    private static Vector3 CamPos_BaalSys = new Vector3(0f, 1f, -10f);

    /*Main UI Objects*/

    //코드에서 OutGameUI를 가져와 코드로 참조하도록

    public GameObject EnergyReinforcePanel;
    public GameObject CoreReinforcePanel;
    public GameObject PlanetStagePanel;
    public GameObject TutorialStagePanel;
    [SerializeField]
    private GameObject EnergyCalculatePanel;


    public GameObject MainPanel;
    public GameObject BaalPlanetPanel;
    public GameObject EtcPlanetPanel;

    [SerializeField]
    private GameObject Info_Const;
    [SerializeField]
    private GameObject Info_EC;

    /*조정용 bool 변수*/
    bool isbaalzoomed;//바알이 줌 인
    bool isplanetzoomed;//행성이 줌 인
    bool iscoredopened;//코어 강화 UI가 오픈
    bool isenergydopened;//에너지 강화 UI가 오픈
    bool isplanetstageopened;//행성 스테이지 UI가 오픈
    bool issettingopened;//설정 Ui가 오픈
    public bool isenergycalculating;//true 시 에너지 정산 해야함.

    /*Now System*/
    private enum NowSys { HomeSide = 0, EnemySide };
    private NowSys nowsystem = NowSys.HomeSide;

    /*레이캐스트*/
    private GameObject touchdowntarget;
    private GameObject touchuptarget;

    /*바알 클릭을 위한 줌*/
    private GameObject zoomtarget;//줌 대상 용
    private Transform zoomtr;

    /*Planet Number*/
    private enum PlanetNum { BaalPlanet = 0, C_Planet_1, C_Planet_2, C_Planet_3, NC_Planet_1, NC_Planet_2, NC_Planet_3 };
    //열린 행성 확인용
    private PlanetNum OpenedPlanet = 0;



    /*---------함수----------*/
    void Start()
    {
        Time.timeScale = 1;
        maintr = MainCamera.transform;
        StarfieldEffect.instance.StarfieldRelocate(CamPos_BaalSys);
        OutGameUIManager.instance.ApplyInfo_Const();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TouchDown();
        }
        if (Input.GetMouseButtonUp(0))
        {
            TouchUp();
        }
        if (Input.GetMouseButton(0))
        {
            OnDrag();
        }
    }

    void InitBool()
    {
        isbaalzoomed = false;
        isplanetzoomed = false;
        iscoredopened = false;
        isenergydopened = false;
        issettingopened = false;
        isplanetstageopened = false;
        isenergycalculating = false;
    }

    /************************************
     *Main UI*
     ************************************/
    #region MainUI
    /*바알 행성 줌*/
    public void BaalPlanetZoomIn(GameObject PlanetAnchor)
    {
        if (IsBaalZoomInAvailable())
        {
            zoomtarget = BaalStar;
            zoomtr = zoomtarget.GetComponent<Transform>();

            MainCamera.transform.LookAt(PlanetAnchor.transform);
            MainCamera.orthographicSize = 0.9f;

            isbaalzoomed = true;

            StarfieldEffect.instance.StarFieldZoomIn_Baal();
            StarAuraEffect.instance.StarAuraON((int)PlanetNum.BaalPlanet);

            MainPanel.SetActive(false);
            BaalPlanetPanel.SetActive(true);
        }
    }
    public void BaalPlanetZoomOut()
    { 
        if (IsBaalZoomOutAvailable())
        { 
            Vector3 mainrotate = new Vector3(0f, 0f, 0f);
            MainCamera.transform.position = maintr.position;//포지션 원래대로
            MainCamera.transform.rotation = Quaternion.Euler(mainrotate);//로테이션 원래대로
            MainCamera.orthographicSize = 5.0f;

            StarfieldEffect.instance.StarFieldZoomOut();
            StarAuraEffect.instance.StarAuraOFF();
        
            BaalPlanetPanel.SetActive(false);
            MainPanel.SetActive(true);

            StartCoroutine(BaalZoomedControl());
        }
    }
    private IEnumerator BaalZoomedControl()
    {
        yield return new WaitForSeconds(UI_DELAY);
        isbaalzoomed = false;
    }
    public bool IsBaalZoomInAvailable()
    {
        if (isplanetzoomed == true)
        { return false; }
        if (isbaalzoomed == true)
        { return false; }
        if (iscoredopened == true)
        { return false; }
        if (isenergydopened == true)
        { return false; }
        if (isplanetstageopened == true)
        { return false; }
        if (issettingopened == true)
        { return false; }
        if (isenergycalculating == true)
        { return false; }

        return true;
    }
    public bool IsBaalZoomOutAvailable()
    {
        if (isplanetzoomed == true)
        { return false; }
        if (isbaalzoomed == false)
        { return false; }
        if (iscoredopened == true)
        { return false; }
        if (isenergydopened == true)
        { return false; }
        if (isplanetstageopened == true)
        { return false; }
        if (issettingopened == true)
        { return false; }
        if (isenergycalculating == true)
        { return false; }
        return true;
    }

    /*여타 행성 줌*/
    void PlanetZoomIn(GameObject Planet, GameObject PlanetAnchor, PlanetNum num)
    {
        if (IsPlanetZoomInAvailable())
        {
            zoomtarget = Planet;
            zoomtr = zoomtarget.GetComponent<Transform>();

            MainCamera.transform.LookAt(PlanetAnchor.transform);

            if (num == PlanetNum.C_Planet_1 || num == PlanetNum.NC_Planet_1)
            {
                MainCamera.orthographicSize = 0.55f;
            }
            else if (num == PlanetNum.C_Planet_2 || num == PlanetNum.NC_Planet_2)
            {
                MainCamera.orthographicSize = 0.8f;
            }
            else if (num == PlanetNum.C_Planet_3 || num == PlanetNum.NC_Planet_3)
            {
                MainCamera.orthographicSize = 0.65f;
            }
            SetNowZoomedPlanet(num); // 지금 어떤 행성이 줌 되고 있나?
            isplanetzoomed = true;

            StarfieldEffect.instance.StarFieldZoomIn_Planet((int)num);
            StarAuraEffect.instance.StarAuraON((int)num);
         
            MainPanel.SetActive(false);
            EtcPlanetPanel.SetActive(true);
            ShowStageSelectPanel(num);
        }
    }
    void SetNowZoomedPlanet(PlanetNum num)
    {
        OpenedPlanet = num;
    }
    public void PlanetZoomOut()
    {
        if (IsPlanetZoomOutAvailable())
        {
            Vector3 mainrotate = new Vector3(0f, 0f, 0f);
            MainCamera.transform.position = maintr.position;//포지션 원래대로            

            MainCamera.transform.rotation = Quaternion.Euler(mainrotate);//로테이션 원래대로
            MainCamera.orthographicSize = 5.0f;

            StarfieldEffect.instance.StarFieldZoomOut();
            StarAuraEffect.instance.StarAuraOFF();

            HideStageSelectPanel();
            EtcPlanetPanel.SetActive(false);
            MainPanel.SetActive(true);

            StartCoroutine(PlanetZoomedControl());
        }
    }
    private IEnumerator PlanetZoomedControl()
    {
        yield return new WaitForSeconds(UI_DELAY);
        isplanetzoomed = false;
    }
    public bool IsPlanetZoomInAvailable()
    {
        if (isbaalzoomed == true)
        { return false; }
        if (isplanetzoomed == true)
        { return false; }
        if (iscoredopened == true)
        { return false; }
        if (isenergydopened == true)
        { return false; }
        if (isplanetstageopened == true)
        { return false; }
        if (issettingopened == true)
        { return false; }
        if (isenergycalculating == true)
        { return false; }

        return true;
    }
    public bool IsPlanetZoomOutAvailable()
    {
        if (isbaalzoomed == true)
        { return false; }
        if (isplanetzoomed == false)
        { return false; }
        if (iscoredopened == true)
        { return false; }
        if (isenergydopened == true)
        { return false; }
        if (isplanetstageopened == true)
        { return false; }
        if (issettingopened == true)
        { return false; }
        if (isenergycalculating == true)
        { return false; }
        return true;
    }

    private void ShowStageSelectPanel(PlanetNum planetnum)
    {
        GameObject StageSelectPanel = EtcPlanetPanel.transform.Find("StageSelectPanel").gameObject;
        if (planetnum == PlanetNum.C_Planet_1 || planetnum == PlanetNum.NC_Planet_1)
        {
            StageSelectPanel.transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (planetnum == PlanetNum.C_Planet_2 || planetnum == PlanetNum.NC_Planet_2)
        {
            StageSelectPanel.transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (planetnum == PlanetNum.C_Planet_3 || planetnum == PlanetNum.NC_Planet_3)
        {
            StageSelectPanel.transform.GetChild(2).gameObject.SetActive(true);
        }
    }
    private void HideStageSelectPanel()
    {
        GameObject StageSelectPanel = EtcPlanetPanel.transform.Find("StageSelectPanel").gameObject;
        StageSelectPanel.transform.GetChild(0).gameObject.SetActive(false);
        StageSelectPanel.transform.GetChild(1).gameObject.SetActive(false);
        StageSelectPanel.transform.GetChild(2).gameObject.SetActive(false);
    }

    /*진영간 이동*/
    public void MoveToEnemySys()
    {
        if (IsMoveSysAvailable())
        {
            StarfieldEffect.instance.StarfieldRelocate(CamPos_EnemySys);
            MainCamera.transform.position = CamPos_EnemySys;
            nowsystem = NowSys.EnemySide;
            GameObject buttons = MainPanel.transform.Find("Buttons").gameObject;
            buttons.transform.Find("ToFight").gameObject.SetActive(false);
            buttons.transform.Find("ToMain").gameObject.SetActive(true);
        }    
    }
    public void MoveToBaalSys()
    {
        if (IsMoveSysAvailable())
        {
            StarfieldEffect.instance.StarfieldRelocate(CamPos_BaalSys);
            MainCamera.transform.position = CamPos_BaalSys;
            nowsystem = NowSys.HomeSide;
            GameObject buttons = MainPanel.transform.Find("Buttons").gameObject;
            buttons.transform.Find("ToFight").gameObject.SetActive(true);
            buttons.transform.Find("ToMain").gameObject.SetActive(false);
        }
    }
    public bool IsMoveSysAvailable()
    {
        if (isplanetstageopened == true)
        { return false; }
        if (iscoredopened == true)
        { return false; }
        if (isenergydopened == true)
        { return false; }
        if (isplanetzoomed == true)
        { return false; }
        if (isbaalzoomed == true)
        { return false; }
        if (isenergycalculating == true)
        { return false; }
        return true;
    }

    /*강화 간 이동*/
    public void MoveToCoreR()
    {
        HideEnergyReinforcePanel();
        ShowCoreReinforcePanel();
    }
    public void MoveToEnergyR()
    {
        HideCoreReinforcePanel();
        ShowEnergyReinforcePanel();
    }

    /*Info Show*/
    public void ApplyInfo_Const()//start, after ec
    {
        Info_Const.transform.Find("ReinforcePoint").gameObject.GetComponent<Text>().text = BaalStatus.instance.GetReinforcePoint().ToString();
    }
    public void ApplyInfo_EC(int addpoint)
    {
        Info_EC.transform.Find("AddPoint").gameObject.GetComponent<Text>().text = addpoint.ToString();
    }
    public void ShowInfo_EC()
    {
        Info_EC.transform.gameObject.SetActive(true);

        StartCoroutine(HideInfo_EC());
    }
    IEnumerator HideInfo_EC()
    {
        yield return new WaitForSeconds(2.0f);

        ApplyInfo_Const();
        Info_EC.transform.Find("AddPoint").gameObject.GetComponent<Text>().text = "0";
        Info_EC.transform.gameObject.SetActive(false);
    }

    /*Calculation*/
    public void ShowEnergyCalculatePanel()
    {
        isenergycalculating = true;
        EnergyCalculatePanel.SetActive(true);
    }
    public void HideEnergyCalculatePanel()
    {
        isenergycalculating = false;
        EnergyCalculatePanel.SetActive(false);
    }

    #endregion MainUI
    /************************************
     *강화*
     ************************************/
    #region Reinforce
    /*바알 에너지 강화*/
    public void ShowEnergyReinforcePanel()
    { 
        if (IsEnergyRAvailable())
        { 
            isenergydopened = true;
            BaalPlanetPanel.SetActive(false);
            EnergyReinforcePanel.SetActive(true);
            TutorialStagePanelClose();
        }
    }
    private IEnumerator EnergyReinforcePanelControl()
    { 
        yield return new WaitForSeconds(UI_DELAY); 
        isenergydopened = false;
    }
    public void HideEnergyReinforcePanel()
    {
        EnergyReinforcePanel.SetActive(false); 
        StartCoroutine(EnergyReinforcePanelControl()); 
    }
    public bool IsEnergyRAvailable()
    {
        if (isplanetstageopened == true)
        { return false; }
        if (isenergydopened == true)
        { return false; }
        if (isplanetzoomed == true)
        { return false; }
        if (isbaalzoomed == false)
        { return false; }
        if (issettingopened == true)
        { return false; }


        return true;
    }

    /*바알 코어 강화*/
    public void ShowCoreReinforcePanel()
    {
        if (IsCoreRAvailable())
        {
            iscoredopened = true;
            BaalPlanetPanel.SetActive(false);
            CoreReinforcePanel.SetActive(true);
        }
    }
    private IEnumerator CoreReinforcePanelControl()
    {
        yield return new WaitForSeconds(UI_DELAY);
        iscoredopened = false;
    }
    public void HideCoreReinforcePanel()
    {
        CoreReinforcePanel.SetActive(false);
        StartCoroutine(CoreReinforcePanelControl());
    }
    public bool IsCoreRAvailable()
    {
        if (isplanetstageopened == true)
        { return false; }
        if (iscoredopened == true)
        { return false; }
        if (isplanetzoomed == true)
        { return false; }
        if (isbaalzoomed == false)
        { return false; }
        if (issettingopened == true)
        { return false; }

        return true;
    }

    //모든 강화 패널 끄기
    public void HideAllReinforcePanel()
    {
        HideEnergyReinforcePanel();
        HideCoreReinforcePanel();
        BaalPlanetPanel.SetActive(true);
    }
    #endregion Reinforce
    /************************************
    *튜토리얼 스테이지*
    ************************************/
    #region Tutorial
    public void TutorialStagePanelOpen()
    {
        if (IsTutorialStageAvailable())
        {
            TutorialStagePanel.SetActive(true);
            BaalPlanetPanel.transform.Find("BackBtn").gameObject.SetActive(false);
            BaalPlanetPanel.transform.Find("TutorialDirectBtn").gameObject.SetActive(false);
            BaalPlanetPanel.transform.Find("TutorialSelectBtn").gameObject.SetActive(false);
        }
    }
    public void TutorialStagePanelClose()
    { 
        BaalPlanetPanel.transform.Find("BackBtn").gameObject.SetActive(true);
        BaalPlanetPanel.transform.Find("TutorialDirectBtn").gameObject.SetActive(true);
        BaalPlanetPanel.transform.Find("TutorialSelectBtn").gameObject.SetActive(true);
        TutorialStagePanel.SetActive(false);
    }
    //UI DELAY 없음
    public bool IsTutorialStageAvailable()
    {
        if (isplanetstageopened == true)
        { return false; }
        if (iscoredopened == true)
        { return false; }
        if (isenergydopened == true)
        { return false; }
        if (isbaalzoomed == true)
        { return true; }
        if (issettingopened == true)
        { return false; }

        return false;
    }
    #endregion Tutorial
    /************************************
     *행성 스테이지*
     ************************************/
    #region Stage
    public void PlanetStagePanelOpen()
    {
        if (IsPlanetStageAvailable())
        {            
            PlanetStagePanel.SetActive(true);
            EtcPlanetPanel.SetActive(false);
            PlanetStage.instance.ShowPlanetStage((int)OpenedPlanet);
            isplanetstageopened = true;
        }
    }
    public void PlanetStagePanelClose()
    {
        EtcPlanetPanel.SetActive(true);
        PlanetStagePanel.SetActive(false);
        PlanetStage.instance.HidePlanetStage((int)OpenedPlanet);
        StartCoroutine(PlanetStagePanelControl());
    }
    private IEnumerator PlanetStagePanelControl()
    {
        yield return new WaitForSeconds(UI_DELAY);
        isplanetstageopened = false;
    }
    public bool IsPlanetStageAvailable()
    {
        if (isplanetstageopened == true)
        { return false; }
        if (iscoredopened == true)
        { return false; }
        if (isenergydopened == true)
        { return false; }
        if (isplanetzoomed == true)
        { return true; }
        if (issettingopened == true)
        { return false; }

        return false;
    }
    #endregion Stage
    /************************************
     *설정*
     ************************************/
    public void SettingBtn()
    {
        SettingManager.instance.ShowSettingPanel();
    }
    public void SetSettingOpenBool(bool tmp_)
    {
        if(tmp_ == false)
            issettingopened = false;
        else
            issettingopened = true;

    }
    public bool IsSettingAvailable()
    {
        if (isplanetstageopened == true)
        { return false; }
        if (iscoredopened == true)
        { return false; }
        if (isenergydopened == true)
        { return false; }
        if (isplanetzoomed == true)
        { return false; }
        if (isbaalzoomed == true)
        { return false; }
        if (issettingopened == true)
        { return false; }
        if (isenergycalculating == true)
        { return false; }

        return true;
    }
    /************************************
     *시스템*
     ************************************/
    #region System
    /*터치*/
    private void TouchDown()
    {
        touchdowntarget = GetPointedObject();
    }
    private void TouchUp()
    {
        touchuptarget = GetPointedObject();
        if (touchdowntarget == touchuptarget)
        {
            GameObject target = touchuptarget;

            //줌아웃 상태
            if (IsBaalZoomInAvailable() && IsPlanetZoomInAvailable() )
            {
                if (target == BaalStar)
                {
                    BaalPlanetZoomIn(BaalStar.transform.Find("PlanetAnchor").gameObject);
                }
                else if (target == C_Planet_1)
                {
                    PlanetZoomIn(C_Planet_1, C_Planet_1.transform.Find("PlanetAnchor").gameObject,PlanetNum.C_Planet_1);
                }
                else if (target == C_Planet_2)
                {
                    PlanetZoomIn(C_Planet_2, C_Planet_2.transform.Find("PlanetAnchor").gameObject, PlanetNum.C_Planet_2);
                }
                else if (target == C_Planet_3)
                {
                    PlanetZoomIn(C_Planet_3, C_Planet_3.transform.Find("PlanetAnchor").gameObject, PlanetNum.C_Planet_3);
                }
                //수집 이전 적대행성
                else if (target == NC_Planet_1)
                {
                    PlanetZoomIn(NC_Planet_1, NC_Planet_1.transform.Find("PlanetAnchor").gameObject, PlanetNum.NC_Planet_1);
                }
                else if (target == NC_Planet_2)
                {
                    PlanetZoomIn(NC_Planet_2, NC_Planet_2.transform.Find("PlanetAnchor").gameObject, PlanetNum.NC_Planet_2);
                }
                else if (target == NC_Planet_3)
                {
                    PlanetZoomIn(NC_Planet_3, NC_Planet_3.transform.Find("PlanetAnchor").gameObject, PlanetNum.NC_Planet_3);
                }
            }
            //바알 줌인 상태
            else
            {
                if (isbaalzoomed == true)
                {  
                }
                else if (isplanetzoomed == true)
                {
                    //PlanetZoomOut();
                }
            }
        }
        ExitDrag();
    }

    /*화면이동용 드래그*/
    public void OnDrag()
    {
        /*스마트폰 터치 - 수정 요*/
        #region
        //int touchCount = Input.touchCount;

        //if(touchCount == 1)
        //{
        //    Vector2 tmpPos = Input.GetTouch(0).position;

        //    if (prevPos == Vector2.zero)
        //    {
        //        prevPos = tmpPos;
        //        return;
        //    }

        //    Vector3 tmpvec = new Vector3((float)(tmpPos.x - prevPos.x), (float)(tmpPos.y - prevPos.y), 0f);

        //    //벡터의 길이
        //    float mag = Vector3.Magnitude(tmpvec);
        //    //벡터의 방향

        //    //방향과 길이 만족 시 헤카테 진영으로 이동
        //    //MoveToHecateSys();

        //    Debug.Log("벡터의 길이 : " + mag);

        //    prevPos = tmpPos;
        //}
        #endregion

        /*마우스 드래그*/
        Vector2 tmpPos;
        tmpPos = Input.mousePosition;

        if (downPos == Vector2.zero)
        {
            downPos = tmpPos;
            return;
        }

        Vector3 vec_x = new Vector3((float)(tmpPos.x - downPos.x), 0f, 0f);
        Vector3 vec_y = new Vector3(0f, (float)(tmpPos.y - downPos.y), 0f);

        //벡터의 방향 별 길이
        float mag_x = Vector3.Magnitude(vec_x);
        float mag_y = Vector3.Magnitude(vec_y);
      
        //시작지점과 길이에 따라 움직이기

        upPos = tmpPos;
    }
    public void ExitDrag()
    {
        //벡터의 방향과 길이
        float mag_x = (float)(upPos.x - downPos.x);
        float mag_y = (float)(upPos.y - downPos.y);

        //시작지점과 길이 조건 만족 시 실질적인 카메라 이동
        if (downPos.x > Screen.width / 2 && mag_x < 0 && -mag_x >= Screen.width / 4)
        {
            MoveToEnemySys();        
        }
        else if (downPos.x < Screen.width / 2 && mag_x > 0 && mag_x >= Screen.width / 4)
        {
            MoveToBaalSys();           
        }

        //초기화
        downPos = Vector2.zero;
        upPos = Vector2.zero;
        prevDistance = 0f;
    }

    /*레이캐스트*/
    //3D - 오브젝트에 3D 콜라이더 필요
    private GameObject GetPointedObject()
    {
        RaycastHit hit;
        GameObject target = null;
        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);

        if (true == (Physics.Raycast(ray.origin, ray.direction * 10, out hit))) 
        {
            target = hit.collider.gameObject;
            //Debug.Log(target);
        }
        return target;
    }
    //2D - 오브젝트에 2D 콜라이더 필요
    private GameObject GetClickedObject2D() 
    {
        GameObject target = null;
        Vector2 pos = MainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

        if (hit.collider != null)
        {
            target = hit.collider.gameObject; 
        }
        return target;
    }
    #endregion System
}
