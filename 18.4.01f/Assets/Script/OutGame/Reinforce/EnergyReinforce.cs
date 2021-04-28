using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyReinforce : MonoBehaviour
{
    /*싱글톤*/
    public static EnergyReinforce instance;
    #region Singleton
    private void Awake()
    {
        if (instance == null)
        {
            //DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            //Destroy(this.gameObject);
        }
    }
    #endregion Singleton

    /*강화 패널*/
    public GameObject ConfirmPanel_E;
    public GameObject RejectPanel_E;
    public GameObject PointLackPopup;

    /*강화 포인트 패널*/
    public GameObject ED_Info;
    public GameObject ED_Info_Sub;

    /*화면출력용 텍스트 변수*/
    public Text ATK_LV;
    public Text HP_LV;
    public Text GLUT_LV;
    public Text SPEED_LV;

    /*MAX출력용 텍스트 변수*/
    public Text ATK_BtnText;
    public Text HP_BtnText;
    public Text GLUT_BtnText;
    public Text SPEED_BtnText;

    //에너지 강화 확인용
    public GameObject ConfirmPanel_E_ATK;
    public GameObject ConfirmPanel_E_HP;
    public GameObject ConfirmPanel_E_GLUT;
    public GameObject ConfirmPanel_E_SPEED;

    //에너지 강화 거부 확인용
    public GameObject RejectPanel_E_ATK;
    public GameObject RejectPanel_E_HP;
    public GameObject RejectPanel_E_GLUT;
    public GameObject RejectPanel_E_SPEED;


    public enum E_StatusType { ATK = 0, HP, GLUT, SPEED };


    /*Reinforce Point 출력*/
    private void ShowReinforcePoint()
    {
        ED_Info.SetActive(true);
    }
    private void HideReinforcePoint()
    {
        ED_Info.SetActive(false);
    }
    private void ShowReinforcePoint_Sub()
    {
        ED_Info_Sub.SetActive(true);
    }
    private void HideReinforcePoint_Sub()
    {
        ED_Info_Sub.SetActive(false);
    }

    /* 에너지 강화 UI 창에 바알 에너지 강화 스테이터스 출력 */
    public void PrintEnergyReinforcePanel()
    {
        //스탯 레벨 받아와 출력
        ATK_LV.text = BaalStatus.instance.GetATKLevel().ToString();
        HP_LV.text = BaalStatus.instance.GetHPLevel().ToString();
        GLUT_LV.text = BaalStatus.instance.GetGLUTLevel().ToString();
        SPEED_LV.text = BaalStatus.instance.GetSPEEDLevel().ToString();

        ED_Info.transform.Find("ReinforcePoint").GetComponent<Text>().text = BaalStatus.instance.GetReinforcePoint().ToString();
        ED_Info_Sub.transform.Find("ReinforcePoint").GetComponent<Text>().text = BaalStatus.instance.GetReinforcePoint().ToString();
    }

    /* 에너지 강화 UI Confirm창에 레벨과 RP 출력 */
    public void PrintConfirmPanel(E_StatusType statustype)
    {
        switch (statustype)
        {
            case E_StatusType.ATK:
                PrintConfirmPanel_ATK();
                break;
            case E_StatusType.HP:
                PrintConfirmPanel_HP();
                break;
            case E_StatusType.GLUT:
                PrintConfirmPanel_GLUT();
                break;
            case E_StatusType.SPEED:
                PrintConfirmPanel_SPEED();
                break;
        }
    }
    private void PrintConfirmPanel_ATK()
    {
        //정보 반영
        Text Level_Now = ConfirmPanel_E.transform.Find("ConfirmPanel_E_ATK").gameObject.transform.Find("DataTexts").gameObject.transform.Find("Level_Now").gameObject.GetComponent<Text>();
        Text Level_Next = ConfirmPanel_E.transform.Find("ConfirmPanel_E_ATK").gameObject.transform.Find("DataTexts").gameObject.transform.Find("Level_Next").gameObject.GetComponent<Text>();
        Text RP_Now = ConfirmPanel_E.transform.Find("ConfirmPanel_E_ATK").gameObject.transform.Find("DataTexts").gameObject.transform.Find("RP_Now").gameObject.GetComponent<Text>();
        Text RP_Next = ConfirmPanel_E.transform.Find("ConfirmPanel_E_ATK").gameObject.transform.Find("DataTexts").gameObject.transform.Find("RP_Next").gameObject.GetComponent<Text>();

        Level_Now.text = BaalStatus.instance.GetATKLevel().ToString();
        Level_Next.text = (BaalStatus.instance.GetATKLevel()+1).ToString();
        RP_Now.text = BaalStatus.instance.GetReinforcePoint().ToString();
        RP_Next.text = (BaalStatus.instance.GetReinforcePoint()-1).ToString();
        
        //UI 반영
        Button YesBtn = ConfirmPanel_E.transform.Find("ConfirmPanel_E_ATK").gameObject.transform.Find("YesBtn").gameObject.GetComponent<Button>();

        if (BaalStatus.instance.GetReinforcePoint() <= 0)
        {
            YesBtn.interactable = false;
        }
        else
        {
            YesBtn.interactable = true;
        }
    }
    private void PrintConfirmPanel_HP()
    {
        Text Level_Now = ConfirmPanel_E.transform.Find("ConfirmPanel_E_HP").gameObject.transform.Find("DataTexts").gameObject.transform.Find("Level_Now").gameObject.GetComponent<Text>();
        Text Level_Next = ConfirmPanel_E.transform.Find("ConfirmPanel_E_HP").gameObject.transform.Find("DataTexts").gameObject.transform.Find("Level_Next").gameObject.GetComponent<Text>();
        Text RP_Now = ConfirmPanel_E.transform.Find("ConfirmPanel_E_HP").gameObject.transform.Find("DataTexts").gameObject.transform.Find("RP_Now").gameObject.GetComponent<Text>();
        Text RP_Next = ConfirmPanel_E.transform.Find("ConfirmPanel_E_HP").gameObject.transform.Find("DataTexts").gameObject.transform.Find("RP_Next").gameObject.GetComponent<Text>();

        Level_Now.text = BaalStatus.instance.GetHPLevel().ToString();
        Level_Next.text = (BaalStatus.instance.GetHPLevel() + 1).ToString();
        RP_Now.text = BaalStatus.instance.GetReinforcePoint().ToString();
        RP_Next.text = (BaalStatus.instance.GetReinforcePoint() - 1).ToString();

        //UI 반영
        Button YesBtn = ConfirmPanel_E.transform.Find("ConfirmPanel_E_HP").gameObject.transform.Find("YesBtn").gameObject.GetComponent<Button>();

        if (BaalStatus.instance.GetReinforcePoint() <= 0)
        {
            YesBtn.interactable = false;
        }
        else
        {
            YesBtn.interactable = true;
        }
    }
    private void PrintConfirmPanel_GLUT()
    {
        Text Level_Now = ConfirmPanel_E.transform.Find("ConfirmPanel_E_GLUT").gameObject.transform.Find("DataTexts").gameObject.transform.Find("Level_Now").gameObject.GetComponent<Text>();
        Text Level_Next = ConfirmPanel_E.transform.Find("ConfirmPanel_E_GLUT").gameObject.transform.Find("DataTexts").gameObject.transform.Find("Level_Next").gameObject.GetComponent<Text>();
        Text RP_Now = ConfirmPanel_E.transform.Find("ConfirmPanel_E_GLUT").gameObject.transform.Find("DataTexts").gameObject.transform.Find("RP_Now").gameObject.GetComponent<Text>();
        Text RP_Next = ConfirmPanel_E.transform.Find("ConfirmPanel_E_GLUT").gameObject.transform.Find("DataTexts").gameObject.transform.Find("RP_Next").gameObject.GetComponent<Text>();

        Level_Now.text = BaalStatus.instance.GetGLUTLevel().ToString();
        Level_Next.text = (BaalStatus.instance.GetGLUTLevel() + 1).ToString();
        RP_Now.text = BaalStatus.instance.GetReinforcePoint().ToString();
        RP_Next.text = (BaalStatus.instance.GetReinforcePoint() - 1).ToString();

        //UI 반영
        Button YesBtn = ConfirmPanel_E.transform.Find("ConfirmPanel_E_GLUT").gameObject.transform.Find("YesBtn").gameObject.GetComponent<Button>();
        if (BaalStatus.instance.GetReinforcePoint() <= 0)
        {
            YesBtn.interactable = false;
        }
        else
        {
            YesBtn.interactable = true;
        }
    }
    private void PrintConfirmPanel_SPEED()
    {
        Text Level_Now = ConfirmPanel_E.transform.Find("ConfirmPanel_E_SPEED").gameObject.transform.Find("DataTexts").gameObject.transform.Find("Level_Now").gameObject.GetComponent<Text>();
        Text Level_Next = ConfirmPanel_E.transform.Find("ConfirmPanel_E_SPEED").gameObject.transform.Find("DataTexts").gameObject.transform.Find("Level_Next").gameObject.GetComponent<Text>();
        Text RP_Now = ConfirmPanel_E.transform.Find("ConfirmPanel_E_SPEED").gameObject.transform.Find("DataTexts").gameObject.transform.Find("RP_Now").gameObject.GetComponent<Text>();
        Text RP_Next = ConfirmPanel_E.transform.Find("ConfirmPanel_E_SPEED").gameObject.transform.Find("DataTexts").gameObject.transform.Find("RP_Next").gameObject.GetComponent<Text>();

        Level_Now.text = BaalStatus.instance.GetSPEEDLevel().ToString();
        Level_Next.text = (BaalStatus.instance.GetSPEEDLevel() + 1).ToString();
        RP_Now.text = BaalStatus.instance.GetReinforcePoint().ToString();
        RP_Next.text = (BaalStatus.instance.GetReinforcePoint() - 1).ToString();

        //버튼 반영
        Button YesBtn = ConfirmPanel_E.transform.Find("ConfirmPanel_E_SPEED").gameObject.transform.Find("YesBtn").gameObject.GetComponent<Button>();
        if (BaalStatus.instance.GetReinforcePoint() <= 0)
        {
            YesBtn.interactable = false;
        }
        else
        {
            YesBtn.interactable = true;
        }
    }

    /* 에너지 강화 UI Reject창에 레벨과 RP 출력 */
    public void PrintRejectPanel(E_StatusType statustype)
    {
        switch (statustype)
        {
            case E_StatusType.ATK:
                PrintRejectPanel_ATK();
                break;
            case E_StatusType.HP:
                PrintRejectPanel_HP();
                break;
            case E_StatusType.GLUT:
                PrintRejectPanel_GLUT();
                break;
            case E_StatusType.SPEED:
                PrintRejectPanel_SPEED();
                break;
        }
    }
    private void PrintRejectPanel_ATK()
    {
        Text Level_Now = RejectPanel_E.transform.Find("RejectPanel_E_ATK").gameObject.transform.Find("DataTexts").gameObject.transform.Find("Level_Now").gameObject.GetComponent<Text>();
        Text RP_Now = RejectPanel_E.transform.Find("RejectPanel_E_ATK").gameObject.transform.Find("DataTexts").gameObject.transform.Find("RP_Now").gameObject.GetComponent<Text>();

        Level_Now.text = BaalStatus.instance.GetATKLevel().ToString();
        RP_Now.text = BaalStatus.instance.GetReinforcePoint().ToString();
    }
    private void PrintRejectPanel_HP()
    {
        Text Level_Now = RejectPanel_E.transform.Find("RejectPanel_E_HP").gameObject.transform.Find("DataTexts").gameObject.transform.Find("Level_Now").gameObject.GetComponent<Text>();
        Text RP_Now = RejectPanel_E.transform.Find("RejectPanel_E_HP").gameObject.transform.Find("DataTexts").gameObject.transform.Find("RP_Now").gameObject.GetComponent<Text>();

        Level_Now.text = BaalStatus.instance.GetHPLevel().ToString();
        RP_Now.text = BaalStatus.instance.GetReinforcePoint().ToString();
    }
    private void PrintRejectPanel_GLUT()
    {
        Text Level_Now = RejectPanel_E.transform.Find("RejectPanel_E_GLUT").gameObject.transform.Find("DataTexts").gameObject.transform.Find("Level_Now").gameObject.GetComponent<Text>();
        Text RP_Now = RejectPanel_E.transform.Find("RejectPanel_E_GLUT").gameObject.transform.Find("DataTexts").gameObject.transform.Find("RP_Now").gameObject.GetComponent<Text>();

        Level_Now.text = BaalStatus.instance.GetGLUTLevel().ToString();
        RP_Now.text = BaalStatus.instance.GetReinforcePoint().ToString();
    }
    private void PrintRejectPanel_SPEED()
    {
        Text Level_Now = RejectPanel_E.transform.Find("RejectPanel_E_SPEED").gameObject.transform.Find("DataTexts").gameObject.transform.Find("Level_Now").gameObject.GetComponent<Text>();
        Text RP_Now = RejectPanel_E.transform.Find("RejectPanel_E_SPEED").gameObject.transform.Find("DataTexts").gameObject.transform.Find("RP_Now").gameObject.GetComponent<Text>();

        Level_Now.text = BaalStatus.instance.GetSPEEDLevel().ToString();
        RP_Now.text = BaalStatus.instance.GetReinforcePoint().ToString();
    }

    /*PointLack Popup*/
    public void ShowPointLackPopup()
    {
        PointLackPopup.SetActive(true);
    }
    public void HidePointLackPopup()
    {
        PointLackPopup.SetActive(false);
    }

    /*강화 버튼 연결 - 레벨 최대치 달성 검사 후 확인 or 거부 패널 띄우기*/
    public void showDetailPanel_E_ATK()
    {
        if (BaalStatus.instance.GetATKLevel() >= BaalStatus.instance.GetStatLimit())//Max
        {
            //서브패널 활성화
            RejectPanel_E_ATK.transform.Find("MaxInfoTexts").gameObject.SetActive(true);
            RejectPanel_E_ATK.transform.Find("LackInfoTexts").gameObject.SetActive(false);

            ShowRejectPanel_E_ATK();
        }
        else if (BaalStatus.instance.ReinforcePoint <= 0)//PointLack
        {
            //서브패널 활성화
            RejectPanel_E_ATK.transform.Find("MaxInfoTexts").gameObject.SetActive(false);
            RejectPanel_E_ATK.transform.Find("LackInfoTexts").gameObject.SetActive(true);

            ShowRejectPanel_E_ATK();

            //포인트랙 팝업
            ShowPointLackPopup();
        }
        else
        {
            //Debug.Log("Upgradable");
            ShowConfirmPanel_E_ATK();
        }
        ShowReinforcePoint_Sub();
        HideReinforcePoint();
    }
    public void showDetailPanel_E_HP()
    {
        if (BaalStatus.instance.GetHPLevel() >= BaalStatus.instance.GetStatLimit())//Max
        {
            //서브패널 활,비활성화
            RejectPanel_E_HP.transform.Find("MaxInfoTexts").gameObject.SetActive(true);
            RejectPanel_E_HP.transform.Find("LackInfoTexts").gameObject.SetActive(false);

            //본 패널 활성화
            ShowRejectPanel_E_HP();
        }
        else if (BaalStatus.instance.ReinforcePoint <= 0)//PointLack
        {
            //서브패널 활,비활성화
            RejectPanel_E_HP.transform.Find("MaxInfoTexts").gameObject.SetActive(false);
            RejectPanel_E_HP.transform.Find("LackInfoTexts").gameObject.SetActive(true);

            //본 패널 활성화
            ShowRejectPanel_E_HP();

            //포인트랙 팝업
            ShowPointLackPopup();
        }
        else
        {
            ShowConfirmPanel_E_HP();
        }
        ShowReinforcePoint_Sub();
        HideReinforcePoint();
    }
    public void showDetailPanel_E_GLUT()
    {
        if (BaalStatus.instance.GetGLUTLevel() >= BaalStatus.instance.GetStatLimit())//Max
        {
            //서브패널 활,비활성화
            RejectPanel_E_GLUT.transform.Find("MaxInfoTexts").gameObject.SetActive(true);
            RejectPanel_E_GLUT.transform.Find("LackInfoTexts").gameObject.SetActive(false);

            //본 패널 활성화
            ShowRejectPanel_E_GLUT();
        }
        else if (BaalStatus.instance.ReinforcePoint <= 0)//PointLack
        {
            //서브패널 활,비활성화
            RejectPanel_E_GLUT.transform.Find("MaxInfoTexts").gameObject.SetActive(false);
            RejectPanel_E_GLUT.transform.Find("LackInfoTexts").gameObject.SetActive(true);

            //본 패널 활성화
            ShowRejectPanel_E_GLUT();

            //포인트랙 팝업
            ShowPointLackPopup();

        }
        else
        {
            ShowConfirmPanel_E_GLUT();
        }
        ShowReinforcePoint_Sub();
        HideReinforcePoint();
    }
    public void showDetailPanel_E_SPEED()
    {
        if (BaalStatus.instance.GetSPEEDLevel() >= BaalStatus.instance.GetStatLimit())//Max
        {
            //서브패널 활,비활성화
            RejectPanel_E_SPEED.transform.Find("MaxInfoTexts").gameObject.SetActive(true);
            RejectPanel_E_SPEED.transform.Find("LackInfoTexts").gameObject.SetActive(false);

            //본 패널 활성화
            ShowRejectPanel_E_SPEED();
        }
        else if (BaalStatus.instance.ReinforcePoint <= 0)//PointLack
        {
            //서브패널 활,비활성화
            RejectPanel_E_SPEED.transform.Find("MaxInfoTexts").gameObject.SetActive(false);
            RejectPanel_E_SPEED.transform.Find("LackInfoTexts").gameObject.SetActive(true);

            //본 패널 활성화
            ShowRejectPanel_E_SPEED();

            //포인트랙 팝업
            ShowPointLackPopup();
        }
        else
        {
            ShowConfirmPanel_E_SPEED();
        }
        ShowReinforcePoint_Sub();
        HideReinforcePoint();
    }

    //에너지 강화 확인
    private void ShowConfirmPanel_E_ATK()
    {
        PrintConfirmPanel(E_StatusType.ATK);
        ConfirmPanel_E_ATK.SetActive(true);
        ConfirmPanel_E.SetActive(true);
    }
    private void ShowConfirmPanel_E_HP()
    {
        PrintConfirmPanel(E_StatusType.HP);
        ConfirmPanel_E_HP.SetActive(true);
        ConfirmPanel_E.SetActive(true);
    }
    private void ShowConfirmPanel_E_GLUT()
    {
        PrintConfirmPanel(E_StatusType.GLUT);
        ConfirmPanel_E_GLUT.SetActive(true);
        ConfirmPanel_E.SetActive(true);
    }
    private void ShowConfirmPanel_E_SPEED()
    {
        PrintConfirmPanel(E_StatusType.SPEED);
        ConfirmPanel_E_SPEED.SetActive(true);
        ConfirmPanel_E.SetActive(true);
    }
    public void HideConfirmPanel_E()
    {
        ConfirmPanel_E_ATK.SetActive(false);
        ConfirmPanel_E_HP.SetActive(false);
        ConfirmPanel_E_GLUT.SetActive(false);
        ConfirmPanel_E_SPEED.SetActive(false);

        ConfirmPanel_E.SetActive(false);

        ShowReinforcePoint();
        HideReinforcePoint_Sub();
    }

    /*스텟 업그레이드 버튼 함수 - 업그레이드 YES 버튼으로 호출*/
    public void UpgradeATK_Btn()
    {
        //업그레이드 후 맥스 처리를 위해 elseif 쓰지 않음
        //가능
        if (BaalStatus.instance.GetATKLevel() < BaalStatus.instance.GetStatLimit())
        {
            BaalStatus.instance.UpgradeATK();
            HideConfirmPanel_E();

            if (BaalStatus.instance.GetATKLevel() >= BaalStatus.instance.GetStatLimit())//Max
            {
                ATK_BtnText.text = "MAX";

                //서브패널 활,비활성화
                RejectPanel_E_ATK.transform.Find("MaxInfoTexts").gameObject.SetActive(true);
                RejectPanel_E_ATK.transform.Find("LackInfoTexts").gameObject.SetActive(false);
                //본 패널 활성화
                ShowRejectPanel_E_ATK();
            }
        }
    }
    public void UpgradeHP_Btn()
    {
        //가능
        if (BaalStatus.instance.GetHPLevel() < BaalStatus.instance.GetStatLimit())
        {
            BaalStatus.instance.UpgradeHP();
            HideConfirmPanel_E();

            if (BaalStatus.instance.GetHPLevel() >= BaalStatus.instance.GetStatLimit())//Max
            {
                HP_BtnText.text = "MAX";

                //서브패널 활,비활성화
                RejectPanel_E_HP.transform.Find("MaxInfoTexts").gameObject.SetActive(true);
                RejectPanel_E_HP.transform.Find("LackInfoTexts").gameObject.SetActive(false);
                //본 패널 활성화
                ShowRejectPanel_E_HP();
            }
        }
    }
    public void UpgradeGLUT_Btn()
    {
        //가능
        if (BaalStatus.instance.GetGLUTLevel() < BaalStatus.instance.GetStatLimit())
        {
            BaalStatus.instance.UpgradeGLUT();
            HideConfirmPanel_E();

            if (BaalStatus.instance.GetGLUTLevel() >= BaalStatus.instance.GetStatLimit())//Max
            {
                GLUT_BtnText.text = "MAX";

                //서브패널 활,비활성화
                RejectPanel_E_GLUT.transform.Find("MaxInfoTexts").gameObject.SetActive(true);
                RejectPanel_E_GLUT.transform.Find("LackInfoTexts").gameObject.SetActive(false);
                //본 패널 활성화
                ShowRejectPanel_E_GLUT();
            }
        }
    }
    public void UpgradeSPEED_Btn()
    {
        if (BaalStatus.instance.GetSPEEDLevel() < BaalStatus.instance.GetStatLimit())
        {
            BaalStatus.instance.UpgradeSPEED();
            HideConfirmPanel_E();

            if (BaalStatus.instance.GetSPEEDLevel() >= BaalStatus.instance.GetStatLimit())//Max
            {
                SPEED_BtnText.text = "MAX";

                //서브패널 활,비활성화
                RejectPanel_E_SPEED.transform.Find("MaxInfoTexts").gameObject.SetActive(true);
                RejectPanel_E_SPEED.transform.Find("LackInfoTexts").gameObject.SetActive(false);

                //본 패널 활성화
                ShowRejectPanel_E_SPEED();
            }
        }
    }

    //에너지 강화 거부
    private void ShowRejectPanel_E_ATK()
    {
        PrintRejectPanel(E_StatusType.ATK);
        RejectPanel_E_ATK.SetActive(true);
        RejectPanel_E.SetActive(true);
    }
    private void ShowRejectPanel_E_HP()
    {
        PrintRejectPanel(E_StatusType.HP);
        RejectPanel_E_HP.SetActive(true);
        RejectPanel_E.SetActive(true);
    }
    private void ShowRejectPanel_E_GLUT()
    {
        PrintRejectPanel(E_StatusType.GLUT);
        RejectPanel_E_GLUT.SetActive(true);
        RejectPanel_E.SetActive(true);
    }
    private void ShowRejectPanel_E_SPEED()
    {
        PrintRejectPanel(E_StatusType.SPEED);
        RejectPanel_E_SPEED.SetActive(true);
        RejectPanel_E.SetActive(true);
    }
    public void HideRejectPanel_E()
    {
        //패널 자체 비활성화
        RejectPanel_E_ATK.SetActive(false);
        RejectPanel_E_HP.SetActive(false);
        RejectPanel_E_GLUT.SetActive(false);
        RejectPanel_E_SPEED.SetActive(false);

        RejectPanel_E.SetActive(false);

        ShowReinforcePoint();
        HideReinforcePoint_Sub();
    }


}
