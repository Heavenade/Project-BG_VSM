using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarfieldEffect : MonoBehaviour
{
    public static StarfieldEffect instance;
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
    GameObject StarField;

    public ParticleSystem StarParticleSys_Mili;
    public ParticleSystem StarParticleSys_Soft;
    public ParticleSystem StarParticleSys_Hard;

    private static Vector3 ZOOM_OUT_SCALE = new Vector3(1f, 1f, 1f);

    private static Vector3 ZOOM_IN_SCALE_1 = new Vector3(0.55f, 0.55f, 1f);
    private static Vector3 ZOOM_IN_SCALE_2 = new Vector3(0.8f, 0.8f, 1f);
    private static Vector3 ZOOM_IN_SCALE_3 = new Vector3(0.65f, 0.65f, 1f);
    private static Vector3 ZOOM_IN_SCALE_BAAL = new Vector3(0.9f, 0.9f, 1f);

    private ParticleSystem.Particle[] points;  

    void Start()
    {
        StarParticleSys_Mili.Play();
        StarParticleSys_Soft.Play();
        StarParticleSys_Hard.Play();
    }

    public void StarFieldZoomIn_Planet(int num)
    {
        if (num == 1 || num == 4)
        {
            StarParticleSys_Mili.transform.localScale = ZOOM_IN_SCALE_1;
            StarParticleSys_Soft.transform.localScale = ZOOM_IN_SCALE_1;
            StarParticleSys_Hard.transform.localScale = ZOOM_IN_SCALE_1;
        }
        else if (num == 2 || num == 5)
        {
            StarParticleSys_Mili.transform.localScale = ZOOM_IN_SCALE_2;
            StarParticleSys_Soft.transform.localScale = ZOOM_IN_SCALE_2;
            StarParticleSys_Hard.transform.localScale = ZOOM_IN_SCALE_2;
        }
        else if (num == 3 || num == 6)
        {
            StarParticleSys_Mili.transform.localScale = ZOOM_IN_SCALE_3;
            StarParticleSys_Soft.transform.localScale = ZOOM_IN_SCALE_3;
            StarParticleSys_Hard.transform.localScale = ZOOM_IN_SCALE_3;
        }
    }
    public void StarFieldZoomIn_Baal()
    {
        StarParticleSys_Mili.transform.localScale = ZOOM_IN_SCALE_BAAL;
        StarParticleSys_Soft.transform.localScale = ZOOM_IN_SCALE_BAAL;
        StarParticleSys_Hard.transform.localScale = ZOOM_IN_SCALE_BAAL;
    }
    public void StarFieldZoomOut()
    {
        StarParticleSys_Mili.transform.localScale = ZOOM_OUT_SCALE;
        StarParticleSys_Soft.transform.localScale = ZOOM_OUT_SCALE;
        StarParticleSys_Hard.transform.localScale = ZOOM_OUT_SCALE;
    }

    public void StarfieldRelocate(Vector3 campos)
    {
        StarParticleSys_Mili.transform.position = new Vector3 (campos.x,campos.y,0);
        StarParticleSys_Soft.transform.position = new Vector3(campos.x, campos.y, 0);
        StarParticleSys_Hard.transform.position = new Vector3(campos.x, campos.y, 0);
    }
}
