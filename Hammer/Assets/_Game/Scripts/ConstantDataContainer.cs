using UnityEngine;

[CreateAssetMenu(menuName = "DataContainer", fileName = "ConstantDataConfig",order = 1)]
public class ConstantDataContainer:ScriptableObject 
{
    [Header("Hammer")]
    [SerializeField,Tooltip("Divides hammer angle to strength intervals. Number of intervals = introduced variable + 1. First strength interval is 0")]
    int maxHammerStrength = 5;
    public static int MaxHammerStrength => Instance.maxHammerStrength;
    
    [SerializeField]
    float positionOverNailHeadBeforeHit = 1.5f;
    public static float PositionOverNailHeadBeforeHit => Instance.positionOverNailHeadBeforeHit;
    
    [SerializeField]
    float positionOverNailHeadAfterHit = 0.7f;
    public static float PositionOverNailHeadAfterHit => Instance.positionOverNailHeadAfterHit;

    [Header("Nails")]
    [SerializeField] 
    private int scoreForDefaultNail=300;
    public static int ScoreForDefaultNail => Instance.scoreForDefaultNail;
    
    [SerializeField] 
    private int scoreForRedNail=600;
    public static int ScoreForRedNail => Instance.scoreForRedNail;



    static ConstantDataContainer instance;
    public static ConstantDataContainer Instance
    {
        get
        {
            if (instance == null)
                instance = Resources.Load<ConstantDataContainer>("ConstantDataConfig");
            return instance;
        }
    }
}
