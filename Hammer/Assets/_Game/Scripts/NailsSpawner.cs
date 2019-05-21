using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailsSpawner : MonoBehaviour
{
    [SerializeField] Nail defaultNail;
    [SerializeField] Nail mediumNail;
    [SerializeField] Nail redNail;

    private LevelData data;

    private int numberOfNails;
    private float Xoffset = 2.5f;
    private int spawnedDefaultNails;
    private int spawnedMediumNails;
    private int spawnedRedNails;
    private int spawnedMovingDefaultNails;
    private int spawnedMovingMediumNails;
    private int spawnedMovingRedNails;
    
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
    }

    private IEnumerator SpawnWithDelay()
    {
        yield return new WaitForEndOfFrame();
        data = LevelsDifficultyContainer.Houses[LevelContainer.CurrentHouseNumber-1].levelsData[LevelContainer.CurrentLevelIndex];

        numberOfNails = LevelContainer.NumberOfNails;
        int i = 0;
        while (i < numberOfNails)
        {
            yield return new WaitForEndOfFrame();

            int index = Random.Range(1, 7);

            switch (index)
            {
                case 1:
                    if (spawnedDefaultNails < data.numberOfDefaultNails)
                    {
                        Nail defNail = Instantiate(defaultNail, defaultNail.transform.position + new Vector3(Xoffset * i, 0f, 0f), Quaternion.identity) as DefaultNail;
                        defNail.gameObject.transform.SetParent(transform);
                        defNail.Xoffset = Xoffset;

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

                        spawnedRedNails++;
                        i++;
                    }
                    break;
                case 3:
                    if (spawnedMediumNails < data.numberOfMediumNails)
                    {
                        Nail mNail = Instantiate(mediumNail, mediumNail.transform.position + new Vector3(Xoffset * i, 0f, 0f), Quaternion.identity) as MediumNail;
                        mNail.gameObject.transform.SetParent(transform);
                        mNail.Xoffset = Xoffset;

                        spawnedMediumNails++;
                        i++;
                    }
                    break;
                case 4:
                    if (spawnedMovingDefaultNails < data.movingDefaultNails)
                    {
                        Nail defNail = Instantiate(defaultNail, defaultNail.transform.position + new Vector3(Xoffset * i, 0f, 0f), Quaternion.identity) as DefaultNail;
                        defNail.gameObject.transform.SetParent(transform);
                        defNail.Xoffset = Xoffset;

                        defNail.transform.position = defNail.DefaultPosition;
                        defNail.isMoving = true;

                        spawnedMovingDefaultNails++;
                        i++;
                    }
                    break;
                case 5:
                    if (spawnedMovingRedNails < data.movingRedNails)
                    {
                        Nail rNail = Instantiate(redNail, redNail.transform.position + new Vector3(Xoffset * i, 0f, 0f), Quaternion.identity) as RedNail;
                        rNail.gameObject.transform.SetParent(transform);
                        rNail.Xoffset = Xoffset;

                        rNail.transform.position = rNail.DefaultPosition;
                        rNail.isMoving = true;

                        spawnedMovingRedNails++;
                        i++;
                    }
                    break;
                case 6:
                    if (spawnedMovingMediumNails < data.movingMediumNails)
                    {
                        Nail mNail = Instantiate(mediumNail, mediumNail.transform.position + new Vector3(Xoffset * i, 0f, 0f), Quaternion.identity) as MediumNail;
                        mNail.gameObject.transform.SetParent(transform);
                        mNail.Xoffset = Xoffset;

                        mNail.transform.position = mNail.DefaultPosition;
                        mNail.isMoving = true;

                        spawnedMovingMediumNails++;
                        i++;
                    }
                    break;
            }
        }
    }
}
