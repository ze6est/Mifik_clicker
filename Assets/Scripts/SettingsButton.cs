using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SettingsButton : MonoBehaviour
{
    [SerializeField] private SettingsPanel _settingsPanel;
    [SerializeField] private Button _settingsButton;

    private void OnValidate() =>
        _settingsButton = gameObject.GetComponent<Button>();

    private void Awake()
    {
        _settingsPanel.gameObject.SetActive(false);
        _settingsButton.onClick.AddListener(OpenSettingsPanel);        
    }

    private void OnDestroy() => 
        _settingsButton.onClick.RemoveListener(OpenSettingsPanel);

    private void OpenSettingsPanel() => 
        _settingsPanel.gameObject.SetActive(true);
}