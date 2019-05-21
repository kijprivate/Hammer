using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public static Tutorial Instance;
    public static bool wasPlayed;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; } 
        DontDestroyOnLoad(gameObject); 
        Instance = this;
    }
}
