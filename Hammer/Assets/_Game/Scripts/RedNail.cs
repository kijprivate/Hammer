using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedNail : Nail
{
    protected override void Awake()
    {
        scoreForNail = ConstantDataContainer.ScoreForRedNail;
        cashedMaxHammerStrength = ConstantDataContainer.MaxHammerStrength;
        hammer = FindObjectOfType<Hammer>();
        SetRandomHeight();
        CalculateMinHammerHits();
    }

    protected override void SetRandomHeight()
    {
        int random = Random.Range(1, 5);
        switch (random)
        {
            case 1:
                Yoffset = 0f;
                strengthForCorrectHit = 9;
                break;
            case 2:
                Yoffset = -0.5f;
                strengthForCorrectHit = 7;
                break;
            case 3:
                Yoffset = -1f;
                strengthForCorrectHit = 5;
                break;
            case 4:
                Yoffset = -1.5f;
                strengthForCorrectHit = 3;
                break;
            default:
                break;
        }
        transform.position += new Vector3(0f, Yoffset, 0f);
    }
}
