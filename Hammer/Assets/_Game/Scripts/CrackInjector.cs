using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackInjector : MonoBehaviour
{
    [SerializeField] AppearanceData appearanceData;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = appearanceData.houses[LevelContainer.CurrentHouseIndex].BoardCrack;
    }
}
