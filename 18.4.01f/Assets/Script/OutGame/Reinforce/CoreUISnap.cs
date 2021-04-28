using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class CoreUISnap : MonoBehaviour
{

    [SerializeField] public HorizontalScrollSnap scrollsnap;

    private void Awake()
    {  
        scrollsnap.OnSelectionPageChangedEvent.AddListener(OnPageChanged);
        scrollsnap.OnSelectionChangeEndEvent.AddListener(OnPageChangeEnd);
    }
    void OnPageChanged(int index)
    {
        CoreReinforce.instance.ShowCoreFrame(index);
        //Debug.Log("CHanged TO :" + index);
    }
    void OnPageChangeEnd(int index)
    {
    }
}
