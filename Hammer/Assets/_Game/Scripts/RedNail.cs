using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedNail : Nail
{
    protected override void Awake()
    {
        base.Awake();
        scoreForNail = ConstantDataContainer.RedNail;
        StartCoroutine(AdditionalPointsForMoving());
    }

    IEnumerator AdditionalPointsForMoving()
    {
        yield return new WaitForEndOfFrame();
        if (isMoving)
        {
            scoreForNail = ConstantDataContainer.MovingRedNail;
            strengthForCorrectHit = defaultStrengthForCorrectHit;
        }
        else
        {
            scoreForNail = ConstantDataContainer.RedNail;
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
