using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Points : MonoBehaviour
{
    [SerializeField] private ClickButton _clickButton;
    [SerializeField] private TextMeshProUGUI _pointsHud;

    private int _points;

    private void OnValidate()
    {
        _clickButton = FindObjectOfType<ClickButton>();
        _pointsHud = gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void Awake()
    {
        _points = 1;
        _clickButton.ButtonClicked += RefreshPoints;
    }

    private void Start() =>
        RefreshText();

    private void OnDestroy() => 
        _clickButton.ButtonClicked -= RefreshPoints;

    private void RefreshPoints(int count)
    {
        _points += count;
        RefreshText();
    }

    private void RefreshText() =>
        _pointsHud.text = $"{_points}";
}
