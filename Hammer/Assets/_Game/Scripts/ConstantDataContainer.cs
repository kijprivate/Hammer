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

    [Header("Nails")]
    [SerializeField] 
    private int scoreForDefaultNail=300;
    public static int ScoreForDefaultNail => Instance.scoreForDefaultNail;
    
    [SerializeField] 
    private int scoreForRedNail=600;
    public static int ScoreForRedNail => Instance.scoreForRedNail;

    [Header("Score")]
    [SerializeField] 
    private int scoreBonusForPerfectHit = 100;
    public static int ScoreBonusForPerfectHit => Instance.scoreBonusForPerfectHit;
    
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
    private int scoreDividerWhenMoreStars = 100;

    public static int ScoreDividerWhenMoreStars => Instance.scoreDividerWhenMoreStars;
    
    [SerializeField] 
    private int scoreDividerWhenSameStars = 200;

    public static int ScoreDividerWhenSameStars => Instance.scoreDividerWhenSameStars;

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
