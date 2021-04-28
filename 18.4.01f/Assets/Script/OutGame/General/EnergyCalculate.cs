using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyCalculate : MonoBehaviour
{
    /*싱글톤*/
    public static EnergyCalculate instance;
    #region Singleton
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion Singleton

    public GameObject EnergyCalculatePanel;

    public Slider EnergyBar;
    public Text EnergyUIText;
    public Text PointUIText;
    private GameObject Fill;

    [SerializeField]
    private int AddedValue_visual;
    private int ReinforcePoint_visual;

    private int AddedValue_real;
    private int ReinforcePoint_real;

    Color emptycolor = new Color32(0, 0, 0, 255);
    Color fillcolor = new Color32(245, 180, 67, 255);


    bool isfillcollision = false;

    private void Start()
    {      
    }

    //startCalculation
    public void StartEnergyCalculation(int tmpEnergy)
    {
        EPoint_Calculate_NData(tmpEnergy);
        SetBarUI();
        StartBarUpdate();//비주얼 바 업데이트
    }

    /*Calculation*/
    #region Energy & Point Calculation _ Not Data
    private void EPoint_Calculate_NData(int addedvalue)
    {
        AddedValue_real = addedvalue;
        //현재 에너지 합계
        int sum = addedvalue;
        sum += BaalStatus.instance.GetBaalEnergy();

        //환산
        int point = EnergyToPoint_NData(sum);
    }
    private int EnergyToPoint_NData(int sum)
    {
        int point = 0;

        point = (sum / BaalStatus.instance.GetBaalEnergyLimit());

        ReinforcePoint_real = point;

        return point;
    }
    #endregion Energy & Point Calculation _ Not Data

    /*Visual Bar Update*/
    #region Visual Bar Update
    private void SetBarUI()
    {
        ReinforcePoint_visual = 0;
        AddedValue_visual = AddedValue_real;

        EnergyBar.minValue = 0;
        EnergyBar.value = BaalStatus.instance.GetBaalEnergy();
        EnergyBar.maxValue = BaalStatus.instance.GetBaalEnergyLimit();

        EnergyUIText.text = AddedValue_visual.ToString();
        PointUIText.text = ReinforcePoint_visual.ToString();

        PointUIText.color = emptycolor;

        GetFillUI();
        SetFillCollider();
    }
    private void StartBarUpdate()
    {
        StartCoroutine(EnergyBarUpdate());
    }
    IEnumerator EnergyBarUpdate()
    {
        int tmpValue = AddedValue_visual;

        yield return new WaitForSeconds(0.5f);

        WaitForSeconds barupdate = new WaitForSeconds(0.005f);
        WaitForSeconds barfulldelay = new WaitForSeconds(0.5f);

        while (tmpValue > 0)
        {
            EnergyBar.value++;
            tmpValue--;
            EnergyUIText.text = tmpValue.ToString();

            SetFillCollider();

            //텍스트 컬러체인지
            VisualPointColorChange();

            //게이지 바 초기화
            if (EnergyBar.value >= EnergyBar.maxValue)
            {
                VisualPointUpdate();
                yield return barfulldelay;
                EnergyBar.value = EnergyBar.minValue;
            }
            yield return barupdate;
        }
        AddedValue_visual = 0;
    }
    private void VisualPointUpdate()
    {
        ReinforcePoint_visual++;
        PointUIText.text = ReinforcePoint_visual.ToString();
    }

    public void SetFillIsCollision()
    {
        if (isfillcollision)
            isfillcollision = false;
        else
            isfillcollision = true;
    }
    private void VisualPointColorChange()
    {
        if (isfillcollision)
            PointUIText.color = fillcolor;
        else
            PointUIText.color = emptycolor;
    }
    #endregion Visual Bar Update

    /*UI*/
    private void GetFillUI()
    {
        Fill = EnergyBar.transform.Find("Fill Area").gameObject.transform.Find("Fill").gameObject;
    }
    private void SetFillCollider()
    {
        Rect fillrect = Fill.transform.GetComponent<RectTransform>().rect;
        Fill.transform.GetComponent<BoxCollider2D>().size = new Vector2(fillrect.width, fillrect.height);
    }

    //EndCalculation
    public void EndEnergyCalculation()
    {
        StartCoroutine(ShowEnergyInfo());
    }
    IEnumerator ShowEnergyInfo()
    {
        OutGameUIManager.instance.ApplyInfo_EC(ReinforcePoint_real);
        yield return new WaitForSeconds(OutGameUIManager.UI_DELAY);

        OutGameUIManager.instance.ShowInfo_EC();

        OutGameUIManager.instance.HideEnergyCalculatePanel();
    }
}
