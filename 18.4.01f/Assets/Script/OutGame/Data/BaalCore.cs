using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*바알 코어 관련 데이터*/
/*현재 코드는 데이터 저장을 활용하지 않아, 인 게임 내에서만 반영 - 이후 JSON으로 */

public class BaalCore : MonoBehaviour
{
    public static BaalCore instance;
    #region Singleton + Awake
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.transform.root.gameObject);
            instance = this;

            InitCore();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion 

    private static int CORENUM = 3;

    private List<int[]> CoreDataList;

    private void InitCore()
    {
        CoreDataList = new List<int[]>();

        //TestData
        int[] tmp = new int[] { 1, 1, 1, 1, 1, 1 };//core1
        CoreDataList.Add(tmp);
        tmp = new int[] { 1, 1, 1, 1, 0, 0 };//core2
        CoreDataList.Add(tmp);
        tmp = new int[] { 1, 1, 1, 0, 0, 0 };//core3
        CoreDataList.Add(tmp);

        //RealData

        //for (int i = 0; i < CORENUM; i++)
        //{
        //    int[] tmp = new int[] { 1, 0, 0, 0, 0, 0 };//shard 5 core 1
        //    CoreDataList.Add(tmp);
        //}     
    }

    //Data Get,Set
    public List<int[]> GetCoreData()
    {
        return CoreDataList;
    }
    public void SetCoreData(List<int[]> CoreDataList_)
    {
        CoreDataList = CoreDataList_;
    }

    //For Count
    public int GetCoreNum()
    {
        return CORENUM;
    }

    //Update Core
    public void UpdateCoreData_Each(int Planet, int Stage, bool[] StarNum)
    {
        int corenum = Planet;//수정 필요
        int shardnum = Stage;//수정 필요
        int cnt = 0;

        for (int i = 0; i < StarNum.Length; i++)
        {
            if (StarNum[i] == true)
            {
                cnt++;
            }
        }
        if (cnt >= 3)
        {
            CoreDataList[corenum][shardnum] = 1;
        }
        //해당 업데이트로 코어젬 활성화가 있는지 확인
        UpdateCoreGem();
    }
    public void UpdateCoreGem()
    {
        for (int i = 0; i < CORENUM; i++)
        {
            int cnt = 0;
            for (int j = 0; j < CoreDataList[i].Length - 1; j++)
            {
                if (CoreDataList[i][j] == 1)
                    cnt++;
            }
            if (cnt >= 5)
                CoreDataList[i][5] = 1;
        }
    }
    #region CoreBonusStatus
    public void PickCoreBonus(int core, int shard)
    {

    }
    #endregion CoreBonusStatus
}
