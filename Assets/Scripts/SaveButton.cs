using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(AudioSource))]
public class SaveButton : MonoBehaviour
{
    [SerializeField] private Button _saveButton;
    [SerializeField] private YandexAdv _yandexAdv;
    [SerializeField] private AudioSource _audioSource;

    private ProgressService _progressService;
    private Coroutine _saveCoroutine;

    private void OnValidate()
    {
        _saveButton = gameObject.GetComponent<Button>();
        _audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void Construct(ProgressService progressService, YandexAdv yandexAdv)
    {
        _progressService = progressService;
        _yandexAdv = yandexAdv;
    }

    private void Start() => 
        _saveButton.onClick.AddListener(SaveProgress);

    private void OnDestroy()
    {
        _saveButton.onClick.RemoveListener(SaveProgress);        
    }

    private void SaveProgress()
    {
        _audioSource.Play();

        _saveCoroutine = StartCoroutine(_progressService.SaveProgress());

        _progressService.ProgressSaved += StopSaveProgress;

        _yandexAdv.ShowFullScreen();
    }

    private void StopSaveProgress()
    {
        StopCoroutine(_saveCoroutine);
        _progressService.ProgressSaved -= StopSaveProgress;
    }    
}