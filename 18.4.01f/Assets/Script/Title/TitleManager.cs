using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    /*싱글톤*/
    public static TitleManager instance;
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

    [SerializeField]
    private Text TouchtoStart;

    [SerializeField]
    private GameObject LoadingCircle;

    [SerializeField]
    private ParticleSystem starfield_mili;
    [SerializeField]
    private ParticleSystem starfield_soft;
    [SerializeField]
    private ParticleSystem starfield_hard;

    bool isFlickering;
    bool isRotating;
    bool isStarPlaying;

    private void Start()
    {
        isStarPlaying = true;
        LoadingCircle.SetActive(false);
        StartCoroutine(TextFlicker());
    }

    private void Update()
    {
        PlayStarField();
    }

    public void GameStart()
    {
        HideTextFlicker();
        StartCoroutine(StartLoadingCycle());
        StartCoroutine(MoveToOutGameCoroutine());
        StartCoroutine(StopStarField());
    }

    IEnumerator MoveToOutGameCoroutine()
    {
        yield return SceneManager.LoadSceneAsync("Outgame");
    }

    IEnumerator TextFlicker()
    {
        float time = 0f;
        float value = 2.0f;

        Color originColor = TouchtoStart.GetComponent<Text>().color;
        isFlickering = true;

        while (isFlickering)
        {
            float flicker = Mathf.Abs(Mathf.Sin(time * value));
            TouchtoStart.GetComponent<Text>().color = originColor * flicker;

            time += Time.deltaTime;

            yield return null;
        }
        TouchtoStart.GetComponent<Text>().color = originColor;

        yield return null;
    }

    public void HideTextFlicker()
    {
        isFlickering = false;
        TouchtoStart.gameObject.SetActive(false);
    }

    IEnumerator StartLoadingCycle()
    {
        LoadingCircle.SetActive(true);
        float value = 5.0f;

        isRotating = true;

        Vector3 rotateangle = new Vector3(0f,0f,0f);
        WaitForSeconds delay = new WaitForSeconds(0.0001f);

        while (isRotating)
        {
            rotateangle.z += value;
            LoadingCircle.transform.rotation = Quaternion.Euler(rotateangle);
            yield return delay;
        }
        yield return null;
    }


    //부하가 클 경우 다른 방법 모색
    void PlayStarField()
    {
        if (isStarPlaying == false)
            return;
        if (starfield_mili.isPlaying == false)
            starfield_mili.Play();
        if (starfield_soft.isPlaying == false)
            starfield_soft.Play();
        if (starfield_hard.isPlaying == false)
            starfield_hard.Play();
    }
    IEnumerator StopStarField()
    {
        isStarPlaying = false;

        starfield_mili.Stop();
        starfield_soft.Stop();
        starfield_hard.Stop();

        yield return null;
    }
}
