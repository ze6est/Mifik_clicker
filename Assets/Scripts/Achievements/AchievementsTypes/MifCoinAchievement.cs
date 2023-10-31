using Assets.Scripts.Achievements;
using Assets.Scripts.Infrastructure.States;

public class MifCoinAchievement : AchievementsButton<long>, ISavedProgress
{
    protected override long Parametr { get; set; }    

    public void Construct(long mifCoin)
    {        
        Parametr = mifCoin;
    }    

    protected override void Start()
    {
        base.Start();

        Points.PointReceived += AddMifCoin;
        CheckPoints();
    }

    public override void UpdateProgress(PlayerProgress progress)
    {
        base.UpdateProgress(progress);

        progress.Achievements[(int)Type].MifCoinAchievementProgress.MifCoin = Parametr;
    }

    public override void LoadProgress(PlayerProgress progress)
    {
        base.LoadProgress(progress);

        if (!LoadProgressState.IsNewProgress)
        {
            Parametr = progress.Achievements[(int)Type].MifCoinAchievementProgress.MifCoin;
        }
    }

    protected override void GetAwardPoints()
    {
        Points.RefreshPoints(TaskAwardPoints[AchievementNumber - 1]);

        AchievementNumber++;

        if (AchievementNumber > TaskValues.Length)
        {
            Button.interactable = false;
            IsLocked = true;
            TaskNameHUD.text = "Все задания выполнены";
            Button.onClick.RemoveListener(GetAwardPoints);
        }
        else
        {
            if (Parametr >= TaskValues[AchievementNumber - 1])
            {
                IsLocked = false;
                Button.interactable = true;
                TaskNameHUD.text = $"Заберите {TaskAwardPoints[AchievementNumber - 1]} поинтов";
            }
            else
            {
                IsLocked = true;
                Button.interactable = false;
                TaskNameHUD.text = $"{TaskName} {TaskValues[AchievementNumber - 1]}";
            }
        }
    }

    private void AddMifCoin(long mifCoin)
    {
        Parametr += mifCoin;

        CheckPoints();
    }

    private void CheckPoints()
    {
        if (AchievementNumber > TaskValues.Length)
            Points.PointReceived -= AddMifCoin;
        else if (Parametr >= TaskValues[AchievementNumber - 1])
        {
            IsLocked = false;
            Button.interactable = true;
            TaskNameHUD.text = $"Заберите {TaskAwardPoints[AchievementNumber - 1]} поинтов";
        }
    }
}
