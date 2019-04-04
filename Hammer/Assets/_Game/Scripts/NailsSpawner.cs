using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NailsSpawner : MonoBehaviour
{
    private int maxAvailableScore;
    public static int MaxAvailableScore => Instance.maxAvailableScore;

    [SerializeField] Nail defaultNail;
    [SerializeField] Nail redNail;

    private int maxScoreForNails;
    private int minHammerHits;
    private int numberOfNails;
    float Xoffset = 2f;
    int defaultNails;
    int redNails;
    private int cashedScoreForMoves;
    private int cashedScoreForNails;
    private int cashedMaxHammerStrength;
    
    static NailsSpawner instance;
    public static NailsSpawner Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<NailsSpawner>();
            }
            return instance;
        }
    }
    private void Awake()
    {
        StartCoroutine(SpawnWithDelay());
        cashedScoreForMoves = ConstantDataContainer.ScoreDuplicatorForMovesLeft;
        cashedScoreForNails = ConstantDataContainer.ScoreDuplicatorForCorrectNails;
        cashedMaxHammerStrength = ConstantDataContainer.MaxHammerStrength;
    }

    private IEnumerator SpawnWithDelay()
    {
        yield return new WaitForEndOfFrame();
        var data = LevelsDifficultyContainer.LevelsData[PlayerPrefsManager.GetChosenLevelNumber()-1];
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

                        CalculatePoints(defNail.GetStrengthForCorrectHit(), defNail.ScoreForNail);
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

                        CalculatePoints(rNail.GetStrengthForCorrectHit(), rNail.ScoreForNail);
                        redNails++;
                        i++;
                    }
                    break;
            }
        }
        maxAvailableScore = maxScoreForNails + (LevelContainer.HammerHits - minHammerHits)*cashedScoreForMoves +numberOfNails*cashedScoreForNails;
        print(maxAvailableScore);
    }
    private void CalculatePoints(int strengthForCorrectHit,int scoreForNail)
    {
        if (strengthForCorrectHit % cashedMaxHammerStrength == 0)
        {
            maxScoreForNails += scoreForNail / (strengthForCorrectHit / cashedMaxHammerStrength);
            minHammerHits += strengthForCorrectHit / cashedMaxHammerStrength;
        }
        else
        {
            maxScoreForNails += scoreForNail / (strengthForCorrectHit / cashedMaxHammerStrength + 1);
            minHammerHits += strengthForCorrectHit / cashedMaxHammerStrength + 1;
        }
    }
}
