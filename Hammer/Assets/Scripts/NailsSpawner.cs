using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailsSpawner : MonoBehaviour
{
    public static int MAX_SCORE_FOR_NAILS=0;
    public static int MIN_HAMMER_HITS=0;
    public static int NUMBER_OF_NAILS;
    [SerializeField] GameObject defaultNail;
    [SerializeField] int numberOfDefaultNails=10;
    [SerializeField,Tooltip("Distance between nails")]
    float Xoffset = 2f;

    private void Awake()
    {
        NUMBER_OF_NAILS = numberOfDefaultNails;
        for (int i = 0; i < numberOfDefaultNails; i++)
        {
            GameObject nail = Instantiate(defaultNail, defaultNail.transform.position + new Vector3(Xoffset * i, 0f, 0f), Quaternion.identity);
            nail.transform.SetParent(this.transform);
            MAX_SCORE_FOR_NAILS += nail.GetComponent<Nail>().scoreForNail;
            MIN_HAMMER_HITS += nail.GetComponent<Nail>().GetStrengthForPerfectHit()/5 + 1;
        }
    }

    private void OnDestroy()
    {
        MAX_SCORE_FOR_NAILS = 0;
        MIN_HAMMER_HITS = 0;
        NUMBER_OF_NAILS = 0;
    }
}
