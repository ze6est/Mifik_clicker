using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class LoadFactory
    {
        private StaticDataService _staticDataService = new StaticDataService();
        private Canvas _root;
        private Points _points;

        public List<ISavedProgress> ProgressSaveds { get; } = new List<ISavedProgress>();

        public LoadFactory(Canvas root)
        {
            _root = root;
        }

        public void LoadGame()
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
            ClickButton clickButton = clickButtonInstanse.GetComponent<ClickButton>();
            BlocksContent blocksContent = mifiksContentInstanse.GetComponentInChildren<BlocksContent>();

            InstantiateBloks(blocksContent.gameObject);

            _points.Construct(clickButton);
            clickButton.Construct(awardsPerClick);

            RegisterProgressSaveds(centerPanelContentInstanse);
            RegisterProgressSaveds(achievementsContentInstanse);
            RegisterProgressSaveds(mifiksContentInstanse);
            RegisterProgressSaveds(clickButtonInstanse);
        }

        private void InstantiateBloks(GameObject container)
        {
            Block block = Resources.Load<Block>("Block");

            _staticDataService.LoadMifiks();
            List<MifiksStaticData> mifiks = _staticDataService.GetMifiks();

            foreach (MifiksStaticData mifik in mifiks)
            {
                LockedButton lockedButton = block.GetComponentInChildren<LockedButton>();
                lockedButton.Construct(_points, mifik.CostUnlocked, mifik.IsLocked);

                Card card = block.GetComponentInChildren<Card>();
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

                Object.Instantiate(block, container.transform);
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
    }
}