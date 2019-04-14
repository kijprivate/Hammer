using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private Image progressBar;
    private float fillInterval;
    private Hammer hammer;
    void Start()
    {
        hammer = FindObjectOfType<Hammer>();
        progressBar = GetComponent<Image>();
        fillInterval = 1f / LevelContainer.NumberOfNails;
        EventManager.EventNailFinished += OnNailFinished;
    }

    private float Remap (float from, float fromMin, float fromMax, float toMin,  float toMax)
    {
        var fromAbs  =  from - fromMin;
        var fromMaxAbs = fromMax - fromMin;      
           
        var normal = fromAbs / fromMaxAbs;
     
        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;
     
        var to = toAbs + toMin;
           
        return to;
    }
    
    void OnNailFinished()
    {
        progressBar.fillAmount = Remap((float) LevelContainer.Score, 0f, LevelContainer.MaxAvailableScore, 0f, 1f);
    }

    private void OnDestroy()
    {
        EventManager.EventNailFinished -= OnNailFinished;
    }
}
