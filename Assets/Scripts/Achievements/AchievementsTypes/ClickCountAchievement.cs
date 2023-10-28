using System;
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

        protected override void Awake()
        {
            base.Awake();

            _clickButton.ButtonClicked += AddClick;
            Button.onClick.AddListener(CheckPoints);
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            
        }

        public void LoadProgress(PlayerProgress progress)
        {
            
        }

        private void AddClick(long points)
        {
            _clickCount++;

            if(_clickCount >= TaskValues[TaskValues.Length - 1])
            {
                _clickButton.ButtonClicked -= AddClick;
                Button.onClick.RemoveListener(CheckPoints);
            }


            if (_clickCount >= TaskValues[AchievementNumber])            
                Button.interactable = true;
        }

        private void CheckPoints()
        {
            if (_clickCount >= TaskValues[AchievementNumber])
                Button.interactable = true;
        }
    }
}