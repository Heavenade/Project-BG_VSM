using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*플레이어가 설정한 환경설정, 등의 데이터 - 플레이어 데이터를 전부 이 스크립트로 옮겨야함. (바알 등)*/
public class PlayerData : MonoBehaviour
{
    /*싱글톤*/
    public static PlayerData instance;
    #region Singleton
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.transform.root.gameObject);
            InitStageData();
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion Singleton

    private List<int[]> StageDataList;

    private void InitStageData()
    {
        StageDataList = new List<int[]>();

        //4개의 스테이지 셋 - 5개의 스테이지 별의 갯수
        //별의 갯수가 1 이상이면 클리어. 0이면 미클. -1이면 도전 불가

        //Init - Debug
        for (int i = 0; i < 3; i++)
        {
            int[] tmp = new int[] {3,2,1,0,-1};
            StageDataList.Add(tmp);
        }
    }
    public List<int[]> GetStageData()
    {
        InitStageData();
        return StageDataList;
    }
    public void UpdateStageData(int selectplanet, int selectstage)
    {
        StageDataList[selectplanet][selectstage] = 1;
        if(selectstage < 4)
            StageDataList[selectplanet][selectstage + 1] = 0;
    }

    public float[] GetBaalStatusData()//스테이터스의 계산된 실제값 반환(레벨x, 레벨로 계산한 값)
    { 
        //공격력, 체력, 회복량, 이동속도
        float[] realvalue = {BaalStatus.instance.GetATKValue(), BaalStatus.instance.GetHPValue(),
            BaalStatus.instance.GetGLUTValue(), BaalStatus.instance.GetSPEEDValue()};
        return realvalue;
    }
}
