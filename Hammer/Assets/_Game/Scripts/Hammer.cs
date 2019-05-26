using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using ControlFreak2;

public class Hammer : MonoBehaviour
{
    public int targetIndex { get; private set; }

    [SerializeField] HammerSprites sprites;
    [SerializeField] GameObject Tutorial;
    [SerializeField] AudioClip[] HammerHits;

    private LevelData data;
    Transform myTransform;
    Nail targetNail;
    GameObject NailsParent;
    Quaternion startingRotation;
    Vector3 startingPosition;

    private float cashedPositionBeforeHit;
    private float cashedPositionAfterHit;
    private float verticalMoveSpeed;
    bool isMovingUp = true;
    bool isHammerReady;
    float rotationZ;
    float movingY;
    float depthAfterHit;
    int strength;
    private int cashedMaxStrength;
    private ParticleSystem sparks;
    private AudioSource hammerAudioSource;

    private void Awake()
    {
        var sprite = Instantiate(sprites.HammerSprite[PlayerPrefsManager.GetChosenHammer()], transform);
        sprite.transform.SetParent(transform);
    }

    void Start()
    {
        myTransform = transform;
        sparks = GetComponentInChildren<ParticleSystem>();
        hammerAudioSource = GetComponent<AudioSource>();
        
        StartCoroutine(SetupWithDelay());
    }

    private IEnumerator SetupWithDelay()
    {
        yield return new WaitForSeconds(0.6f);
        data = LevelsDifficultyContainer.Houses[LevelContainer.CurrentHouseNumber-1].levelsData[LevelContainer.CurrentLevelIndex];
        verticalMoveSpeed = data.rotationSpeed / 100f;
        NailsParent = FindObjectOfType<NailsSpawner>().gameObject;
        targetNail = NailsParent.transform.GetChild(targetIndex).gameObject.GetComponent<Nail>();
        EventManager.EventPerfectHit += OnPerfectHit;
        EventManager.EventHammerSpriteChanged += OnSpriteChanged;


        cashedMaxStrength = ConstantDataContainer.MaxHammerStrength;
        cashedPositionBeforeHit = ConstantDataContainer.PositionOverNailHeadBeforeHit;
        cashedPositionAfterHit = ConstantDataContainer.PositionOverNailHeadAfterHit;
        
        myTransform.DOMove(new Vector3(myTransform.position.x,targetNail.nailHead.transform.position.y + cashedPositionBeforeHit, myTransform.position.z),0.3f) ;
        yield return new WaitForSeconds(0.3f);

        rotationZ = myTransform.rotation.z;
        movingY = myTransform.position.y;
        startingRotation = myTransform.rotation;
        startingPosition = myTransform.position;
        isHammerReady=true;
        targetIndex = 0;
    }
    void Update()
    {
        if (CF2Input.GetButton("Click") && !LevelContainer.MenuHided)
        {
            StartCoroutine(CoroutineHideMenuAndStartGame());
        }

        if (CF2Input.GetButton("Click") && isHammerReady && LevelContainer.GameStarted)
        {
            if(Tutorial.active)
            { Tutorial.SetActive(false); }
            if (isMovingUp)
            {
                rotationZ += data.rotationSpeed * Time.deltaTime;
                movingY += verticalMoveSpeed*Time.deltaTime;
                myTransform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
                myTransform.position = new Vector3(myTransform.position.x, movingY, myTransform.position.z);

                if (myTransform.rotation.eulerAngles.z >= 70f)
                { isMovingUp = false; }
            }
            else if (!isMovingUp)
            {
                rotationZ -= data.rotationSpeed * Time.deltaTime;
                movingY -= verticalMoveSpeed*Time.deltaTime;
                myTransform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
                myTransform.position = new Vector3(myTransform.position.x, movingY, myTransform.position.z);

                if (myTransform.rotation.z <= 0f)
                { isMovingUp = true; }
            }
        }

        if (CF2Input.GetButtonUp("Click") && isHammerReady && LevelContainer.GameStarted)
        {
            SetupStrength();
            ParticleSystem.Burst b = sparks.emission.GetBurst(0);
            b.count = strength*20;
            sparks.emission.SetBurst(0, b);
            sparks.Play();
            PlayHitSound();
            SetupHammerPositionAfterHit();

            isHammerReady = false;

            EventManager.RaiseEventHammerHit();

            if (LevelContainer.GameOver)
            { return; }

            StartCoroutine(BackToPositionRoutine());
        }

    }

