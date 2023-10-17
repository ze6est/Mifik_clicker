using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour
{
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private LockedButton _lockedButton;
    [SerializeField] private AutoClick _autoClick;
    [SerializeField] private Points _points;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _pointsPerSecondHUD;
    [SerializeField] private TextMeshProUGUI _upgradeCountHUD;
    [SerializeField] private Image _icon;

    [SerializeField] private long _pointsPerSecond;
    [SerializeField] private long _upgradeCount;

    private long _currentPointsPerSecond;     

    public void Construct(MifiksName nameId, long pointsPerSecond, long upgradeCount, Image icon, LockedButton lockedButton, AutoClick autoClick, Points points)
    {
        _lockedButton = lockedButton;
        _autoClick = autoClick;
        _points = points;
        _name.text = Enum.GetName(typeof(MifiksName), nameId);        
        _pointsPerSecondHUD.text = $"+{pointsPerSecond}";
        _pointsPerSecond = pointsPerSecond;
        _currentPointsPerSecond = pointsPerSecond;
        _upgradeCountHUD.text = $"{upgradeCount}";
        _upgradeCount = upgradeCount;
        _icon = icon;        
    }

    private void OnValidate()
    {
        _upgradeButton = GetComponentInChildren<Button>();
    }

    private void Awake()
    {
        _lockedButton.CardUnlocked += OnCardUnlocked;
        _upgradeButton.onClick.AddListener(AddPointsPerSecond);
    }

    private void OnDestroy()
    {
        _lockedButton.CardUnlocked -= OnCardUnlocked;
    }

    private void OnCardUnlocked()
    {
        _autoClick.SetPointsPerSecond(_pointsPerSecond);
        _currentPointsPerSecond = _pointsPerSecond;
        _pointsPerSecondHUD.text = $"+{_pointsPerSecond}";
    }

private void AddPointsPerSecond()
    {
        if (_points.CurrentPoints >= _upgradeCount)
        {
            _pointsPerSecond *= 2;
            _pointsPerSecondHUD.text = $"+{_pointsPerSecond}";

            long addPoints = _pointsPerSecond - _currentPointsPerSecond;

            _autoClick.SetPointsPerSecond(addPoints);

            _currentPointsPerSecond = _pointsPerSecond;

            _points.RefreshPoints(-_upgradeCount);
        }        
    }
}