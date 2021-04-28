using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillCollision2D : MonoBehaviour
{
    [SerializeField]
    GameObject PointText;
    Collider2D pointCol;

    private void Awake()
    {
        pointCol = PointText.transform.GetComponent<Collider2D>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other == pointCol)
        {
            EnergyCalculate.instance.SetFillIsCollision();
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other == pointCol)
        {
            EnergyCalculate.instance.SetFillIsCollision();
        }
    }
}
