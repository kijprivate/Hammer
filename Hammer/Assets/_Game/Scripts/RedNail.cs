using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedNail : Nail
{
    protected override void Awake()
    {
        base.Awake();
        scoreForNail = ConstantDataContainer.ScoreForRedNail;
        StartCoroutine(AdditionalPointsForMoving());
    }

    IEnumerator AdditionalPointsForMoving()
    {
        yield return new WaitForEndOfFrame();
        if (isMoving)
        {
            scoreForNail = (int)(ConstantDataContainer.ScoreForRedNail * 1.5f);
        }
        else
        {
            scoreForNail = ConstantDataContainer.ScoreForRedNail;
        }
    }

    protected override void SetRandomHeight()
    {
        defaultPosition = transform.position;
        int random = Random.Range(1, 5);
        switch (random)
        {
            case 1:
                Yoffset = 0f;
                strengthForCorrectHit = 8;
                break;
            case 2:
                Yoffset = -0.5f;
                strengthForCorrectHit = 6;
                break;
            case 3:
                Yoffset = -1f;
                strengthForCorrectHit = 4;
                break;
            case 4:
                Yoffset = -1.5f;
                strengthForCorrectHit = 2;
                break;
            default:
                break;
        }
        transform.position += new Vector3(0f, Yoffset, 0f);
    }
}
