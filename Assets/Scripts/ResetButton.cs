using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ResetButton : MonoBehaviour
{
    [SerializeField] private Button _resetButton;
    [SerializeField] private ProgressService _progressService;

    private void OnValidate() =>
        _resetButton = gameObject.GetComponent<Button>();

    public void Construct(ProgressService progressService) =>
        _progressService = progressService;

    private void Start() =>
        _resetButton.onClick.AddListener(ResetGame);

    private void ResetGame()
    {
        _resetButton.onClick.RemoveListener(ResetGame);

        _progressService.ClearProgress();
        SceneManager.LoadScene("Game");
    }
}