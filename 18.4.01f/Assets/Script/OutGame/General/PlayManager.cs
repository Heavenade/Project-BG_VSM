using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*현재 플레이 되는 게임의 매니지먼트 - 인게임, 아웃게임의 중간다리 역할로 인게임, 아웃게임에서 켜져있음*/

public class PlayManager : MonoBehaviour
{
    #region Singleton
    public static PlayManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.transform.root.gameObject);
            instance = this;
        }
        else
        {
            //Destroy(this.gameObject);
        }
    }
    #endregion Singleton    

    //변수 - 현재 플레이하는 스테이지 정보
    private int NowStage = -1;
    private int NowPlanet = -1;

    private bool[] StageChallenge = {false,false,false};//세 개의 도전과제 클리어 배열

    private int tmpEnergy = 0;//인게임 플레이 중 획득한 에너지

    //아웃게임 -> 인게임 시 
    public void InitPlayManager()
    {
        NowStage = -1;
        NowPlanet = -1;
        tmpEnergy = 0;

        StageChallenge[0] = false;
        StageChallenge[1] = false;
        StageChallenge[2] = false;
    }

    //아웃게임 -> 인게임 시 플레이할 스테이지 정보 전달
    public void SetNowStageInfo(int nowplanet, int nowstage)
    {
        NowStage = nowstage;
        NowPlanet = nowplanet;
    }
    public int[] GetNowStageInfo_PLN_ST()
    {
        int[] info = {NowPlanet,NowStage};

        return info;
    }

    /*(인게임)바알 획득 에너지 정보 저장 - GameClear_UpdateData() 호출 전 반드시 저장해주어야함*/
    public void SetBaalEnergy(int energy)
    {
        tmpEnergy = energy;
        EPoint_Calculate_Data(tmpEnergy);
        Debug.Log("게임 클리어 시 전달 받은 에너지: " + tmpEnergy);
    }

    /*(인게임)도전 과제 클리어 정보 저장 - GameClear_UpdateData() 호출 전 반드시 저장해주어야함*/
    public void SetStageChallenge(bool[] challenge)
    {
        StageChallenge = challenge;
    }
    public bool[] GetStageChallenge()
    {
        return StageChallenge;
    }

    /*(인게임)게임 플레이 이후*/
    public void GameClear_UpdateData()//인게임에서 게임을 클리어시 호출
    {
        GameClear_UpdateCore();
        GameClear_UpdateStageInfo();
    }
    //코어 정보 갱신
    public void GameClear_UpdateCore()
    {
        BaalCore.instance.UpdateCoreData_Each(NowPlanet, NowStage, GetStageChallenge());
    }
    //스테이지 Star,Clear 정보를 갱신
    public void GameClear_UpdateStageInfo()
    {
        PlayerData.instance.UpdateStageData(NowPlanet, NowStage);
    }

    /*(아웃게임)게임 플레이 이후*/
    public void GameClear_EnergyCalculate()
    {
        OutGameUIManager.instance.ShowEnergyCalculatePanel();
        Debug.Log("현재 모은 에너지: " +tmpEnergy);
        EnergyCalculate.instance.StartEnergyCalculation(tmpEnergy);
    }

    /*에너지, 포인트 변환 - 데이터상*/
    #region Energy & Point Calculation
    public void EPoint_Calculate_Data(int addedvalue)
    {
        //현재 에너지 합계
        int sum = addedvalue;
        sum += BaalStatus.instance.GetBaalEnergy();
        //환산
        int point = EnergyToPoint(sum);
        int energy = GetEnergyLeft(sum, point);

        //결과값 저장
        SaveCalculation(point, energy);
    }
    private int EnergyToPoint(int sum)
    {
        int point = (sum / BaalStatus.instance.GetBaalEnergyLimit());
        return point;
    }
    private int GetEnergyLeft(int sum, int point)
    {
        int energy = sum - (point * BaalStatus.instance.GetBaalEnergyLimit());
        return energy;
    }
    private void SaveCalculation(int point, int energy)
    {
        BaalStatus.instance.SetReinforcePoint(point);
        BaalStatus.instance.SetBaalEnergy(energy);
    }
    #endregion Calculation
}
