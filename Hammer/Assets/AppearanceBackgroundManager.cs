using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearanceBackgroundManager : MonoBehaviour
{
    [SerializeField] AppearanceData appearanceData;

    [SerializeField] Transform Board;
    [SerializeField] GameObject BackgroundGradient;

    [SerializeField] GameObject simpleBoardPrefab;

    Transform hammerTransform;
    float boardXOffset = 8f;
    float hammerOffsetMin = 8f;
    float hammerOffsetMax = 11f;
    int counterMultiplier = 1;

    private void Start()
    {
        hammerTransform = FindObjectOfType<Hammer>().transform;

        for(int i=0; i<3;i++)
        {
            var b = Instantiate(simpleBoardPrefab, new Vector3(boardXOffset*i, 0f, 0f),Quaternion.identity);
            b.transform.SetParent(Board);
            b.GetComponent<MeshRenderer>().material = appearanceData.houses[LevelContainer.CurrentHouseIndex].BoardMaterial;
        }

        BackgroundGradient.GetComponent<MeshRenderer>().material.mainTexture = appearanceData.houses[LevelContainer.CurrentHouseIndex].BackgroundGradient;
        StartCoroutine(MoveBoard());
    }

    IEnumerator MoveBoard()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (hammerTransform.position.x > hammerOffsetMin * counterMultiplier)
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
                counterMultiplier++;
            }
        }
    }
}
