using System;

namespace Assets.Scripts.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private GameStateMachine _gameStateMachine;
        private ProgressService _progressService;

        public LoadProgressState(GameStateMachine gameStateMachine, ProgressService progressService)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();
            _gameStateMachine.Enter<LoadLevelState>();
        }

        public void Exit()
        {
            
        }

        private void LoadProgressOrInitNew()
        {
            _progressService.Progress = _progressService.LoadProgress() ?? GetNewProgress();
        }

        private PlayerProgress GetNewProgress() =>
            new PlayerProgress();
    }
}