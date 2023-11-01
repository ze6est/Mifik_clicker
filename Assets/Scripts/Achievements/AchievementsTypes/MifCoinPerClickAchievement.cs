using Assets.Scripts.Achievements;
using Assets.Scripts.Infrastructure.States;
using UnityEngine;

public class MifCoinPerClickAchievement : AchievementsButton<long>, ISavedProgress
{
    [SerializeField] private AwardsPerClick _awardsPerClick;

    protected override long Parametr { get; set; }

    public void Construct(AwardsPerClick awardsPerClick, long mifCoinPerClick)
    {
        _awardsPerClick = awardsPerClick;
        Parametr = mifCoinPerClick;
    }

    public void Construct(AwardsPerClick awardsPerClick)
    {
        _awardsPerClick = awardsPerClick;
    }

    protected override void Start()
    {
        base.Start();

        _awardsPerClick.PointsPerClickReceived += GetPointsPerClick;
        CheckPoints();
    }
   
    public override void UpdateProgress(PlayerProgress progress)
    {
        base.UpdateProgress(progress);

        progress.Achievements[(int)Type].MifCoinPerClickAchievementProgress.MifCoinPerClick = Parametr;
    }

    public override void LoadProgress(PlayerProgress progress)
    {
        base.LoadProgress(progress);

        if (!LoadProgressState.IsNewProgress)
        {
            Parametr = progress.Achievements[(int)Type].MifCoinPerClickAchievementProgress.MifCoinPerClick;
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

    private void GetPointsPerClick(long pointsPerClick)
    {
        Parametr = pointsPerClick;
        CheckPoints();
    }

    private void CheckPoints()
    {
        if (AchievementNumber > TaskValues.Length)
            _awardsPerClick.PointsPerClickReceived -= GetPointsPerClick;
        else if (Parametr >= TaskValues[AchievementNumber - 1])
        {
            IsLocked = false;
            Button.interactable = true;
            TaskNameHUD.text = $"Заберите {TaskAwardPoints[AchievementNumber - 1]} поинтов";
        }
    }
}