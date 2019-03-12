using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelContainer : MonoBehaviour
{
    [SerializeField] int hammerHits = 12;
    //[SerializeField] int numberOfNails = 10;

    public static int HammerHits{ get; set; }
    //public static int NumberOfNails { get; set; }

    void Awake()
    {
        HammerHits = hammerHits;
        //NumberOfNails = numberOfNails;
    }
}
