using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using ControlFreak2;

public class Hammer : MonoBehaviour
{
    [SerializeField]
    float rotationSpeed = 10f;
    [SerializeField]
    float verticalMoveSpeed = 1f;
    [SerializeField]
    float HammerPositionOverNailHead = 1.5f;

    Transform myTransform;
    Nail targetNail;
    GameObject NailsParent;
    Quaternion startingRotation;
    Vector3 startingPosition;

    bool isMovingUp = true;
    bool isHammerReady = true;
    float rotationZ;
    float movingY;
    float depthAfterHit;
    int strength;
    int targetIndex = 0;

    void Start()
    {
        myTransform = transform;
        rotationZ = myTransform.rotation.z;
        movingY = myTransform.position.y;
        startingRotation = myTransform.rotation;
        startingPosition = myTransform.position;

        NailsParent = FindObjectOfType<NailsSpawner>().gameObject;
        targetNail = NailsParent.transform.GetChild(targetIndex).gameObject.GetComponent<Nail>();
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
                rotationZ += rotationSpeed * Time.deltaTime;
                movingY += verticalMoveSpeed*Time.deltaTime;
                myTransform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
                myTransform.position = new Vector3(myTransform.position.x, movingY, myTransform.position.z);

                if (myTransform.rotation.z >= 0.6f)
                { isMovingUp = false; }
            }
            else if (!isMovingUp)
            {
                rotationZ -= rotationSpeed * Time.deltaTime;
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
            SetupHammerPositionAfterHit();

            isHammerReady = false;

            HandleLoseCondition();
            EventManager.RaiseEventHammerHit();

            if (LevelContainer.GameOver)
            { return; }

            StartCoroutine(BackToPositionRoutine());
        }

    }

    private IEnumerator BackToPositionRoutine()
    {
        yield return new WaitForSeconds(0.5f);

        if (targetNail.isOverhit)
        {
            SetupNextTarget();
        }
        else
        {
            startingPosition = new Vector3(startingPosition.x, targetNail.nailHead.transform.position.y + HammerPositionOverNailHead, startingPosition.z);
            myTransform.DOMove(startingPosition, 0.2f);
        }

        yield return new WaitForSeconds(0.3f);
        myTransform.rotation = startingRotation;
        startingRotation = myTransform.rotation;
        movingY = myTransform.position.y;
        rotationZ = myTransform.rotation.z;

        isHammerReady = true;
    }

    private void HandleLoseCondition()
    {
        LevelContainer.HammerHits--;
        if (LevelContainer.HammerHits <= 0)
        { 
            EventManager.RaiseEventGameOver();
        }
    }
    private void SetupStrength() // Hardcoded numbers based on hammer angle
    {
        if (myTransform.rotation.z >= 0f && myTransform.rotation.z < 0.12f)
        { strength = 1; }
        else if (myTransform.rotation.z >= 0.12f && myTransform.rotation.z < 0.24f)
        { strength = 2; }
        else if (myTransform.rotation.z >= 0.24f && myTransform.rotation.z < 0.36f)
        { strength = 3; }
        else if (myTransform.rotation.z >= 0.36f && myTransform.rotation.z < 0.48f)
        { strength = 4; }
        else if (myTransform.rotation.z >= 0.48f && myTransform.rotation.z <= 0.60f)
        { strength = 5; }
    }

    private void SetupHammerPositionAfterHit()
    {
        if (strength > targetNail.GetStrengthForPerfectHit())
        {
            depthAfterHit = targetNail.nailHead.position.y + 0.7f - (targetNail.GetStrengthForPerfectHit() * targetNail.GetStep() + (strength - targetNail.GetStrengthForPerfectHit()) * (targetNail.GetStep() / 2f));
        }
        else
        {
            depthAfterHit = targetNail.nailHead.position.y - strength * targetNail.GetStep() + 0.7f;
        }

        myTransform.DORotateQuaternion(Quaternion.Euler(0f, 0f, 0f), 0.06f);
        myTransform.DOMove(new Vector3(myTransform.position.x, depthAfterHit, myTransform.position.z), 0.06f);
    }

    private void SetupNextTarget()
    {
        if (targetIndex < NailsParent.transform.GetChildCount() - 1)
        {
            targetIndex++;
            targetNail = NailsParent.transform.GetChild(targetIndex).gameObject.GetComponent<Nail>();
            startingPosition += new Vector3(targetNail.Xoffset, 0f, 0f);
            startingPosition = new Vector3(startingPosition.x,targetNail.nailHead.transform.position.y + HammerPositionOverNailHead, startingPosition.z);
            myTransform.DOMove(startingPosition, 0.3f);
        }
        else
        {
            EventManager.RaiseEventNoMoreNails();
        }
    }

    private IEnumerator CoroutineHideMenuAndStartGame()
    {
        EventManager.RaiseEventMenuHided();
        yield return new WaitForSeconds(1f);
        EventManager.RaiseEventGameStarted();
    }

    public int GetStrength()
    {
        return strength;
    }
}