    private IEnumerator BackToPositionRoutine()
    {
        yield return new WaitForSeconds(0.7f);

        if (targetNail.isOverhit)
        {
            SetupNextTarget();
            EventManager.RaiseEventNailFinished();
        }
        else
        {
            startingPosition = new Vector3(startingPosition.x, targetNail.nailHead.transform.position.y + cashedPositionBeforeHit, startingPosition.z);
            myTransform.DOMove(startingPosition, 0.2f);
        }

        yield return new WaitForSeconds(0.3f);
        myTransform.rotation = startingRotation;
        startingRotation = myTransform.rotation;
        movingY = myTransform.position.y;
        rotationZ = myTransform.rotation.z;

        isHammerReady = true;
    }
    private void SetupStrength() // Hardcoded numbers based on hammer angle
    {
        float strengthInterval = 70f / (cashedMaxStrength+1);

        for(int i = 0; i < cashedMaxStrength+1; i++)
        {
            if(myTransform.rotation.eulerAngles.z >= i*strengthInterval + Mathf.Epsilon && myTransform.rotation.eulerAngles.z <= (i+1)*strengthInterval + Mathf.Epsilon)
            { strength = i; }
        }
    }

    private void PlayHitSound()
    {
        if (strength < HammerHits.Length)
        {
            hammerAudioSource.clip = HammerHits[strength];
            hammerAudioSource.Play();
        }

    }

    private void SetupHammerPositionAfterHit()
    {
        if (strength > targetNail.strengthForCorrectHit)
        {
            depthAfterHit = targetNail.nailHead.position.y + cashedPositionAfterHit - 
                            (targetNail.strengthForCorrectHit * targetNail.GetStep() + (strength - targetNail.strengthForCorrectHit) * (targetNail.GetStep() / 2f));
        }
        else
        {
            depthAfterHit = targetNail.nailHead.position.y - strength * targetNail.GetStep() + cashedPositionAfterHit;
        }

        myTransform.DORotateQuaternion(Quaternion.Euler(0f, 0f, 0f), 0.06f);
        myTransform.DOMove(new Vector3(myTransform.position.x, depthAfterHit, myTransform.position.z), 0.06f).OnComplete(ContactWithNail);
    }

    private void ContactWithNail()
    {
        targetNail.OnHammerContact();
    }

    private void SetupNextTarget()
    {
        if (targetIndex < NailsParent.transform.childCount - 1)
        {
            targetIndex++;
            targetNail = NailsParent.transform.GetChild(targetIndex).gameObject.GetComponent<Nail>();
            startingPosition += new Vector3(targetNail.Xoffset, 0f, 0f);
            startingPosition = new Vector3(startingPosition.x,targetNail.nailHead.transform.position.y + cashedPositionBeforeHit, startingPosition.z);
            myTransform.DOMove(startingPosition, 0.3f);
        }
        else
        {
            EventManager.RaiseEventGameOver();
        }
    }

    private void OnPerfectHit()
    {
       // Debug.Log("Perfect hit with " +targetNail.ScoreForNail+"score");
        LevelContainer.Score += (int)(targetNail.ScoreForNail*ConstantDataContainer.PercentageBonusForPerfectHit);
    }

    private void OnSpriteChanged()
    {
        var existingSprite = GetComponentInChildren<MeshRenderer>();
        if(existingSprite)
        {
            Destroy(existingSprite.gameObject);
        }
        var sprite = Instantiate(sprites.HammerSprite[PlayerPrefsManager.GetChosenHammer()], transform);
        sprite.transform.SetParent(transform);
    }

    private void OnDestroy()
    {
        EventManager.EventPerfectHit -= OnPerfectHit;
        EventManager.EventHammerSpriteChanged -= OnSpriteChanged;
    }

    private IEnumerator CoroutineHideMenuAndStartGame()
    {
        EventManager.RaiseEventMenuHided();
        yield return new WaitForSeconds(0.1f);
        EventManager.RaiseEventGameStarted();
    }

    public int GetStrength()
    {
        return strength;
    }
}
