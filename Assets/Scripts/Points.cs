using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Points : MonoBehaviour
{
    [SerializeField] private ClickButton _clickButton;
    [SerializeField] private AutoClick _autoClick;
    [SerializeField] private TextMeshProUGUI _pointsHud;

    private long _points;

    private void OnValidate()
    {
        _clickButton = FindObjectOfType<ClickButton>();
        _pointsHud = gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void Awake()
    {
        _points = 1L;
        _clickButton.ButtonClicked += RefreshPoints;
        _autoClick.SecondsPassed += RefreshPoints;
    }

    private void Start() =>
        RefreshText();

    private void OnDestroy()
    {
        _clickButton.ButtonClicked -= RefreshPoints;
        _autoClick.SecondsPassed -= RefreshPoints;
    }

    private void RefreshPoints(long count)
    {
        _points += count;
        RefreshText();
    }

    private void RefreshText() =>
        _pointsHud.text = $"{_points}";
}
