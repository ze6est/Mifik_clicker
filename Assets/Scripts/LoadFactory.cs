using System.Collections.Generic;
using Assets.Scripts.Achievements;
using Assets.Scripts.Achievements.AchievementsTypes;
using Assets.Scripts.Infrastructure.States;
using UnityEngine;

namespace Assets.Scripts
{
    public class LoadFactory
    {
        private StaticDataService _staticDataService = new StaticDataService();
        private Canvas _root;
        private Points _points;
        private List<Card> _cards;

        public List<ISavedProgress> ProgressSaveds { get; } = new List<ISavedProgress>();

        public LoadFactory(Canvas root)
        {
            _root = root;
            _cards = new List<Card>();
        }

        public void LoadGame(ProgressService progressService)
        {
            GameObject centerPanelContent = Resources.Load<GameObject>("HUD/CenterPanelContent");
            GameObject achievementsContent = Resources.Load<GameObject>("HUD/AchievementsContent");
            GameObject mifiksContent = Resources.Load<GameObject>("HUD/MifiksContent");
            GameObject clickButtonObj = Resources.Load<GameObject>("HUD/ClickButton");

            GameObject centerPanel = GameObject.FindGameObjectWithTag("CenterPanel");
            GameObject achievements = GameObject.FindGameObjectWithTag("Achievements");
            GameObject mifiks = GameObject.FindGameObjectWithTag("Mifiks");
            GameObject clickPanel = GameObject.FindGameObjectWithTag("ClickPanel");

            GameObject centerPanelContentInstanse = Object.Instantiate(centerPanelContent, centerPanel.transform);
            GameObject achievementsContentInstanse = Object.Instantiate(achievementsContent, achievements.transform);
            GameObject mifiksContentInstanse = Object.Instantiate(mifiksContent, mifiks.transform);
            GameObject clickButtonInstanse = Object.Instantiate(clickButtonObj, clickPanel.transform);

            _points = centerPanelContentInstanse.GetComponentInChildren<Points>();
            AwardsPerClick awardsPerClick = centerPanelContentInstanse.GetComponentInChildren<AwardsPerClick>();
            SaveButton saveButton = centerPanelContentInstanse.GetComponentInChildren<SaveButton>();
            ClickButton clickButton = clickButtonInstanse.GetComponent<ClickButton>();
            BlocksContent blocksContent = mifiksContentInstanse.GetComponentInChildren<BlocksContent>();
            BlocksAchievementsContent blocksAchievementsContent = achievementsContentInstanse.GetComponentInChildren<BlocksAchievementsContent>();

            InstantiateBloks(blocksContent.gameObject, progressService.Progress);
            InstantiateBlocksAchievement(blocksAchievementsContent.gameObject, progressService.Progress, clickButton);

            _points.Construct(clickButton);
            clickButton.Construct(awardsPerClick);
            saveButton.Construct(progressService);

            RegisterProgressSaveds(centerPanelContentInstanse);
            RegisterProgressSaveds(achievementsContentInstanse);
            RegisterProgressSaveds(mifiksContentInstanse);
            RegisterProgressSaveds(clickButtonInstanse);
        }

        private void InstantiateBloks(GameObject container, PlayerProgress progress)
        {
            Block block = Resources.Load<Block>("Block");

            _staticDataService.LoadMifiks();
            List<MifiksStaticData> mifiks = _staticDataService.GetMifiks();

            foreach (MifiksStaticData mifik in mifiks)
            {
                LockedButton lockedButton = block.GetComponentInChildren<LockedButton>();
                Card card = block.GetComponentInChildren<Card>();

                if (LoadProgressState.IsNewProgress)
                {
                    lockedButton.Construct(_points,
                        mifik.CostUnlocked,
                        mifik.IsLocked);

                    card.Construct(mifik.NameId,
                    mifik.PointsPerAutoClick,
                    mifik.TimeSecondsAutoClick,
                    mifik.UpgradeAutoClickCost,
                    mifik.UpgradeCountAutoClick,
                    mifik.UpgradeTimeCost,
                    mifik.UpgradeCountTime,
                    mifik.Icon,
                    lockedButton,
                    _points);
                }
                else
                {
                    lockedButton.Construct(_points,
                        mifik.CostUnlocked,
                        progress.Cards[(int)mifik.NameId].IsLocked);

                    card.Construct(mifik.NameId,
                        mifik.Icon,
                        _points,
                        mifik.UpgradeCountAutoClick,
                        mifik.UpgradeCountTime);
                }

                Block blockInstanse = Object.Instantiate(block, container.transform);
                _cards.Add(blockInstanse.GetComponentInChildren<Card>());
            }
        }

        private void InstantiateBlocksAchievement(GameObject container, PlayerProgress progress, ClickButton clickButton)
        {
            BlockAchievement blockAchievement = Resources.Load<BlockAchievement>("Achievements/Prefabs/BlockAchievement");
            GameObject achievementGO;

            _staticDataService.LoadAchievements();
            List<AchievementsStaticData> achievements = _staticDataService.GetAchievements();

            foreach (AchievementsStaticData achievement in achievements)
            {
                switch (achievement.AchievementsType)
                {
                    case AchievementsType.ClickCount:
                        achievementGO = ConstructClickCount(clickButton, blockAchievement, achievement);
                        break;
                    case AchievementsType.MifCoinClick:
                        achievementGO = ConstructMifCoinClick(clickButton, blockAchievement, achievement);
                        break;
                    case AchievementsType.MifCoinAutoClick:
                        achievementGO = ConstructMifCoinAutoClick(_cards, blockAchievement, achievement);
                        break;
                    case AchievementsType.MifCoin:
                        achievementGO = ConstructMifCoin(blockAchievement, achievement);
                        break;
                    default:
                        achievementGO = null;
                        break;
                }

                BlockAchievement blockAchievementGO = Object.Instantiate(blockAchievement, container.transform);
                Object.Instantiate(achievementGO, blockAchievementGO.transform);
            }           
        }

