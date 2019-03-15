using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailsSpawner : MonoBehaviour
{
    public static int MAX_SCORE_FOR_NAILS=0;
    public static int MIN_HAMMER_HITS=0;
    public static int NUMBER_OF_NAILS;
    [SerializeField] GameObject defaultNail;
    [SerializeField] GameObject redNail;
    [SerializeField] int numberOfDefaultNails = 10;
    [SerializeField] int numberOfRedNails = 10;

    float Xoffset = 2f;
    int defaultNails;
    int redNails;

    private void Awake()
    {
        NUMBER_OF_NAILS = numberOfDefaultNails + numberOfRedNails;
        for (int i = 0; i < NUMBER_OF_NAILS; )
        {
            int index = Random.Range(1, 3);

            switch (index)
            {
                case 1:
                    if (defaultNails < numberOfDefaultNails)
                    {
                        GameObject defNail = Instantiate(defaultNail, defaultNail.transform.position + new Vector3(Xoffset * i, 0f, 0f), Quaternion.identity);
                        defNail.transform.SetParent(this.transform);

                        CalculatePoints(defNail.GetComponent<Nail>().GetStrengthForPerfectHit(), defNail.GetComponent<Nail>().scoreForNail);
                        defaultNails++;
                        i++;
                    }
                    break;
                case 2:
                    if (redNails < numberOfRedNails)
                    {
                        GameObject rNail = Instantiate(this.redNail, this.redNail.transform.position + new Vector3(Xoffset * i, 0f, 0f), Quaternion.identity);
                        rNail.transform.SetParent(this.transform);

                        CalculatePoints(rNail.GetComponent<Nail>().GetStrengthForPerfectHit(), rNail.GetComponent<Nail>().scoreForNail);
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
        NUMBER_OF_NAILS = 0;
    }
}
