﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultNail : Nail
{
    protected override void Awake()
    {
        base.Awake();
        scoreForNail = ConstantDataContainer.DefaultNail;
        StartCoroutine(AdditionalPointsForMoving());
    }

    IEnumerator AdditionalPointsForMoving()
    {
        yield return new WaitForEndOfFrame();
        if (isMoving)
        {
            scoreForNail = ConstantDataContainer.MovingDefaultNail;
            strengthForCorrectHit = defaultStrengthForCorrectHit;
        }
        else
        {
            scoreForNail = ConstantDataContainer.DefaultNail;
        }
    }

    protected override void SetRandomHeight()
    {
        defaultPosition = transform.position;
        int random = Random.Range(1, 4);
        switch (random)
        {
            case 1:
                Yoffset = 0f;
                strengthForCorrectHit = 3;
                break;
            case 2:
                Yoffset = -0.5f;
                strengthForCorrectHit = 2;
                break;
            case 3:
                Yoffset = -1f;
                strengthForCorrectHit = 1;
                break;
            default:
                break;
        }
        transform.position += new Vector3(0f, Yoffset, 0f);
    }
}
