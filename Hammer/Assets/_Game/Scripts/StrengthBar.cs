using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrengthBar : MonoBehaviour
{
    private Image strengthBar;
    private float fillInterval;
    private Hammer hammer;
    
    void Start()
    {
        hammer = FindObjectOfType<Hammer>();
        strengthBar = GetComponent<Image>();
        fillInterval = 1f / LevelContainer.NumberOfNails;
    }

    private void Update()
    {
        strengthBar.fillAmount = Remap(hammer.transform.rotation.z, 0f, 0.6f, 0f, 1f);
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

}
