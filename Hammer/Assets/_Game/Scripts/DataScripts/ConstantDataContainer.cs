using UnityEngine;
using UnityEngine.Serialization;

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

    [Header("Nails Score")]
    [SerializeField] 
    private int defaultNail=100;
    public static int DefaultNail => Instance.defaultNail;

    [SerializeField]
    private int movingDefaultNail = 150;
    public static int MovingDefaultNail => Instance.movingDefaultNail;

    [SerializeField]
    private int mediumNail = 200;
    public static int MediumNail => Instance.mediumNail;

    [SerializeField]
    private int movingMediumNail = 300;
    public static int MovingMediumNail => Instance.movingMediumNail;

    [SerializeField] 
    private int redNail=300;
    public static int RedNail => Instance.redNail;

    [SerializeField]
    private int movingRedNail = 450;
    public static int MovingRedNail => Instance.movingRedNail;

    [Header("Score")]
    [SerializeField] 
    private int percentageBonusForPerfectHit = 50;
    public static float PercentageBonusForPerfectHit => Instance.percentageBonusForPerfectHit/100f;
    
    [SerializeField] 
    private int percentageValueFor1Star = 65;
    public static int PercentageValueFor1Star => Instance.percentageValueFor1Star;
    
    [SerializeField] 
    private int percentageValueFor2Stars = 75;
    public static int PercentageValueFor2Stars => Instance.percentageValueFor2Stars;
    
    [SerializeField] 
    private int percentageValueFor3Stars = 85;
    public static int PercentageValueFor3Stars => Instance.percentageValueFor3Stars;
    
    [Header("Coins")]
    [SerializeField] 
    private int scoreDivider = 10;

    public static int ScoreDivider => Instance.scoreDivider;
    

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
