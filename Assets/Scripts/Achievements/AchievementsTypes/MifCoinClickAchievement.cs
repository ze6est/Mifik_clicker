﻿using Assets.Scripts.Achievements;
using Assets.Scripts.Infrastructure.States;
using UnityEngine;

public class MifCoinClickAchievement : AchievementsButton<long>, ISavedProgress
{
    [SerializeField] private ClickButton _clickButton;

    protected override long Parametr { get; set; }

    public void Construct(ClickButton clickButton, long mifCoin)
    {
        _clickButton = clickButton;
        Parametr = mifCoin;
    }

    public void Construct(ClickButton clickButton)
    {
        _clickButton = clickButton;
    }

    protected override void Start()
    {
        base.Start();

        _clickButton.ButtonClicked += AddMifCoin;
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
        AudioSourceAchievement.PlayOneShot(GetAchievement);

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
                AudioSourceAchievement.PlayOneShot(OpenAchievement);
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
            _clickButton.ButtonClicked -= AddMifCoin;
        else if (Parametr >= TaskValues[AchievementNumber - 1])
        {
            TryGetPlayOpenAchievementSound();
            IsLocked = false;
            Button.interactable = true;
            TaskNameHUD.text = $"Заберите {TaskAwardPoints[AchievementNumber - 1]} поинтов";
        }
    }
}
