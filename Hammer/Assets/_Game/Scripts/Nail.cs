﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class Nail : MonoBehaviour
{
    public Transform nailHead;

    [SerializeField] protected float angle=10f;
    [SerializeField] protected float allowedAngle = 5f;
    [SerializeField] protected float rotationSpeed=10f;
    public int ScoreForNail => scoreForNail;
    public Vector3 DefaultPosition => defaultPosition;

    public int DefaultStrengthForCorrectHit => defaultStrengthForCorrectHit;
    
    [HideInInspector]
    public int hitsPerCurrentNail = 0;
    [HideInInspector]
    public float Xoffset = 2f;

    [SerializeField]
    protected float step = 0.5f;

    public int strengthForCorrectHit = 3;
    protected int defaultStrengthForCorrectHit = 3;
    protected int minHammerHits;
    protected float Yoffset;

    public bool isOverhit { get; set; }

    protected Hammer hammer;
    protected float depthAfterHit;
    public bool isMoving;
    protected bool isMovingRight;

    protected int scoreForNail;
    private Vector3 defaultPosition;
    protected int cashedMaxHammerStrength;
    protected float rotationZ=0f;

    protected virtual void Awake()
    {
        scoreForNail = ConstantDataContainer.ScoreForDefaultNail;
        cashedMaxHammerStrength = ConstantDataContainer.MaxHammerStrength;
        hammer = FindObjectOfType<Hammer>();
        SetRandomHeight();
        CalculateMinHammerHits();
    }

    protected virtual void Update()
    {
        if (isMoving)
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

    protected virtual void OnTriggerEnter(Collider collision)
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

    protected virtual void HandleMovingNail()
    {
        if (isMoving && GetCurrentAngle() > allowedAngle && hammer.GetStrength() > 0)
        {
            // TODO add animation
            isOverhit = true;
            isMoving = false;
            scoreForNail = 0;
            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOMoveY(-5, 1f))
                .Join(transform.DOScale(Vector3.zero, 1f));
        }
        else if (isMoving && GetCurrentAngle() <= allowedAngle)
        {
            isMoving = false;
            transform.DORotate(Vector3.zero, 0.2f);
        }
    }

    protected virtual void SetRandomHeight()
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

    protected void CalculateMinHammerHits()
    {
        if (strengthForCorrectHit % cashedMaxHammerStrength == 0)
        {
            minHammerHits += strengthForCorrectHit / cashedMaxHammerStrength;
        }
        else
        {
            minHammerHits += strengthForCorrectHit / cashedMaxHammerStrength + 1;
        }
    }

    protected void CalculatePoints()
    {
        if (hitsPerCurrentNail <= minHammerHits)
        {
            LevelContainer.Score += scoreForNail;
            EventManager.RaiseEventPerfectHit();
        }
        else
        {
            LevelContainer.Score += scoreForNail;
        }
    }

    public float GetStep()
    {
        return step;
    }

    protected virtual float GetCurrentAngle()
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