        private void RegisterProgressSaveds(GameObject gameObject)
        {
            foreach (ISavedProgress savedProgress in gameObject.GetComponentsInChildren<ISavedProgress>())            
                Register(savedProgress);            
        }

        private void Register(ISavedProgress savedProgress) => 
            ProgressSaveds.Add(savedProgress);

        private void Cleanup() => 
            ProgressSaveds.Clear();

        private GameObject ConstructClickCount(ClickButton clickButton, BlockAchievement blockAchievement, AchievementsStaticData achievement)
        {
            ClickCountAchievement clickCountAchievement = Resources.Load<ClickCountAchievement>("Achievements/Prefabs/ClickCountAchievement");            

            if (LoadProgressState.IsNewProgress)
            {
                clickCountAchievement.Construct(_points,
                    achievement.AchievementsType,
                    achievement.Icon,
                    achievement.TaskName,
                    achievement.TaskAwardPoints,
                    achievement.TaskCount,
                    achievement.TaskValues,
                    achievement.IsLocked,
                    1);

                clickCountAchievement.Construct(clickButton, 0);
            }
            if (!LoadProgressState.IsNewProgress)
            {
                clickCountAchievement.Construct(_points, 
                    achievement.AchievementsType,
                    achievement.TaskName,
                    achievement.TaskCount,
                    achievement.TaskValues,
                    achievement.TaskAwardPoints,
                    achievement.Icon);
                clickCountAchievement.Construct(clickButton);
            }

            return clickCountAchievement.gameObject;
        }

        private GameObject ConstructMifCoin(BlockAchievement blockAchievement, AchievementsStaticData achievement)
        {
            MifCoinAchievement mifCoinAchievement = Resources.Load<MifCoinAchievement>("Achievements/Prefabs/MifCoinAchievement");

            if (LoadProgressState.IsNewProgress)
            {
                mifCoinAchievement.Construct(_points,
                    achievement.AchievementsType,
                    achievement.Icon,
                    achievement.TaskName,
                    achievement.TaskAwardPoints,
                    achievement.TaskCount,
                    achievement.TaskValues,
                    achievement.IsLocked,
                    1);

                mifCoinAchievement.Construct(0);
            }
            if (!LoadProgressState.IsNewProgress)
            {
                mifCoinAchievement.Construct(_points,
                    achievement.AchievementsType,
                    achievement.TaskName,
                    achievement.TaskCount,
                    achievement.TaskValues,
                    achievement.TaskAwardPoints,
                    achievement.Icon);                
            }

            return mifCoinAchievement.gameObject;
        }

        private GameObject ConstructMifCoinAutoClick(List<Card> cards, BlockAchievement blockAchievement, AchievementsStaticData achievement)
        {
            MifCoinAutoClickAchievement mifCoinAutoClickAchievement =
                Resources.Load<MifCoinAutoClickAchievement>("Achievements/Prefabs/MifCoinAutoClickAchievement");

            if (LoadProgressState.IsNewProgress)
            {
                mifCoinAutoClickAchievement.Construct(_points,
                    achievement.AchievementsType,
                    achievement.Icon,
                    achievement.TaskName,
                    achievement.TaskAwardPoints,
                    achievement.TaskCount,
                    achievement.TaskValues,
                    achievement.IsLocked,
                    1);

                mifCoinAutoClickAchievement.Construct(cards, 0);
            }
            if (!LoadProgressState.IsNewProgress)
            {
                mifCoinAutoClickAchievement.Construct(_points,
                    achievement.AchievementsType,
                    achievement.TaskName,
                    achievement.TaskCount,
                    achievement.TaskValues,
                    achievement.TaskAwardPoints,
                    achievement.Icon);
                mifCoinAutoClickAchievement.Construct(cards);
            }

            return mifCoinAutoClickAchievement.gameObject;
        }

        private GameObject ConstructMifCoinClick(ClickButton clickButton, BlockAchievement blockAchievement, AchievementsStaticData achievement)
        {
            MifCoinClickAchievement mifCoinClickAchievement =
                Resources.Load<MifCoinClickAchievement>("Achievements/Prefabs/MifCoinClickAchievement");

            if (LoadProgressState.IsNewProgress)
            {
                mifCoinClickAchievement.Construct(_points,
                    achievement.AchievementsType,
                    achievement.Icon,
                    achievement.TaskName,
                    achievement.TaskAwardPoints,
                    achievement.TaskCount,
                    achievement.TaskValues,
                    achievement.IsLocked,
                    1);

                mifCoinClickAchievement.Construct(clickButton, 0);
            }
            if (!LoadProgressState.IsNewProgress)
            {
                mifCoinClickAchievement.Construct(_points,
                    achievement.AchievementsType,
                    achievement.TaskName,
                    achievement.TaskCount,
                    achievement.TaskValues,
                    achievement.TaskAwardPoints,
                    achievement.Icon);
                mifCoinClickAchievement.Construct(clickButton);
            }

            return mifCoinClickAchievement.gameObject;
        }
    }
}