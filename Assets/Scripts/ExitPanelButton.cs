using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ExitPanelButton : MonoBehaviour
{
    [SerializeField] private Button _exitButton;
    [SerializeField] private GameObject _panel;

    public event Action PanelClosed;

    private void OnValidate() => 
        _exitButton = gameObject.GetComponent<Button>();

    private void Start() =>
        _exitButton.onClick.AddListener(CloseSettingsPanel);

    private void OnDestroy() => 
        _exitButton.onClick.RemoveListener(CloseSettingsPanel);

    private void CloseSettingsPanel()
    {
        PanelClosed?.Invoke();
        _panel.SetActive(false);
    }
}