using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class Nail : MonoBehaviour
{
    public Transform nailHead;

    public int ScoreForNail => scoreForNail;
    
    [HideInInspector]
    public int hitsPerCurrentNail = 0;
    [HideInInspector]
    public float Xoffset = 2f;

    [SerializeField]
    protected float step = 0.5f;

    protected int strengthForCorrectHit = 3;
    protected int minHammerHits;
    protected float Yoffset;

    public bool isOverhit { get; set; }

    protected Hammer hammer;
    protected float depthAfterHit;

    protected int scoreForNail;
    protected int cashedMaxHammerStrength;

    protected virtual void Awake()
    {
        scoreForNail = ConstantDataContainer.ScoreForDefaultNail;
        cashedMaxHammerStrength = ConstantDataContainer.MaxHammerStrength;
        hammer = FindObjectOfType<Hammer>();
        SetRandomHeight();
        CalculateMinHammerHits();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(hammer.GetStrength() > strengthForCorrectHit)
        {
            depthAfterHit = transform.position.y - (strengthForCorrectHit * step + (hammer.GetStrength()-strengthForCorrectHit)*(step/2f) );
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

    protected virtual void SetRandomHeight()
    {
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
            LevelContainer.Score += scoreForNail / hitsPerCurrentNail;
        }
    }

    public float GetStep()
    {
        return step;
    }

    public int GetStrengthForCorrectHit()
    {
        return strengthForCorrectHit;
    }
}
