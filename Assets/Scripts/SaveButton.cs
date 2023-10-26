using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SaveButton : MonoBehaviour
{
    [SerializeField] private Button _saveButton;

    private ProgressService _progressService;

    private void OnValidate() => 
        _saveButton = gameObject.GetComponent<Button>();

    public void Construct(ProgressService progressService)
    {
        _progressService = progressService;
    }

    private void Start() => 
        _saveButton.onClick.AddListener(SaveProgress);

    private void OnDestroy() => 
        _saveButton.onClick.RemoveListener(SaveProgress);

    private void SaveProgress() => 
        _progressService.SaveProgress();
}