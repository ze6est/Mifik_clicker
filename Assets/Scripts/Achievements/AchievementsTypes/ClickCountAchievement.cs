using Assets.Scripts.Infrastructure.States;
using UnityEngine;

namespace Assets.Scripts.Achievements.AchievementsTypes
{
    public class ClickCountAchievement : AchievementsButton<int>, ISavedProgress
    {
        [SerializeField] private ClickButton _clickButton;

        protected override int Parametr { get; set; }

        public void Construct(ClickButton clickButton, int clickCount)
        {
            _clickButton = clickButton;
            Parametr = clickCount;
        }

        public void Construct(ClickButton clickButton)
        {
            _clickButton = clickButton;
        }

        protected override void Start()
        {
            base.Start();            

            _clickButton.ButtonClicked += AddClick;
            CheckPoints();            
        }

        public override void UpdateProgress(PlayerProgress progress)
        {
            base.UpdateProgress(progress);

            progress.Achievements[(int)Type].ClickCountAchievementProgress.ClickCount = Parametr;
        }

        public override void LoadProgress(PlayerProgress progress)
        {
            base.LoadProgress(progress);

            if (!LoadProgressState.IsNewProgress)
            {                
                Parametr = progress.Achievements[(int)Type].ClickCountAchievementProgress.ClickCount;
            }
        }

        private void AddClick(long points)
        {
            Parametr++;

            CheckPoints();
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

        private void CheckPoints()
        {
            if (AchievementNumber > TaskValues.Length)
                _clickButton.ButtonClicked -= AddClick;
            else if (Parametr >= TaskValues[AchievementNumber - 1])
            {
                IsLocked = false;
                Button.interactable = true;
                TaskNameHUD.text = $"Заберите {TaskAwardPoints[AchievementNumber - 1]} поинтов";                
            }
        }
    }
}