namespace Assets.Scripts.Infrastructure.States
{
    public class LoadLevelState : IState
    {
        private LoadFactory _loadFactory;
        private GameStateMachine _gameStateMachine;

        public LoadLevelState(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
            _loadFactory = new LoadFactory();
        }

        public void Enter()
        {            
            _loadFactory.LoadGame();            
        }

        public void Exit()
        {
            
        }
    }
}
