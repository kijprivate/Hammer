using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedNail : Nail
{
//    public bool isMovingRed;
//    public Vector3 DefaultPositionRed => defaultPositionRed;
//
//    private Vector3 defaultPositionRed;
//    private bool isMovingRightRed;
    protected override void Awake()
    {
        scoreForNail = ConstantDataContainer.ScoreForRedNail;
        cashedMaxHammerStrength = ConstantDataContainer.MaxHammerStrength;
        defaultStrengthForCorrectHit = 8;
        hammer = FindObjectOfType<Hammer>();
        SetRandomHeight();
        CalculateMinHammerHits();
    }

//    protected override void Update()
//    {
//        if (isMovingRed)
//        {
//            if (isMovingRightRed)
//            {
//                rotationZ += rotationSpeed * Time.deltaTime;
//                transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
//                if (rotationZ >= angle)
//                {
//                    isMovingRightRed = false;
//                }
//            }
//            else
//            {
//                rotationZ -= rotationSpeed * Time.deltaTime;
//                transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
//                if (rotationZ <= -angle)
//                {
//                    isMovingRightRed = true;
//                }
//            }
//        }
//    }
    
    protected override void SetRandomHeight()
    {
       // defaultPositionRed = transform.position;
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
