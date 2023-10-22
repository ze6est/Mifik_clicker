using UnityEngine;

namespace Assets.Scripts
{
    public class LoadFactory
    {
        private GameObject _camera;
        private GameObject _HUD;

        public void LoadGame()
        {
            _camera = Resources.Load<GameObject>("Camera");

            Object.Instantiate(_camera);

            _HUD = Resources.Load<GameObject>("HUD");

            Object.Instantiate(_HUD);

            _HUD.GetComponent<Canvas>().worldCamera = _camera.GetComponent<Camera>();
        }
    }
}
