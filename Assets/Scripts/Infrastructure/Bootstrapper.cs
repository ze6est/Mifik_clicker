﻿using Assets.Scripts.Infrastructure.States;
using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private Canvas _root;
        [SerializeField] private YandexAdv _yandexAdv;

        private Game _game;

        private void Awake()
        {
            _game = new Game(_root, _yandexAdv);
            _game.StateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);
        }
    }
}