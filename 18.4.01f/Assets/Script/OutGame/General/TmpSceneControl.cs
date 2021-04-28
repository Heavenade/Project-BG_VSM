using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TmpSceneControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void MoveToOutGame()
    {
        SceneManager.LoadScene("Outgame", LoadSceneMode.Single);
    }

    public void MoveToInGame()
    {
        SceneManager.LoadScene("tutorial_stage01", LoadSceneMode.Single);
    }
}
