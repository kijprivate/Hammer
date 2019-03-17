using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedNail : Nail
{
    protected override void Awake()
    {
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
                scoreForNail = scoreForNail;
                break;
            case 2:
                Yoffset = -0.5f;
                strengthForPerfectHit = 8;
                scoreForNail = scoreForNail;
                break;
            case 3:
                Yoffset = -1f;
                strengthForPerfectHit = 6;
                scoreForNail = scoreForNail / 2;
                break;
            case 4:
                Yoffset = -1.5f;
                strengthForPerfectHit = 4;
                scoreForNail = scoreForNail / 2;
                break;
            //case 5:
            //    Yoffset = -2f;
            //    strengthForPerfectHit = 2;
            //    scoreForNail = scoreForNail / 4;
            //    break;
            default:
                break;
        }
        transform.position += new Vector3(0f, Yoffset, 0f);
    }
}
