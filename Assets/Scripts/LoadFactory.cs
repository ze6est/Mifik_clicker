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
        private YandexAdv _yandexAdv;

        public List<ISavedProgress> ProgressSaveds { get; } = new List<ISavedProgress>();

        public LoadFactory(Canvas root, YandexAdv yandexAdv)
        {
            _root = root;
            _cards = new List<Card>();
            _yandexAdv = yandexAdv;
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
            ResetButton resetButton = centerPanelContentInstanse.GetComponentInChildren<ResetButton>();
            BonusButton bonusButton = centerPanelContentInstanse.GetComponentInChildren<BonusButton>();
            ClickButton clickButton = clickButtonInstanse.GetComponent<ClickButton>();
            BlocksContent blocksContent = mifiksContentInstanse.GetComponentInChildren<BlocksContent>();
            BlocksAchievementsContent blocksAchievementsContent = achievementsContentInstanse.GetComponentInChildren<BlocksAchievementsContent>();            

            InstantiateBloks(blocksContent.gameObject, progressService.Progress);
            InstantiateBlocksAchievement(blocksAchievementsContent.gameObject, progressService.Progress, clickButton, awardsPerClick);

            _points.Construct(clickButton);
            clickButton.Construct(awardsPerClick);            
            saveButton.Construct(progressService, _yandexAdv);
            resetButton.Construct(progressService);
            awardsPerClick.Construct(_yandexAdv);
            //bonusButton.Construct(yandexAdv);

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

        private void InstantiateBlocksAchievement(GameObject container, PlayerProgress progress, ClickButton clickButton, AwardsPerClick awardsPerClick)
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
                    case AchievementsType.MifCoinPerClick:
                        achievementGO = ConstructMifCoinPerClick(awardsPerClick, blockAchievement, achievement);
                        break;
                    case AchievementsType.OpenMifiks:
                        achievementGO = ConstructOpenMifiks(_cards, blockAchievement, achievement);
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

        private GameObject ConstructClickCount(ClickButton clickButton, BlockAchievement blockAchievement, AchievementsStaticData achievement)
        {
            ClickCountAchievement clickCountAchievement = Resources.Load<ClickCountAchievement>("Achievements/Prefabs/ClickCountAchievement");            

            if (LoadProgressState.IsNewProgress)
            {
                ConstructAchievementInNewProgress(achievement, clickCountAchievement);
                clickCountAchievement.Construct(clickButton, 0);
            }
            if (!LoadProgressState.IsNewProgress)
            {
                ConstructAchievementInOldProgress(achievement, clickCountAchievement);
                clickCountAchievement.Construct(clickButton);
            }

            return clickCountAchievement.gameObject;
        }
        
        private GameObject ConstructMifCoin(BlockAchievement blockAchievement, AchievementsStaticData achievement)
        {
            MifCoinAchievement mifCoinAchievement = Resources.Load<MifCoinAchievement>("Achievements/Prefabs/MifCoinAchievement");

            if (LoadProgressState.IsNewProgress)
            {
                ConstructAchievementInNewProgress(achievement, mifCoinAchievement);
                mifCoinAchievement.Construct(0);
            }
            if (!LoadProgressState.IsNewProgress)
            {
                ConstructAchievementInOldProgress(achievement, mifCoinAchievement);
            }

            return mifCoinAchievement.gameObject;
        }

        private GameObject ConstructMifCoinAutoClick(List<Card> cards, BlockAchievement blockAchievement, AchievementsStaticData achievement)
        {
            MifCoinAutoClickAchievement mifCoinAutoClickAchievement =
                Resources.Load<MifCoinAutoClickAchievement>("Achievements/Prefabs/MifCoinAutoClickAchievement");

            if (LoadProgressState.IsNewProgress)
            {
                ConstructAchievementInNewProgress(achievement, mifCoinAutoClickAchievement);
                mifCoinAutoClickAchievement.Construct(cards, 0);
            }
            if (!LoadProgressState.IsNewProgress)
            {
                ConstructAchievementInOldProgress(achievement, mifCoinAutoClickAchievement);
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
                ConstructAchievementInNewProgress(achievement, mifCoinClickAchievement);
                mifCoinClickAchievement.Construct(clickButton, 0);
            }
            if (!LoadProgressState.IsNewProgress)
            {
                ConstructAchievementInOldProgress(achievement, mifCoinClickAchievement);
                mifCoinClickAchievement.Construct(clickButton);
            }

            return mifCoinClickAchievement.gameObject;
        }

        private GameObject ConstructMifCoinPerClick(AwardsPerClick awardsPerClick, BlockAchievement blockAchievement, AchievementsStaticData achievement)
        {
            MifCoinPerClickAchievement mifCoinPerClickAchievement =
                Resources.Load<MifCoinPerClickAchievement>("Achievements/Prefabs/MifCoinPerClickAchievement");

            if (LoadProgressState.IsNewProgress)
            {
                ConstructAchievementInNewProgress(achievement, mifCoinPerClickAchievement);
                mifCoinPerClickAchievement.Construct(awardsPerClick, awardsPerClick.PointsPerClick);
            }
            if (!LoadProgressState.IsNewProgress)
            {
                ConstructAchievementInOldProgress(achievement, mifCoinPerClickAchievement);
                mifCoinPerClickAchievement.Construct(awardsPerClick);
            }

            return mifCoinPerClickAchievement.gameObject;
        }

        private GameObject ConstructOpenMifiks(List<Card> cards, BlockAchievement blockAchievement, AchievementsStaticData achievement)
        {
            OpenMifiksAchievement openMifiksAchievement =
                Resources.Load<OpenMifiksAchievement>("Achievements/Prefabs/OpenMifiksAchievement");

            if (LoadProgressState.IsNewProgress)
            {
                ConstructAchievementInNewProgress(achievement, openMifiksAchievement);
                openMifiksAchievement.Construct(cards, 0);
            }
            if (!LoadProgressState.IsNewProgress)
            {
                ConstructAchievementInOldProgress(achievement, openMifiksAchievement);
                openMifiksAchievement.Construct(cards);
            }

            return openMifiksAchievement.gameObject;
        }

        private void ConstructAchievementInNewProgress<T>(AchievementsStaticData achievement, AchievementsButton<T> achievementsButton)
        {
            achievementsButton.Construct(_points,
                                achievement.AchievementsType,
                                achievement.Icon,
                                achievement.TaskName,
                                achievement.TaskAwardPoints,
                                achievement.TaskCount,
                                achievement.TaskValues,
                                achievement.IsLocked,
                                1);
        }


        private void ConstructAchievementInOldProgress<T>(AchievementsStaticData achievement, AchievementsButton<T> achievementsButton)
        {
            achievementsButton.Construct(_points,
                                achievement.AchievementsType,
                                achievement.TaskName,
                                achievement.TaskCount,
                                achievement.TaskValues,
                                achievement.TaskAwardPoints,
                                achievement.Icon);
        }
    }
}