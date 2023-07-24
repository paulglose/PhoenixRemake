using UnityEngine;

[CreateAssetMenu(menuName ="Stat")]
public class Stat: UnityEngine.ScriptableObject
{
    [SerializeField] float StatBase;
    [SerializeField] StatType statType;
    float StatImprovement;

    public float Value { private set; get; }
   
    /// <summary>
    /// Resets the values of the "Value" variable. So the actual stats
    /// of the Stat are correct and updated. 
    /// </summary>
    public void ResetStat()
    {
        switch (statType)
        {
            case StatType.Additive: Value = StatBase + StatImprovement; break;
            case StatType.Multiplikative: Value = StatBase * StatImprovement; break;
        }
    }

    public void OnEnable()
    {
        switch (statType)
        {
            case StatType.Additive: StatImprovement = 0; break;
            case StatType.Multiplikative: StatImprovement = 1; break;
        }

        ResetStat();
    }

    public void ImproveStat(float by)
    {
        StatImprovement += by;
        ResetStat();
    }

    // Implicit conversion from Stat to float.
    public static implicit operator float(Stat s)
    {
        return s.Value;
    }
}