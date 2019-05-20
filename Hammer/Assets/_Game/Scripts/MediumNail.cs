using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumNail : Nail
{
    protected override void Awake()
    {
        base.Awake();
        scoreForNail = ConstantDataContainer.MediumNail;
        StartCoroutine(AdditionalPointsForMoving());
    }

    IEnumerator AdditionalPointsForMoving()
    {
        yield return new WaitForEndOfFrame();
        if (isMoving)
        {
            scoreForNail = ConstantDataContainer.MovingMediumNail;
            strengthForCorrectHit = defaultStrengthForCorrectHit;
        }
        else
        {
            scoreForNail = ConstantDataContainer.MediumNail;
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
                strengthForCorrectHit = 5;
                break;
            case 2:
                Yoffset = -0.38f;
                strengthForCorrectHit = 4;
                break;
            case 3:
                Yoffset = -1.14f;
                strengthForCorrectHit = 2;
                break;
            case 4:
                Yoffset = -1.52f;
                strengthForCorrectHit = 1;
                break;
            default:
                break;
        }
        transform.position += new Vector3(0f, Yoffset, 0f);
    }
}
