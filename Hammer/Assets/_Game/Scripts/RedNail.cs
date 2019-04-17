using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RedNail : Nail
{
    public bool isMovingRed;
    public Vector3 DefaultPositionRed => defaultPositionRed;

    private Vector3 defaultPositionRed;
    private bool isMovingRight;
    protected override void Awake()
    {
        scoreForNail = ConstantDataContainer.ScoreForRedNail;
        cashedMaxHammerStrength = ConstantDataContainer.MaxHammerStrength;
        defaultStrengthForCorrectHit = 8;
        hammer = FindObjectOfType<Hammer>();
        SetRandomHeight();
        CalculateMinHammerHits();
    }

    protected override void Update()
    {
        if (isMovingRed)
        {
            if (isMovingRight)
            {
                rotationZ += rotationSpeed * Time.deltaTime;
                transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
                if (rotationZ >= angle)
                {
                    isMovingRight = false;
                }
            }
            else
            {
                rotationZ -= rotationSpeed * Time.deltaTime;
                transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
                if (rotationZ <= -angle)
                {
                    isMovingRight = true;
                }
            }
        }
    }
    
    protected override void OnTriggerEnter(Collider collision)
    {
        HandleMovingNail();
        if(isOverhit)
        {return;}
        if(hammer.GetStrength() > strengthForCorrectHit)
        {
            depthAfterHit = transform.position.y - (strengthForCorrectHit * step + (hammer.GetStrength()-strengthForCorrectHit)*(step/3f) );
            isOverhit = true;
        }
        else
        {
            if (hammer.GetStrength() == strengthForCorrectHit)
            {
                isOverhit = true;
                hitsPerCurrentNail++;
                
                CalculatePoints();
                EventManager.RaiseEventNailPocket();
            }
            depthAfterHit = transform.position.y - (hammer.GetStrength() * step);
            strengthForCorrectHit -= hammer.GetStrength();
        }
        hitsPerCurrentNail++;
        transform.DOMove(new Vector3(transform.position.x, depthAfterHit, transform.position.z), 0.06f);
    }
    
    protected override void HandleMovingNail()
    {
        if (isMovingRed && GetCurrentAngle() > allowedAngle && hammer.GetStrength() > 0)
        {
            // TODO add animation
            isOverhit = true;
            isMovingRed = false;
            scoreForNail = 0;
            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOMoveY(-5, 1f))
                .Join(transform.DOScale(Vector3.zero, 1f));
        }
        else if (isMovingRed && GetCurrentAngle() <= allowedAngle)
        {
            isMovingRed = false;
            transform.DORotate(Vector3.zero, 0.2f);
        }
    }
    protected override void SetRandomHeight()
    {
        defaultPositionRed = transform.position;
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
    
    protected override float GetCurrentAngle()
    {
        if (transform.rotation.eulerAngles.z > angle)
        {
            return 360f - transform.rotation.eulerAngles.z;
        }
        else
        {
            return transform.rotation.eulerAngles.z;
        }
    }
}
