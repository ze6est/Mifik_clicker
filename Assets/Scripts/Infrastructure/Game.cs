using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public class Game
    {
        public GameStateMachine StateMachine;

        private LoadFactory _loadFactory;
        private ProgressService _progressService;

        public Game(Canvas root) 
        {
            _loadFactory = new LoadFactory(root);
            _progressService = new ProgressService(_loadFactory);
            StateMachine = new GameStateMachine(_loadFactory, _progressService);
        }
    }
}