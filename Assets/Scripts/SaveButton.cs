using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SaveButton : MonoBehaviour
{
    [SerializeField] private Button _saveButton;
    [SerializeField] private YandexAdv _yandexAdv;

    private ProgressService _progressService;
    private Coroutine _saveCoroutine;

    private void OnValidate() => 
        _saveButton = gameObject.GetComponent<Button>();

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
        _saveButton.onClick.RemoveListener(SaveProgress);
    }

    private void SaveProgress()
    {
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