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

    public event Action<long> PointReceived;

    public void Construct(ClickButton clickButton)
    {
        _clickButton = clickButton;
    }

    private void OnValidate()
    {        
        _pointsHud = gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void Awake()
    {
        _points = 0;             
    }

    private void Start()
    {
        _clickButton.ButtonClicked += RefreshPoints;
        RefreshText();
    }

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
        _points = progress.Points;
    }

    public void RefreshPoints(long count)
    {
        _points += count;
        PointReceived?.Invoke(count);
        RefreshText();
    }

    private void RefreshText() =>
        _pointsHud.text = $"{_points}";    
}