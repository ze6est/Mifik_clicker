using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public class Game
    {
        public GameStateMachine StateMachine;

        private LoadFactory _loadFactory;
        private ProgressService _progressService;

        public Game(Canvas root, YandexAdv yandexAdv) 
        {
            _loadFactory = new LoadFactory(root, yandexAdv);
            _progressService = new ProgressService(_loadFactory);
            StateMachine = new GameStateMachine(_loadFactory, _progressService);
        }
    }
}