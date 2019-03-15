using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class Nail : MonoBehaviour
{
    public Transform nailHead;
    public int scoreForNail = 300;
    [HideInInspector]
    public int hitsPerCurrentNail = 0;

    [SerializeField]
    protected float step = 0.5f;

    protected int strengthForPerfectHit = 3;
    protected float Yoffset;

    public bool isOverhit { get; set; }

    protected Hammer hammer;
    protected float depthAfterHit;

    protected virtual void Awake()
    {
        hammer = FindObjectOfType<Hammer>();
        SetRandomHeight();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(hammer.GetStrength() > strengthForPerfectHit)
        {
            depthAfterHit = transform.position.y - (strengthForPerfectHit * step + (hammer.GetStrength()-strengthForPerfectHit)*(step/2f) );
            isOverhit = true;
        }
        else
        {
            if (hammer.GetStrength() == strengthForPerfectHit)
            {
                isOverhit = true;
                hitsPerCurrentNail++;
                LevelContainer.Score += scoreForNail / hitsPerCurrentNail;
                EventManager.RaiseEventNailPocket();
            }
            depthAfterHit = transform.position.y - (hammer.GetStrength() * step);
            strengthForPerfectHit -= hammer.GetStrength();
        }
        hitsPerCurrentNail++;
        //transform.position = new Vector3(transform.position.x, depthAfterHit, transform.position.z);
        transform.DOMove(new Vector3(transform.position.x, depthAfterHit, transform.position.z), 0.06f);
    }

    protected virtual void SetRandomHeight()
    {
        int random = Random.Range(1, 4);
        switch (random)
        {
            case 1:
                Yoffset = 0f;
                strengthForPerfectHit = 3;
                break;
            case 2:
                Yoffset = -0.5f;
                strengthForPerfectHit = 2;
                scoreForNail = scoreForNail/2;
                break;
            case 3:
                Yoffset = -1f;
                strengthForPerfectHit = 1;
                scoreForNail = scoreForNail / 4;
                break;
            default:
                break;
        }
        transform.position += new Vector3(0f, Yoffset, 0f);
    }

    public float GetStep()
    {
        return step;
    }

    public int GetStrengthForPerfectHit()
    {
        return strengthForPerfectHit;
    }
}
