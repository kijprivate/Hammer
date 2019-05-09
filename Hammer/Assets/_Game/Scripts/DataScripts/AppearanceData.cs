using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "AppearanceData", fileName = "AppearanceData", order = 5)]
public class AppearanceData:ScriptableObject
{
    [SerializeField] public List<HouseAppearance> houses = new List<HouseAppearance>();
}

[System.Serializable]
public class HouseAppearance
{
    public int houseNumber;

    [SerializeField] public Sprite ShowIcon;
}
