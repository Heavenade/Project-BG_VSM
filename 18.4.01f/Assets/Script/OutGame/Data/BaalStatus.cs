using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*바알 스텟 관련 데이터*/
/*현재 코드는 데이터 외부저장을 활용하지 않아, 인 게임 내에서만 반영 - 이후 JSON으로 */

public class BaalStatus : MonoBehaviour
{
    /*싱글톤*/
    public static BaalStatus instance;
    #region Singleton
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.transform.root.gameObject);
            instance = this;
            InitStatus();//스텟 초기값
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion Singleton

    /*바알 에너지*/
    private int BaalEnergy = 0;
    private const int BaalEnergyLimit = 100;//최대 에너지 - 100 에너지가 1 강화 포인트

    /*바알 강화 포인트*/
    public int ReinforcePoint = 0;
    private const int ReinforcePointLimit = 50;//최대 강화 포인트

    /*유저에게 공개되는 수치 - 명확한 구분을 위해 대문자 표기*/
    #region OpenStatus
    private int Stat_ATK_LV = 0;//0-5
    private static float Stat_ATK_LIM;
  
    private int Stat_HP_LV = 0;
    private static float Stat_HP_LIM;

    private int Stat_GLUT_LV = 0;
    private static float Stat_GLUT_LIM;


    private int Stat_SPEED_LV = 0;
    private static float Stat_SPEED_LIM;
    #endregion OpenStatus

    /*유저에게 공개되지 않는 수치 - 실스텟값, 기본값, 증가폭, 최대 레벨*/
    #region HiddenStatus

    //기본값, 증가폭, 최대 레벨
    private static int StatLevelLim;//에너지 강화 스텟 최대레벨

    //ATK
    private float Stat_ATK_Value;//★공격력

    private static float Stat_Atk_Default; //기본 공격력
    private static float Stat_Atk_IncRate; //공격력 증가폭

    //HP
    private float Stat_HP_Value;//★체력

    private static float Stat_Hp_Default; //기본 체력
    private static float Stat_Hp_IncRate; //체력 증가폭

    //GLUT
    private float Stat_Recover_Value;//★회복량

    private static float Stat_Recover_Default; //기본 회복량
    private float Stat_RecoverRate = 0.0f; //회복량 증가폭

    private float Stat_SpeedDecTime = 0.0f;//★이속감소 시간

    private float Stat_Speed_DecTime_Default = 0.0f; //기본 이속감소 시간
    private float Stat_Speed_DecTimeRate = 0.0f; //이속 감소 시간폭

    //SPEED
    private float Stat_SPEED_Value = 0.0f;//★이동속도

    private static float Stat_Speed_Default;//기본 이동속도
    private float Stat_Speed_IncRate = 0.0f;//이속 증가폭
    private float Stat_Speed_DecRate = 0.0f; //이속 감소폭

    #endregion HiddenStatus

    /*기본 함수*/
    private void Start()
    {   
        UpdateStatus();
    }


    /*스탯 초기화 - awake 삽입*/
    private void InitStatus()
    {
        /*스탯 정보 초기값을 적용 - 이후 데이터 로드로 변경*/
        /*바알 에너지*/
        //BaalEnergy = 0; 데이터에서 가져오기
        BaalEnergy = 0;
        /*바알 강화 포인트*/
        //ReinforcePoint 데이터에서 가져오기

        /*바알 스텟 - 실스텟 / 레벨 / 레벨 최대치*/
        Stat_ATK_LV = 0;//0-5
        Stat_ATK_LIM = 200;

        Stat_HP_LV = 0;
        Stat_HP_LIM = 200;
       
        Stat_GLUT_LV = 0;
        Stat_GLUT_LIM = 5;
        
        Stat_SPEED_LV = 0;
        Stat_SPEED_LIM = 5;
    
        //기본값, 증가폭, 최대 레벨
        StatLevelLim = 5;//에너지 강화 스텟 최대레벨

        Stat_ATK_Value = 100;//공격력
        Stat_Atk_Default = 100; //기본 공격력
        Stat_Atk_IncRate = 0.1f; //공격력 증가폭 (10%)

        Stat_HP_Value = 100;//체력
        Stat_Hp_Default = 100; //기본 체력
        Stat_Hp_IncRate = 0.2f; //체력 증가폭

        Stat_Recover_Value = 100;//회복량 - 기획에게 물어서 수정
        Stat_Recover_Default = 100; //기본 회복량 - 기획에게 물어서 수정
        Stat_RecoverRate = 0.2f; //회복량 증가폭

        Stat_SpeedDecTime = 20f;//★이속감소 시간
        Stat_Speed_DecTime_Default = 20f; //기본 이속 감소 시간 - 기획에게 물어서 수정
        Stat_Speed_DecTimeRate = 20f; //이속 감소 시간폭 (100%->0%) - 기획에게 물어서 수정

        Stat_SPEED_Value = 0;//이속
        Stat_Speed_Default = 100; //기본 이동속도 - 기획에게 물어서 수정
        Stat_Speed_IncRate = 0.05f; //이속 증가폭
        Stat_Speed_DecRate = 0.1f; //이속 감소폭(고정값) - 기획에게 물어서 수정
    }

    /*스텟 업데이트*/
    private void UpdateStatus()
    {
        /*스탯 정보 재계산해서 반영 - 이후 데이터 저장으로 변경*/
        UpdateATK();
        UpdateHP();
        UpdateRecover();
        UpdateSPEEDDecTime();
        UpdateSPEED();

        /*화면 재출력 함수 연결*/
        EnergyReinforce.instance.PrintEnergyReinforcePanel();
    }
    private void UpdateATK()
    {
        Stat_ATK_Value = Stat_Atk_Default + (Stat_Atk_Default * Stat_Atk_IncRate * Stat_ATK_LV);
    }
    private void UpdateHP()
    {
        Stat_HP_Value = Stat_Hp_Default + (Stat_Hp_Default * Stat_Hp_IncRate * Stat_HP_LV);
    }
    private void UpdateRecover()
    {
        Stat_Recover_Value = Stat_Recover_Default + (Stat_Recover_Default * Stat_RecoverRate * Stat_GLUT_LV);
    }
    private void UpdateSPEEDDecTime()
    {
        Stat_SpeedDecTime = Stat_Speed_Default - (Stat_Speed_Default * Stat_Speed_DecTimeRate * Stat_GLUT_LV);
    }
    private void UpdateSPEED()
    {
        Stat_SPEED_Value = Stat_Speed_Default + (Stat_Speed_Default * Stat_Speed_IncRate * Stat_SPEED_LV);
    }

    /*스텟 업데이트 - 코어 반영*/
    private void ApplyCoreStatus()
    {
        //BaalCore의 함수를 호출
    }

    /*스텟 정보 전달 함수*/
    public int GetBaalEnergy()
    {
        return BaalEnergy;
    }
    public void SetBaalEnergy(int _energy)
    {
        BaalEnergy += _energy;
    }
    public int GetBaalEnergyLimit()
    {
        return BaalEnergyLimit;
    }
    public void SetReinforcePoint(int point)
    {
        ReinforcePoint += point;
    }
    public int GetReinforcePoint()
    {
        return ReinforcePoint;
    }
    public int GetReinforcePointLimit()
    {
        return ReinforcePointLimit;
    }

    //실스텟
    public float GetATKValue()
    {
        return Stat_ATK_Value;
    }
    public float GetHPValue()
    {
        return Stat_HP_Value;
    }
    public float GetGLUTValue()
    {
        return Stat_Recover_Value;
    }
    public float GetSPEEDValue()
    {
        return Stat_SPEED_Value;
    }

    //레벨 - 화면 투사용
    public int GetATKLevel()
    {
        return Stat_ATK_LV;
    }
    public int GetHPLevel()
    {
        return Stat_HP_LV;
    }
    public int GetGLUTLevel()
    {
        return Stat_GLUT_LV;
    }
    public int GetSPEEDLevel()
    {
        return Stat_SPEED_LV;
    }
    public int GetStatLimit()
    {
        return StatLevelLim;
    }

    //실스텟★
    #region Get Calculated Status
    public float GetATKStat()
    {
        return Stat_ATK_Value;
    }
    public float GetHPStat()
    {
        return Stat_HP_Value;
    }
    public float GetRecoverStat()
    {
        return Stat_Recover_Value;
    }
    public float GetSPEEDDecTimeStat()
    {
        return Stat_SpeedDecTime;
    }
    public float GetSPEEDStat()
    {
        return Stat_SPEED_Value;
    }
    #endregion Get Calculated Status

    /*스텟 업그레이드 함수*/
    #region Status Upgrade
    public void UpgradeATK()
    {
        Stat_ATK_LV += 1;
        ReinforcePoint -= 1;
        UpdateStatus();
    }
    public void UpgradeHP()
    {
        Stat_HP_LV += 1;
        ReinforcePoint -= 1;
        UpdateStatus();
    }
    public void UpgradeGLUT()
    {
        Stat_GLUT_LV += 1;
        ReinforcePoint -= 1;
        UpdateStatus();
    }
    public void UpgradeSPEED()
    {
        Stat_SPEED_LV += 1;
        ReinforcePoint -= 1;
        UpdateStatus();
    }
    #endregion Status Upgrade


    /*플레이어 레벨 계산해서 반환*/
    public int GetPlayerLevel()
    {
        int P_Level = Stat_ATK_LV + Stat_GLUT_LV + Stat_HP_LV + Stat_SPEED_LV;
        return P_Level;
    }
}
