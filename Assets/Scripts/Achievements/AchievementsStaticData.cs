using Assets.Scripts.Achievements;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "AchievementsData", menuName = "StaticData/Achievement", order = 51)]
public class AchievementsStaticData : ScriptableObject
{
    public AchievementsType AchievementsType;

    public string TaskName;
    public int TaskCount;    
    public int[] TaskValues;
    public long[] TaskAwardPoints;

    public Image Icon;
    public bool IsLocked;    
}
