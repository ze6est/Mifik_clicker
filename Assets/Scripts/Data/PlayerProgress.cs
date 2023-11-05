using System;
using Assets.Scripts.Achievements;

[Serializable]
public class PlayerProgress
{
    public CardProgress[] Cards;
    public AchievementProgress[] Achievements;
    public long Points;
    public long PointsPerClick;

    private CardProgress CardProgress;
    private AchievementProgress AchievementProgress;

    public PlayerProgress()
    {        
        Cards = new CardProgress[Enum.GetNames(typeof(MifiksName)).Length];
        Achievements = new AchievementProgress[Enum.GetNames(typeof(AchievementsType)).Length];

        for (int i = 0; i < Cards.Length; i++)
        {
            CardProgress = new CardProgress();
            Cards[i] = CardProgress;
        }

        for (int i = 0; i < Achievements.Length; i++)
        {
            AchievementProgress = new AchievementProgress();
            Achievements[i] = AchievementProgress;
        }
    }
}