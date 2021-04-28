using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


/*현재는 활성화된 코어 이미지에만 Info Trigger가 설정되어 있음
 
  리소스 추가 후 비활성화된 코어 이미지 역시 작업해야함.*/

public class CoreReinforce : MonoBehaviour
{

    public static CoreReinforce instance;
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
            Destroy(this.gameObject);
        }


    }
    #endregion Singleton    

    private List<int[]> CoreDataList;

    /*상수*/
    private const int TUTO_COMPLETE_X = 450;
    private const int TUTO_INCOMPLETE_X = 630;

    /*UI*/
    public GameObject CoreContent;//스크롤뷰 Content

    public GameObject CoreFrame_1;
    public GameObject CoreFrame_2;

    public GameObject CoreInfoPanel;
    public GameObject ShardInfoPanel;

    /*그래픽 레이캐스트*/
    private GraphicRaycaster gr;
    private PointerEventData ped;


    private void Start()
    {
        gr = GetComponent<GraphicRaycaster>();
        ped = new PointerEventData(null);

        LoadCoreReinforceData();
        SetCoreInfoEvent();
        ShowCoreContent();
        ShowCoreFrame(0);
    }


    /*Init, Data*/
    private void SetCoreInfoEvent()
    {
        //Set EventFunction
        for (int i = 0; i < BaalCore.instance.GetCoreNum(); i++)
        {
            GameObject curCoreSet = CoreContent.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject;
            for (int j = 0; j < 5; j++)
            {
                GameObject curShard = curCoreSet.transform.Find("Shard_" + (j + 1)).gameObject;
                List<EventTrigger.Entry> curShardTrigger = curShard.GetComponent<EventTrigger>().triggers;

                curShardTrigger.Add(GetShardEventSystem_Down(i, j));
                curShardTrigger.Add(GetShardEventSystem_Up(i, j));
            }
            //Core
            GameObject curCore = curCoreSet.transform.Find("CoreGem").gameObject;

            List<EventTrigger.Entry> curCoreTrigger = curCore.GetComponent<EventTrigger>().triggers;
            curCoreTrigger.Add(GetCoreEventSystem_Down(i));
            curCoreTrigger.Add(GetCoreEventSystem_Up(i));
        }
    }

    public void LoadCoreReinforceData()
    {
        CoreDataList = BaalCore.instance.GetCoreData();
    }
    public void SaveCoreReinforceData()
    {
        BaalCore.instance.SetCoreData(CoreDataList);
    }


    /*Show UI*/
    public void ShowCoreContent()
    {
        for (int i = 0; i < BaalCore.instance.GetCoreNum(); i++)
        {
            GameObject curCoreSet = CoreContent.transform.GetChild(i).transform.GetChild(0).gameObject;

            //샤드
            for (int j = 0; j < 5; j++)
            {
                GameObject curShard = curCoreSet.transform.Find("Shard_" + (j + 1)).gameObject;
                if (CoreDataList[i][j] == 0) //미획득
                {
                    curShard.SetActive(false);
                }
                else if (CoreDataList[i][j] == 1)//샤드 획득
                {
                    curShard.SetActive(true);
                }
            }
            //코어
            GameObject curCore = curCoreSet.transform.Find("CoreGem").gameObject;

            if (CoreDataList[i][5] == 0) //미획득
            {
                curCore.SetActive(false);
            }
            else if (CoreDataList[i][5] == 1) //코어 획득   
            {
                curCore.SetActive(true);
            }
        }
    }
    public void ShowCoreFrame(int index)
    {
        if (CoreDataList[index][5] == 0) //미획득
        {
            CoreFrame_1.transform.GetChild(0).gameObject.SetActive(true);
            CoreFrame_2.transform.GetChild(0).gameObject.SetActive(true);

            CoreFrame_1.transform.GetChild(1).gameObject.SetActive(false);
            CoreFrame_2.transform.GetChild(1).gameObject.SetActive(false);
        }
        else if (CoreDataList[index][5] == 1) //코어 획득   
        {
            CoreFrame_1.transform.GetChild(1).gameObject.SetActive(true);
            CoreFrame_2.transform.GetChild(1).gameObject.SetActive(true);

            CoreFrame_1.transform.GetChild(0).gameObject.SetActive(false);
            CoreFrame_2.transform.GetChild(0).gameObject.SetActive(false);
        }

        //튜토리얼 클리어 여부에 따라 X 좌표 바꾸기 - 추후 추가
    }

    /*Event Function for EventSystem*/
    //샤드
    private EventTrigger.Entry GetShardEventSystem_Down(int CoreSetNum, int ShardNum)
    {
        GameObject coreset = CoreContent.transform.GetChild(CoreSetNum).gameObject.transform.GetChild(0).gameObject;
        GameObject shard = coreset.transform.Find("Shard_" + (ShardNum + 1)).gameObject;

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((eventData) => { ShowShardinfo(CoreSetNum, ShardNum, shard); });

        return entry;
    }
    private EventTrigger.Entry GetShardEventSystem_Up(int CoreSetNum, int ShardNum)
    {
        GameObject coreset = CoreContent.transform.GetChild(CoreSetNum).gameObject.transform.GetChild(0).gameObject;
        GameObject shard = coreset.transform.Find("Shard_" + (ShardNum + 1)).gameObject;

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((eventData) => { HideShardinfo(CoreSetNum, ShardNum, shard); });

        return entry;
    }

    //코어
    private EventTrigger.Entry GetCoreEventSystem_Down(int CoreSetNum)
    {
        GameObject coreset = CoreContent.transform.GetChild(CoreSetNum).gameObject.transform.GetChild(0).gameObject;
        GameObject core = coreset.transform.Find("CoreGem").gameObject;

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((eventData) => { ShowCoreinfo(CoreSetNum, core); });

        return entry;
    }
    private EventTrigger.Entry GetCoreEventSystem_Up(int CoreSetNum)
    {
        GameObject coreset = CoreContent.transform.GetChild(CoreSetNum).gameObject.transform.GetChild(0).gameObject;
        GameObject core = coreset.transform.Find("CoreGem").gameObject;

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((eventData) => { HideCoreinfo(CoreSetNum, core); });

        return entry;
    }


    /*Information*/
    //샤드
    private void ShowShardinfo(int CoreSetNum, int ShardNum, GameObject shard)
    {
        ShardInfoPanel.SetActive(true);
    }
    private void HideShardinfo(int CoreSetNum, int ShardNum, GameObject shard)
    {
        ShardInfoPanel.SetActive(false);
    }

    //코어
    private void ShowCoreinfo(int CoreSetNum, GameObject core)
    {
        CoreInfoPanel.SetActive(true);
    }
    private void HideCoreinfo(int CoreSetNum, GameObject core)
    {
        CoreInfoPanel.SetActive(false);
    }
}
