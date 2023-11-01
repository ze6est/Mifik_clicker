using System.Collections.Generic;
using Assets.Scripts.Achievements;
using Assets.Scripts.Infrastructure.States;
using UnityEngine;

public class OpenMifiksAchievement : AchievementsButton<int>, ISavedProgress
{
    [SerializeField] private List<Card> _cards;

    protected override int Parametr { get; set; }

    public void Construct(List<Card> cards, int unlockedMifiksCount)
    {
        _cards = cards;
        Parametr = unlockedMifiksCount;        
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
            card.CardUnlocked += OnCardUnlocked;
        }

        CheckPoints();
    }

    public override void UpdateProgress(PlayerProgress progress)
    {
        base.UpdateProgress(progress);

        progress.Achievements[(int)Type].OpenMifiksAchievementProgress.OpenMifiksCount = Parametr;
    }

    public override void LoadProgress(PlayerProgress progress)
    {
        base.LoadProgress(progress);

        if (!LoadProgressState.IsNewProgress)
        {
            Parametr = progress.Achievements[(int)Type].OpenMifiksAchievementProgress.OpenMifiksCount;            
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

    private void OnCardUnlocked()
    {
        Parametr++;

        CheckPoints();
    }


    private void CheckPoints()
    {
        if (AchievementNumber > TaskValues.Length)
        {
            foreach (Card card in _cards)
            {
                card.CardUnlocked -= OnCardUnlocked;
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