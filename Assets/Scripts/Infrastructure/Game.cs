using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public class Game
    {
        public GameStateMachine StateMachine;

        public Game() 
        {
            StateMachine = new GameStateMachine();
        }
    }
}