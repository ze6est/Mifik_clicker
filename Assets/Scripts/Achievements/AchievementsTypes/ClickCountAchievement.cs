using Assets.Scripts.Infrastructure.States;
using UnityEngine;

namespace Assets.Scripts.Achievements.AchievementsTypes
{
    public class ClickCountAchievement : AchievementsButton, ISavedProgress
    {
        [SerializeField] private ClickButton _clickButton;
        [SerializeField] private int _clickCount;

        public void Construct(ClickButton clickButton, int clickCount)
        {
            _clickButton = clickButton;
            _clickCount = clickCount;
        }

        public void Construct(ClickButton clickButton)
        {
            _clickButton = clickButton;
        }

        protected override void Start()
        {
            Button.onClick.AddListener(GetAwardPoints);

            base.Start();

            if (AchievementNumber > TaskValues.Length)
                Button.onClick.RemoveListener(GetAwardPoints);

            _clickButton.ButtonClicked += AddClick;
            CheckPoints();
            
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.Achievements[(int)Type].IsLocked = IsLocked;
            progress.Achievements[(int)Type].AchievementNumber = AchievementNumber;
            progress.Achievements[(int)Type].ClickCountAchievementProgress.ClickCount = _clickCount;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (!LoadProgressState.IsNewProgress)
            {
                IsLocked = progress.Achievements[(int)Type].IsLocked;
                AchievementNumber = progress.Achievements[(int)Type].AchievementNumber;
                _clickCount = progress.Achievements[(int)Type].ClickCountAchievementProgress.ClickCount;
            }
        }

        private void AddClick(long points)
        {
            _clickCount++;

            CheckPoints();
        }

        private void GetAwardPoints()
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
                if (_clickCount >= TaskValues[AchievementNumber - 1])
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

        private void CheckPoints()
        {
            if (AchievementNumber > TaskValues.Length)
                _clickButton.ButtonClicked -= AddClick;
            else if (_clickCount >= TaskValues[AchievementNumber - 1])
            {
                IsLocked = false;
                Button.interactable = true;
                TaskNameHUD.text = $"Заберите {TaskAwardPoints[AchievementNumber - 1]} поинтов";                
            }
        }
    }
}