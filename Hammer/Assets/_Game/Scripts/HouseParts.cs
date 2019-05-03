using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HouseParts
{
    public int houseNumber;

    [SerializeField] public List<GameObject> parts = new List<GameObject>();
}

[CreateAssetMenu(menuName = "HouseParts", fileName = "HouseParts", order = 4)]
public class HousePartsArray:ScriptableObject
{
    [SerializeField] public List<HouseParts> houseParts = new List<HouseParts>();
}