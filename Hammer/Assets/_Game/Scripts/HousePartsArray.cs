using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HouseParts", fileName = "HouseParts", order = 4)]
[System.Serializable]
public class HousePartsArray : ScriptableObject
{
    [SerializeField] public List<HouseParts> houseParts = new List<HouseParts>();
}


