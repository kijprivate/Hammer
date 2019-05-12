using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] Image star1, star2, star3;

    float percentageValueOfScore;

    float finished1Star;
    float current1Star;

    float finished2Star;
    float current2Star;

    float finished3Star;
    float current3Star;

    void Start()
    {
        star1.fillAmount = 0f;
        star2.fillAmount = 0f;
        star3.fillAmount = 0f;

        EventManager.EventNailPocket += OnNailFinished;
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
        percentageValueOfScore = (float)LevelContainer.Score / LevelContainer.MaxAvailableScore;

        if (percentageValueOfScore >= ConstantDataContainer.PercentageValueFor2Stars / 100f)
        {
            finished1Star = Remap(percentageValueOfScore, 0f,
    ConstantDataContainer.PercentageValueFor1Star / 100f, 0f, 1f);
            current1Star = star1.fillAmount;
            DOTween.To(() => current1Star, x => current1Star = x, finished1Star, 0.3f).OnUpdate(UpdateFirst).OnComplete(UpdateFirst);

            finished2Star = Remap(percentageValueOfScore - ConstantDataContainer.PercentageValueFor1Star / 100f, 0f,
    ConstantDataContainer.PercentageValueFor2Stars / 100f - ConstantDataContainer.PercentageValueFor1Star / 100f, 0f, 1f);
            current2Star = star2.fillAmount;
            DOTween.To(() => current2Star, x => current2Star = x, finished2Star, 0.3f).OnUpdate(UpdateSecond).OnComplete(UpdateSecond);

            finished3Star = Remap(percentageValueOfScore - ConstantDataContainer.PercentageValueFor2Stars / 100f,0f,
                ConstantDataContainer.PercentageValueFor3Stars / 100f - ConstantDataContainer.PercentageValueFor2Stars / 100f, 0f, 1f);

            current3Star = star3.fillAmount;
            DOTween.To(() => current3Star, x => current3Star = x, finished3Star, 0.3f).OnUpdate(UpdateThird).OnComplete(UpdateThird);
        }
        else if (percentageValueOfScore >= ConstantDataContainer.PercentageValueFor1Star / 100f)
        {
            finished1Star = Remap(percentageValueOfScore, 0f,
    ConstantDataContainer.PercentageValueFor1Star / 100f, 0f, 1f);
            current1Star = star1.fillAmount;
            DOTween.To(() => current1Star, x => current1Star = x, finished1Star, 0.3f).OnUpdate(UpdateFirst).OnComplete(UpdateFirst);

            finished2Star = Remap(percentageValueOfScore - ConstantDataContainer.PercentageValueFor1Star / 100f, 0f,
                ConstantDataContainer.PercentageValueFor2Stars / 100f - ConstantDataContainer.PercentageValueFor1Star / 100f, 0f, 1f);
            current2Star = star2.fillAmount;
            DOTween.To(() => current2Star, x => current2Star = x, finished2Star, 0.3f).OnUpdate(UpdateSecond).OnComplete(UpdateSecond);
        }
        else if (percentageValueOfScore < ConstantDataContainer.PercentageValueFor1Star / 100f)
        {
            finished1Star = Remap(percentageValueOfScore, 0f,
                ConstantDataContainer.PercentageValueFor1Star / 100f, 0f, 1f);
            current1Star = star1.fillAmount;
            DOTween.To(() => current1Star, x => current1Star = x, finished1Star, 0.3f).OnUpdate(UpdateFirst).OnComplete(UpdateFirst);
        }
    }

    private void UpdateFirst()
    {
        star1.fillAmount = current1Star;
    }
    private void UpdateSecond()
    {
        star2.fillAmount = current2Star;
    }
    private void UpdateThird()
    {
        star3.fillAmount = current3Star;
    }

    private void OnDestroy()
    {
        EventManager.EventNailPocket -= OnNailFinished;
    }
}
