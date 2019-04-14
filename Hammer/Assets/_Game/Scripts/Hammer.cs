using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using ControlFreak2;

public class Hammer : MonoBehaviour
{
    public int targetIndex { get; private set; }
    
    private LevelData data;
    Transform myTransform;
    Nail targetNail;
    GameObject NailsParent;
    Quaternion startingRotation;
    Vector3 startingPosition;

    private float cashedPositionBeforeHit;
    private float cashedPositionAfterHit;
    bool isMovingUp = true;
    bool isHammerReady;
    float rotationZ;
    float movingY;
    float depthAfterHit;
    int strength;
    private int cashedMaxStrength;
    private ParticleSystem sparks;

    void Start()
    {
        myTransform = transform;
        sparks = GetComponentInChildren<ParticleSystem>();
        
        StartCoroutine(SetupWithDelay());
    }

    private IEnumerator SetupWithDelay()
    {
        yield return new WaitForSeconds(0.2f);
        data = LevelsDifficultyContainer.LevelsData[PlayerPrefsManager.GetChosenLevelNumber()-1];
        NailsParent = FindObjectOfType<NailsSpawner>().gameObject;
        EventManager.EventPerfectHit += OnPerfectHit;
        targetNail = NailsParent.transform.GetChild(targetIndex).gameObject.GetComponent<Nail>();

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
            if (isMovingUp)
            {
                rotationZ += data.rotationSpeed * Time.deltaTime;
                movingY += data.verticalMoveSpeed*Time.deltaTime;
                myTransform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
                myTransform.position = new Vector3(myTransform.position.x, movingY, myTransform.position.z);

                if (myTransform.rotation.z >= 0.6f)
                { isMovingUp = false; }
            }
            else if (!isMovingUp)
            {
                rotationZ -= data.rotationSpeed * Time.deltaTime;
                movingY -= data.verticalMoveSpeed*Time.deltaTime;
                myTransform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
                myTransform.position = new Vector3(myTransform.position.x, movingY, myTransform.position.z);

                if (myTransform.rotation.z <= 0f)
                { isMovingUp = true; }
            }
        }

        if (CF2Input.GetButtonUp("Click") && isHammerReady && LevelContainer.GameStarted)
        {
            SetupStrength();
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
        float strengthInterval = 0.60f / (cashedMaxStrength+1);

        for(int i = 0; i < cashedMaxStrength+1; i++)
        {
            if(myTransform.rotation.z >= i*strengthInterval && myTransform.rotation.z < (i+1)*strengthInterval)
            { strength = i; }
        }
    }

    private void SetupHammerPositionAfterHit()
    {
        if (strength > targetNail.GetStrengthForCorrectHit())
        {
            depthAfterHit = targetNail.nailHead.position.y + cashedPositionAfterHit - 
                            (targetNail.GetStrengthForCorrectHit() * targetNail.GetStep() + (strength - targetNail.GetStrengthForCorrectHit()) * (targetNail.GetStep() / 3f));
            EventManager.RaiseEventShowSplash(1);   // raises event needed for displaying splash with hit rating
        }
        else
        {
            if (strength == targetNail.GetStrengthForCorrectHit())
            {
                sparks.Play();
                EventManager.RaiseEventShowSplash(0); // raises event needed for displaying splash with hit rating
            }
            else EventManager.RaiseEventShowSplash(-1); // raises event needed for displaying splash with hit rating
            depthAfterHit = targetNail.nailHead.position.y - strength * targetNail.GetStep() + cashedPositionAfterHit;
        }

        myTransform.DORotateQuaternion(Quaternion.Euler(0f, 0f, 0f), 0.06f);
        myTransform.DOMove(new Vector3(myTransform.position.x, depthAfterHit, myTransform.position.z), 0.06f);
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
            EventManager.RaiseEventNoMoreNails();
        }
    }

    private void OnPerfectHit()
    {
        Debug.Log("Perfect hit with " +targetNail.ScoreForNail+"score");
        LevelContainer.Score += ConstantDataContainer.ScoreBonusForPerfectHit;
    }

    private void OnDestroy()
    {
        EventManager.EventPerfectHit -= OnPerfectHit;
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
