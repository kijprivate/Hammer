using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NailsSpawner : MonoBehaviour
{
    public static int MAX_SCORE_FOR_NAILS=0;
    public static int MIN_HAMMER_HITS=0;

    [SerializeField] Nail defaultNail;
    [SerializeField] Nail redNail;

    private int numberOfNails;
    float Xoffset = 2f;
    int defaultNails;
    int redNails;

    private void Awake()
    {
        var data = LevelsDifficultyContainer.LevelsData[SceneManager.GetActiveScene().buildIndex];
        numberOfNails = LevelContainer.NumberOfNails;
        for (int i = 0; i < numberOfNails; )
        {
            int index = Random.Range(1, 3);

            switch (index)
            {
                case 1:
                    if (defaultNails < data.numberOfDefaultNails)
                    {
                        Nail defNail = Instantiate(defaultNail, defaultNail.transform.position + new Vector3(Xoffset * i, 0f, 0f), Quaternion.identity) as Nail;
                        defNail.gameObject.transform.SetParent(transform);
                        defNail.Xoffset = Xoffset;

                        CalculatePoints(defNail.GetStrengthForPerfectHit(), defNail.ScoreForNail);
                        defaultNails++;
                        i++;
                    }
                    break;
                case 2:
                    if (redNails < data.numberOfRedNails)
                    {
                        Nail rNail = Instantiate(redNail, redNail.transform.position + new Vector3(Xoffset * i, 0f, 0f), Quaternion.identity) as Nail;
                        rNail.gameObject.transform.SetParent(transform);
                        rNail.Xoffset = Xoffset;

                        CalculatePoints(rNail.GetComponent<Nail>().GetStrengthForPerfectHit(), rNail.GetComponent<Nail>().ScoreForNail);
                        redNails++;
                        i++;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private void CalculatePoints(int strengthForPerfectHit,int scoreForNail)
    {
        if (strengthForPerfectHit % 5 == 0)
        {
            MAX_SCORE_FOR_NAILS += scoreForNail / (strengthForPerfectHit / 5);
            MIN_HAMMER_HITS += strengthForPerfectHit / 5;
        }
        else
        {
            MAX_SCORE_FOR_NAILS += scoreForNail / (strengthForPerfectHit / 5 + 1);
            MIN_HAMMER_HITS += strengthForPerfectHit / 5 + 1;
        }
    }

    private void OnDestroy()
    {
        MAX_SCORE_FOR_NAILS = 0;
        MIN_HAMMER_HITS = 0;
    }
}
