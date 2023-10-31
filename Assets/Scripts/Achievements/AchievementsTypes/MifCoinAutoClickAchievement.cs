using System.Collections.Generic;
using Assets.Scripts.Achievements;
using Assets.Scripts.Infrastructure.States;
using UnityEngine;

public class MifCoinAutoClickAchievement : AchievementsButton<long>, ISavedProgress
{
    [SerializeField] private long _parametr;
    [SerializeField] private List<Card> _cards;    

    protected override long Parametr { get; set; }

    public void Construct(List<Card> cards, long mifCoin)
    {
        _cards = cards;
        Parametr = mifCoin;
        _parametr = Parametr;
    }

    public void Construct(List<Card> cards)
    {
        _cards = cards;        
    }

    protected override void Start()
    {
        base.Start();

        foreach (Card card in _cards)
        {
            card.PointsReceived += AddMifCoin;            
        }
        
        CheckPoints();
    }

    public override void UpdateProgress(PlayerProgress progress)
    {
        base.UpdateProgress(progress);

        progress.Achievements[(int)Type].MifCoinAutoClickAchievementProgress.MifCoin = Parametr;
    }

    public override void LoadProgress(PlayerProgress progress)
    {
        base.LoadProgress(progress);

        if (!LoadProgressState.IsNewProgress)
        {
            Parametr = progress.Achievements[(int)Type].MifCoinAutoClickAchievementProgress.MifCoin;
            _parametr = Parametr;
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
        _parametr = Parametr;

        CheckPoints();
    }

    private void CheckPoints()
    {
        if (AchievementNumber > TaskValues.Length)
        {
            foreach (Card card in _cards)
            {
                card.PointsReceived -= AddMifCoin;
            }
        }            
        else if (Parametr >= TaskValues[AchievementNumber - 1])
        {
            IsLocked = false;
            Button.interactable = true;
            TaskNameHUD.text = $"Заберите {TaskAwardPoints[AchievementNumber - 1]} поинтов";
        }
    }
}