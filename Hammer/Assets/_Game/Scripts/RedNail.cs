using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedNail : Nail
{
    protected override void Awake()
    {
        scoreForNail = ConstantDataContainer.ScoreForRedNail;
        hammer = FindObjectOfType<Hammer>();
        SetRandomHeight();
    }

    protected override void SetRandomHeight()
    {
        int random = Random.Range(1, 5);
        switch (random)
        {
            case 1:
                Yoffset = 0f;
                strengthForPerfectHit = 10;
                break;
            case 2:
                Yoffset = -0.5f;
                strengthForPerfectHit = 8;
                break;
            case 3:
                Yoffset = -1f;
                strengthForPerfectHit = 6;
                break;
            case 4:
                Yoffset = -1.5f;
                strengthForPerfectHit = 4;
                break;
            default:
                break;
        }
        transform.position += new Vector3(0f, Yoffset, 0f);
    }
}
