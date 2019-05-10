using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public abstract class Nail : MonoBehaviour
{
    public Transform nailHead;

    [SerializeField] GameObject CrashedNail;
    [SerializeField] GameObject CrackStrength1;
    [SerializeField] GameObject CrackStrength2;
    [SerializeField] protected float angle=10f;
    [SerializeField] protected float allowedAngle = 5f;
    [SerializeField] protected float rotationSpeed=10f;
    [SerializeField] protected int defaultStrengthForCorrectHit = 3;
    
    public int ScoreForNail => scoreForNail;
    public Vector3 DefaultPosition => defaultPosition;

    public int DefaultStrengthForCorrectHit => defaultStrengthForCorrectHit;
    
    [HideInInspector]
    public int hitsPerCurrentNail = 0;
    [HideInInspector]
    public float Xoffset = 2f;
    [HideInInspector]
    public int strengthForCorrectHit = 3;
    [HideInInspector]
    public bool isMoving;

    [SerializeField]
    protected float step = 0.5f;

    protected int minHammerHits;
    protected float Yoffset;
    
    public bool isOverhit { get; set; }

    protected Hammer hammer;
    protected float depthAfterHit;
    protected bool isMovingRight;

    protected int scoreForNail;
    protected Vector3 defaultPosition;
    protected int cashedMaxHammerStrength;
    protected float rotationZ=0f;
    protected AudioSource nailAudioSource;

    protected abstract void SetRandomHeight();
    protected virtual void Awake()
    {
        cashedMaxHammerStrength = ConstantDataContainer.MaxHammerStrength;
        hammer = FindObjectOfType<Hammer>();
        SetRandomHeight();
        StartCoroutine(CalculateMinHammerHits());
        nailAudioSource = GetComponent<AudioSource>();
    }
    protected void Update()
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
    protected void OnTriggerEnter(Collider collision)
    {
        HandleMovingNail();
        if(isOverhit)
        {return;}
        if(hammer.GetStrength() > strengthForCorrectHit)
        {
            depthAfterHit = transform.position.y - (strengthForCorrectHit * step + (hammer.GetStrength() - strengthForCorrectHit) * (step / 2f));
            isOverhit = true;

            DisplayCrack();
        }
        else
        {
            if (hammer.GetStrength() == strengthForCorrectHit)
            {
                isOverhit = true;
                hitsPerCurrentNail++;
                
                CalculatePoints();
                EventManager.RaiseEventEarnScore(scoreForNail);
                EventManager.RaiseEventNailPocket();
            }
            depthAfterHit = transform.position.y - (hammer.GetStrength() * step);
            strengthForCorrectHit -= hammer.GetStrength();
        }
        hitsPerCurrentNail++;
        transform.DOMove(new Vector3(transform.position.x, depthAfterHit, transform.position.z), 0.06f);
    }

    private void DisplayCrack()
    {
        var overHitValue = hammer.GetStrength() - strengthForCorrectHit;
        switch (overHitValue)
        {
            case 1:
                CrackStrength1.SetActive(true);
                break;
            case 2:
                CrackStrength2.SetActive(true);
                break;
            default:
                break;
        }
    }

    protected void HandleMovingNail()
    {
        if (isMoving && GetCurrentAngle() > allowedAngle && hammer.GetStrength() > 0)
        {
            nailAudioSource.Play();
            GetComponentInChildren<MeshRenderer>().enabled = false;
            CrashedNail.SetActive(true);
            isOverhit = true;
            isMoving = false;
            scoreForNail = 0;
        }
        else if (isMoving && GetCurrentAngle() <= allowedAngle)
        {
            isMoving = false;
            transform.DORotate(Vector3.zero, 0.2f);
        }
    }
    protected IEnumerator CalculateMinHammerHits()
    {
        yield return new WaitForEndOfFrame();
 
        if(isMoving)
        {
            minHammerHits += (int)Mathf.Ceil(defaultStrengthForCorrectHit / (float)cashedMaxHammerStrength);
        }
        else
        {
            minHammerHits += (int)Mathf.Ceil(strengthForCorrectHit / (float)cashedMaxHammerStrength);
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
