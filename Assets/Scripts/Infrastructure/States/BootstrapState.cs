namespace Assets.Scripts.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private GameStateMachine _gameStateMachine;

        public BootstrapState(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public void Enter()
        {
            _gameStateMachine.Enter<LoadProgressState>();
        }

        public void Exit()
        {
            
        }
    }
}