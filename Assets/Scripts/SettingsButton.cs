using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(AudioSource))]
public class SettingsButton : MonoBehaviour
{
    [SerializeField] private SettingsPanel _settingsPanel;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private ExitPanelButton _exitPanelButton;
    [SerializeField] private AudioSource _audioSource;

    private void OnValidate()
    {
        _settingsButton = gameObject.GetComponent<Button>();
        _audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void Awake()
    {        
        _exitPanelButton.PanelClosed += OnPanelClosed;
        _settingsPanel.gameObject.SetActive(false);
        _settingsButton.onClick.AddListener(OpenSettingsPanel);        
    }

    private void OnDestroy() => 
        _settingsButton.onClick.RemoveListener(OpenSettingsPanel);

    private void OnPanelClosed() => 
        _audioSource.Play();

    private void OpenSettingsPanel()
    {
        _audioSource.Play();
        _settingsPanel.gameObject.SetActive(true);
    }
}