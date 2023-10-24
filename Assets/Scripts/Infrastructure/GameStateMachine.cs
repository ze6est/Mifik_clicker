using System;
using System.Collections.Generic;
using Assets.Scripts.Infrastructure.States;

namespace Assets.Scripts.Infrastructure
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IState> _states;

        private IState _activeState;

        public GameStateMachine(LoadFactory loadFactory, ProgressService progressService)
        {
            _states = new Dictionary<Type, IState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(this),
                [typeof(LoadLevelState)] = new LoadLevelState(this, loadFactory, progressService),
                [typeof(LoadProgressState)] = new LoadProgressState(this, progressService)
            };
        }

        public void Enter<TState>() where TState : IState
        {
            _activeState?.Exit();

            IState state = _states[typeof(TState)];
            _activeState = state;

            state.Enter();
        }
    }
}