using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HammerSprites", fileName = "HammerSprites", order = 1)]
public class HammerSprites : ScriptableObject
{
    [SerializeField] public GameObject[] HammerSprite;
}
