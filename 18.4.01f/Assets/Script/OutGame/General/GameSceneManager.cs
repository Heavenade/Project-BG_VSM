using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager instance;
    #region Singleton
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

    public void MoveToIngame_Stage(int planet, int stage)
    {
        //씬 이동 - 스테이지 별로 분리
        if (planet == 0)
        {
            SceneManager.LoadSceneAsync("Stage0"); 
        }
        else if (planet == 1)
        {
            SceneManager.LoadSceneAsync("Stage1"); 
        }
        else if (planet == 2)
        {
            SceneManager.LoadSceneAsync("Stage2"); 
        }
    }

    public void MoveToMain()
    {
        SceneManager.LoadSceneAsync("Outgame", LoadSceneMode.Single);
    }
    public void MoveToMain_Clear()
    {
        StartCoroutine(ToMain_Calculate());
    }
    IEnumerator ToMain_Calculate()
    {
        yield return SceneManager.LoadSceneAsync("Outgame", LoadSceneMode.Single);
        PlayManager.instance.GameClear_EnergyCalculate();
    }
}
