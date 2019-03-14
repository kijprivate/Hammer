using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nail : MonoBehaviour
{
    public Transform nailHead;
    public int scoreForNail = 300;
    public int hitsPerCurrentNail = 0;
    [SerializeField]
    float difficulty = 1f;
    [SerializeField]
    int strengthForPerfectHit = 3;
    [SerializeField]
    float step = 0.5f;
    [SerializeField] float Yoffset;

    public bool isOverhit { get; set; }

    Hammer hammer;
    float depthAfterHit;

    private void Awake()
    {
        hammer = FindObjectOfType<Hammer>();
        SetRandomHeight();
    }

    private void OnTriggerEnter2D(Collider2D collision)
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
        transform.position = new Vector3(transform.position.x, depthAfterHit, transform.position.z);
    }

    private void SetRandomHeight()
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
