using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HouseParts
{
    public int houseNumber;

    [SerializeField] public List<GameObject> parts = new List<GameObject>();
    [SerializeField] public List<GameObject> specialItems = new List<GameObject>();
}
