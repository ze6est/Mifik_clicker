using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Points : MonoBehaviour, ISavedProgress
{
    [SerializeField] private ClickButton _clickButton;        
    [SerializeField] private TextMeshProUGUI _pointsHud;

    private long _points;

    public long CurrentPoints => _points;

    private void OnValidate()
    {
        _clickButton = FindObjectOfType<ClickButton>();
        _pointsHud = gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void Awake()
    {
        _points = 1L;
        _clickButton.ButtonClicked += RefreshPoints;        
    }

    private void Start() =>
        RefreshText();

    private void OnDestroy()
    {
        _clickButton.ButtonClicked -= RefreshPoints;        
    }

    public void UpdateProgress(PlayerProgress progress)
    {
        progress.Points = _points;
    }

    public void LoadProgress(PlayerProgress progress)
    {
        
    }

    public void RefreshPoints(long count)
    {
        _points += count;
        RefreshText();
    }

    private void RefreshText() =>
        _pointsHud.text = $"{_points}";    
}
