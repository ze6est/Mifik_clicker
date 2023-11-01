using System;

[Serializable]
public class AchievementProgress
{
    public int AchievementNumber;

    public bool IsLocked;

    public ClickCountAchievementProgress ClickCountAchievementProgress;
    public MifCoinAchievementProgress MifCoinAchievementProgress;
    public MifCoinAutoClickAchievementProgress MifCoinAutoClickAchievementProgress;
    public MifCoinClickAchievementProgress MifCoinClickAchievementProgress;
    public MifCoinPerClickAchievementProgress MifCoinPerClickAchievementProgress;
    public OpenMifiksAchievementProgress OpenMifiksAchievementProgress;

    public AchievementProgress()
    {
        ClickCountAchievementProgress = new ClickCountAchievementProgress();
        MifCoinAchievementProgress = new MifCoinAchievementProgress();
        MifCoinAutoClickAchievementProgress = new MifCoinAutoClickAchievementProgress();
        MifCoinClickAchievementProgress = new MifCoinClickAchievementProgress();
        MifCoinPerClickAchievementProgress = new MifCoinPerClickAchievementProgress();
        OpenMifiksAchievementProgress = new OpenMifiksAchievementProgress();
    }
}