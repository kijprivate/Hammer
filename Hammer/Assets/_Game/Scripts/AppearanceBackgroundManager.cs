﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearanceBackgroundManager : MonoBehaviour
{
    [SerializeField] AppearanceData appearanceData;

    [SerializeField] Transform Board;
    [SerializeField] SpriteRenderer SummaryGround;
    [SerializeField] GameObject BackgroundGradient;
    [SerializeField] GameObject Clouds;
    [SerializeField] GameObject BackgroundEnvironment;

    [SerializeField] GameObject simpleBoardPrefab;

    Transform hammerTransform;
    Transform enviroTransform;
    Transform cloudTransform;
    float boardXOffset = 8f;
    float envXOffset = 24.83f;
    float hammerOffsetBoard = 8f;
    float hammerOffsetEnviro = 24.83f;
    float hammerOffsetClouds = 24.83f;
    int counterMultiplierBoard = 1;
    int counterMultiplierEnviro = 1;
    int counterMultiplierClouds = 1;

    private void Start()
    {
        hammerTransform = FindObjectOfType<Hammer>().transform;
        enviroTransform = BackgroundEnvironment.transform;
        cloudTransform = Clouds.transform;
        SummaryGround.sprite = appearanceData.houses[LevelContainer.CurrentHouseIndex].SummaryGround;
        cloudTransform.GetChild(0).GetComponent<SpriteRenderer>().sprite = appearanceData.houses[LevelContainer.CurrentHouseIndex].Clouds1;
        cloudTransform.GetChild(1).GetComponent<SpriteRenderer>().sprite = appearanceData.houses[LevelContainer.CurrentHouseIndex].Clouds2;
        cloudTransform.GetChild(2).GetComponent<SpriteRenderer>().sprite = appearanceData.houses[LevelContainer.CurrentHouseIndex].Clouds1;

        for (int i=0; i<3;i++)
        {
            var b = Instantiate(simpleBoardPrefab, new Vector3(boardXOffset*i, 0f, 0f),Quaternion.identity);
            b.transform.SetParent(Board);
            b.GetComponent<MeshRenderer>().material = appearanceData.houses[LevelContainer.CurrentHouseIndex].BoardMaterial;
        }
        foreach (Transform child in BackgroundEnvironment.transform)
        {
            child.GetComponent<SpriteRenderer>().sprite = appearanceData.houses[LevelContainer.CurrentHouseIndex].BackgroundEnvironment;
        }

        BackgroundGradient.GetComponent<MeshRenderer>().material.mainTexture = appearanceData.houses[LevelContainer.CurrentHouseIndex].BackgroundGradient;
        StartCoroutine(MoveBoard());

        //TODO easier/better way to do it - set plane with sprite and changing texture offset runtime
        StartCoroutine(MoveEnvironment());
        StartCoroutine(MoveClouds());
    }

    IEnumerator MoveBoard()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (hammerTransform.position.x > hammerOffsetBoard * counterMultiplierBoard)
            {
                if (Board.GetChild(0).transform.position.x < Board.GetChild(1).transform.position.x && Board.GetChild(0).transform.position.x < Board.GetChild(2).transform.position.x)
                {
                    Board.GetChild(0).transform.position += new Vector3(3 * boardXOffset, 0f, 0f);
                }
                else if (Board.GetChild(1).transform.position.x < Board.GetChild(0).transform.position.x && Board.GetChild(1).transform.position.x < Board.GetChild(2).transform.position.x)
                {
                    Board.GetChild(1).transform.position += new Vector3(3 * boardXOffset, 0f, 0f);
                }
                else
                {
                    Board.GetChild(2).transform.position += new Vector3(3 * boardXOffset, 0f, 0f);
                }
                counterMultiplierBoard++;
            }
        }
    }

    IEnumerator MoveEnvironment()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (hammerTransform.position.x > hammerOffsetEnviro * counterMultiplierEnviro)
            {
                if (enviroTransform.GetChild(0).transform.position.x < enviroTransform.GetChild(1).transform.position.x && enviroTransform.GetChild(0).transform.position.x < enviroTransform.GetChild(2).transform.position.x)
                {
                    enviroTransform.GetChild(0).transform.position += new Vector3(3 * envXOffset, 0f, 0f);
                }
                else if (enviroTransform.GetChild(1).transform.position.x < enviroTransform.GetChild(0).transform.position.x && enviroTransform.GetChild(1).transform.position.x < enviroTransform.GetChild(2).transform.position.x)
                {
                    enviroTransform.GetChild(1).transform.position += new Vector3(3 * envXOffset, 0f, 0f);
                }
                else
                {
                    enviroTransform.GetChild(2).transform.position += new Vector3(3 * envXOffset, 0f, 0f);
                }
                counterMultiplierEnviro++;
            }
        }
    }

    IEnumerator MoveClouds()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (hammerTransform.position.x > hammerOffsetClouds * counterMultiplierClouds)
            {
                if (cloudTransform.GetChild(0).transform.position.x < cloudTransform.GetChild(1).transform.position.x && cloudTransform.GetChild(0).transform.position.x < cloudTransform.GetChild(2).transform.position.x)
                {
                    cloudTransform.GetChild(0).transform.position += new Vector3(4 * hammerOffsetClouds, 0f, 0f);
                }
                else if (cloudTransform.GetChild(1).transform.position.x < cloudTransform.GetChild(0).transform.position.x && cloudTransform.GetChild(1).transform.position.x < cloudTransform.GetChild(2).transform.position.x)
                {
                    cloudTransform.GetChild(1).transform.position += new Vector3(4 * hammerOffsetClouds, 0f, 0f);
                }
                else
                {
                    cloudTransform.GetChild(2).transform.position += new Vector3(4 * hammerOffsetClouds, 0f, 0f);
                }
                counterMultiplierClouds++;
            }
        }
    }
}
