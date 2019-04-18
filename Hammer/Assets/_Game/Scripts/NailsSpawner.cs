using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NailsSpawner : MonoBehaviour
{
    private int maxScoreForNails;
    public static int MaxScoreForNails => Instance.maxScoreForNails;

    [SerializeField] Nail defaultNail;
    [SerializeField] Nail redNail;

    private LevelData data;
    
    private int minHammerHits;
    private int numberOfNails;
    private float Xoffset = 2f;
    private int spawnedDefaultNails;
    private int spawnedRedNails;
    private int spawnedMovingDefaultNails;
    private int spawnedMovingRedNails;
    private int cashedScoreForMoves;
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
        cashedMaxHammerStrength = ConstantDataContainer.MaxHammerStrength;
    }

    private IEnumerator SpawnWithDelay()
    {
        yield return new WaitForEndOfFrame();
        data = LevelsDifficultyContainer.Houses[LevelContainer.CurrentHouseNumber-1].levelsData[LevelContainer.CurrentLevelIndex];

        numberOfNails = LevelContainer.NumberOfNails;
        //print(LevelContainer.CurrentLevelIndex);
        for (int i = 0; i < numberOfNails; )
        {
            int index = Random.Range(1, 3);

            switch (index)
            {
                case 1:
                    if (spawnedDefaultNails < data.numberOfDefaultNails)
                    {
                        Nail defNail = Instantiate(defaultNail, defaultNail.transform.position + new Vector3(Xoffset * i, 0f, 0f), Quaternion.identity) as DefaultNail;
                        defNail.gameObject.transform.SetParent(transform);
                        defNail.Xoffset = Xoffset;
                        if (IsMoving(ref spawnedMovingDefaultNails,data.movingDefaultNails,data.numberOfDefaultNails))
                        {
                            defNail.transform.position = defNail.DefaultPosition;
                            defNail.strengthForCorrectHit = defNail.DefaultStrengthForCorrectHit;
                            defNail.isMoving=true;
                        }

                        CalculatePoints(defNail.strengthForCorrectHit, defNail.ScoreForNail);
                        spawnedDefaultNails++;
                        i++;
                    }
                    break;
                case 2:
                    if (spawnedRedNails < data.numberOfRedNails)
                    {
                        Nail rNail = Instantiate(redNail, redNail.transform.position + new Vector3(Xoffset * i, 0f, 0f), Quaternion.identity) as RedNail;
                        rNail.gameObject.transform.SetParent(transform);
                        rNail.Xoffset = Xoffset;
                        if (IsMoving(ref spawnedMovingRedNails,data.movingRedNails,data.numberOfRedNails))
                        {
                            rNail.transform.position = rNail.DefaultPosition;
                            rNail.strengthForCorrectHit = rNail.DefaultStrengthForCorrectHit;
                            rNail.isMoving=true;
                        }
                        
                        CalculatePoints(rNail.strengthForCorrectHit, rNail.ScoreForNail);
                        spawnedRedNails++;
                        i++;
                    }
                    break;
            }
        }
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

    private bool IsMoving(ref int currentSpawnedNails,int targetSpawnedNails,int numberNotMovingNails)
    {
        if (currentSpawnedNails < targetSpawnedNails)
        {
            if (targetSpawnedNails > numberNotMovingNails / 2)     // try spawn all 
            {
                currentSpawnedNails++;
                return true;
            }
            else                                                             // add some randomness
            {
                var index = Random.Range(0, 2);
                if (index == 0)
                {
                    currentSpawnedNails++;
                    return true;
                }
            }
        }
        return false;
    }
}
