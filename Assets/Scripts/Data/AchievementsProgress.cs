using System;

[Serializable]
public class AchievementProgress
{
    public int AchievementNumber;

    public bool IsLocked;

    public ClickCountAchievementProgress ClickCountAchievementProgress;

    public AchievementProgress()
    {
        ClickCountAchievementProgress = new ClickCountAchievementProgress();
    }
}