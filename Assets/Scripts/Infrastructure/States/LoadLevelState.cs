namespace Assets.Scripts.Infrastructure.States
{
    public class LoadLevelState : IState
    {
        private GameStateMachine _gameStateMachine;
        private LoadFactory _loadFactory;
        private ProgressService _progressService;

        public LoadLevelState(GameStateMachine gameStateMachine, LoadFactory loadFactory, ProgressService progressService)
        {
            _gameStateMachine = gameStateMachine;
            _loadFactory = loadFactory;
            _progressService = progressService;
        }

        public void Enter()
        {            
            _loadFactory.LoadGame(_progressService);
            InformProgressSaveds();
        }

        public void Exit()
        {
            
        }

        private void InformProgressSaveds()
        {
            foreach (ISavedProgress savedProgress in _loadFactory.ProgressSaveds)
            {
                savedProgress.LoadProgress(_progressService.Progress);
            }
        }
    }
}