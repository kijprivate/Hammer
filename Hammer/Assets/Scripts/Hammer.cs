using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ControlFreak2;

public class Hammer : MonoBehaviour
{
    [SerializeField]
    float speed = 10f;
    [SerializeField]
    GameObject NailsParent;

    Nail targetNail;
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
        rotationZ = this.transform.rotation.z;
        movingY = this.transform.position.y;
        startingRotation = transform.rotation;
        startingPosition = transform.position;

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
                rotationZ += speed * Time.deltaTime;
                this.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

                if (transform.rotation.z >= 0.6f)
                { isMovingUp = false; }
            }
            else if (!isMovingUp)
            {
                rotationZ -= speed * Time.deltaTime;
                this.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

                if (transform.rotation.z <= 0f)
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
        { transform.position = startingPosition; }
        transform.rotation = startingRotation;

        startingRotation = transform.rotation;
        startingPosition = transform.position;

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
    private void SetupStrength()
    {
        if (transform.rotation.z >= 0f && transform.rotation.z < 0.12f)
        { strength = 1; }
        else if (transform.rotation.z >= 0.12f && transform.rotation.z < 0.24f)
        { strength = 2; }
        else if (transform.rotation.z >= 0.24f && transform.rotation.z < 0.36f)
        { strength = 3; }
        else if (transform.rotation.z >= 0.36f && transform.rotation.z < 0.48f)
        { strength = 4; }
        else if (transform.rotation.z >= 0.48f && transform.rotation.z <= 0.60f)
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

        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        transform.position = new Vector3(transform.position.x, depthAfterHit, transform.position.z);
        rotationZ = this.transform.rotation.z;
    }

    private void SetupNextTarget()
    {
        if (targetIndex < NailsParent.transform.GetChildCount() - 1)
        {
            targetIndex++;
            targetNail = NailsParent.transform.GetChild(targetIndex).gameObject.GetComponent<Nail>();
            transform.position = startingPosition + new Vector3(2f, 0f, 0f);
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
