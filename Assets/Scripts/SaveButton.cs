using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SaveButton : MonoBehaviour
{
    [SerializeField] private Button _saveButton;

    private ProgressService _progressService;
    private Coroutine _saveCoroutine;

    private void OnValidate() => 
        _saveButton = gameObject.GetComponent<Button>();

    public void Construct(ProgressService progressService)
    {
        _progressService = progressService;
    }

    private void Start() => 
        _saveButton.onClick.AddListener(SaveProgress);

    private void OnDestroy()
    {
        _saveButton.onClick.RemoveListener(SaveProgress);
        _progressService.ProgressSaved -= StopSaveProgress;
    }

    private void SaveProgress()
    {
        _saveCoroutine = StartCoroutine(_progressService.SaveProgress());

        _progressService.ProgressSaved += StopSaveProgress;
    }

    private void StopSaveProgress()
    {
        StopCoroutine(_saveCoroutine);
    }    
}