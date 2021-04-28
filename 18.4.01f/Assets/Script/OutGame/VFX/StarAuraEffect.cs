using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarAuraEffect : MonoBehaviour
{
    public static StarAuraEffect instance;
    #region Singleton
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion Singleton

    public GameObject StarAura_1;
    public GameObject StarAura_2;
    public GameObject StarAura_3;
    public GameObject StarAura_1_EN;
    public GameObject StarAura_2_EN;
    public GameObject StarAura_3_EN;
    public GameObject StarAura_Baal;

    public void StarAuraON(int num)
    {
        switch (num)
        {
            case 1:        
                StarAura_1.SetActive(true);
                break;
            case 2:
            
                StarAura_2.SetActive(true);
                break;
            case 3:            
                StarAura_3.SetActive(true);
                break;
            case 4:
                StarAura_1_EN.SetActive(true);
                break;
            case 5:
                StarAura_2_EN.SetActive(true);
                break;
            case 6:
                StarAura_3_EN.SetActive(true);
                break;
            case 0:
                StarAura_Baal.SetActive(true);
                break;
            default:
                break;
        }
    }
    public void StarAuraOFF()
    {
        StarAura_1.SetActive(false);
        StarAura_2.SetActive(false);
        StarAura_3.SetActive(false);
        StarAura_1_EN.SetActive(false);
        StarAura_2_EN.SetActive(false);
        StarAura_3_EN.SetActive(false);
        StarAura_Baal.SetActive(false);
    }


}
