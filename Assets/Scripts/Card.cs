using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour
{
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private Button _upgradeTimeButton;
    [SerializeField] private LockedButton _lockedButton;
    [SerializeField] private AutoClick _autoClick;
    [SerializeField] private Points _points;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _pointsPerAutoClickHUD;
    [SerializeField] private TextMeshProUGUI _timeAutoClickHUD;
    [SerializeField] private TextMeshProUGUI _upgradeCountHUD;
    [SerializeField] private TextMeshProUGUI _upgradeTimeCountHUD;
    [SerializeField] private Image _icon;

    [SerializeField] private long _pointsPerAutoClick;
    [SerializeField] private long _timeAutoClick;
    [SerializeField] private long _upgradeCount;
    [SerializeField] private long _upgradeTimeCount;

    private long _currentPointsPerAutoClick;     

    public void Construct(MifiksName nameId, long pointsPerAutoClick, long timeAutoClick, long upgradeCount, long upgradeTimeCount, Image icon, LockedButton lockedButton, AutoClick autoClick, Points points)
    {
        _lockedButton = lockedButton;
        _autoClick = autoClick;
        _points = points;

        _name.text = Enum.GetName(typeof(MifiksName), nameId);   
        
        _pointsPerAutoClickHUD.text = $"+{pointsPerAutoClick}";
        _pointsPerAutoClick = pointsPerAutoClick;
        _currentPointsPerAutoClick = pointsPerAutoClick;

        _timeAutoClickHUD.text = $"{timeAutoClick} сек";
        _timeAutoClick = timeAutoClick;

        _upgradeCountHUD.text = $"{upgradeCount}";
        _upgradeCount = upgradeCount;

        _upgradeTimeCountHUD.text = $"{upgradeTimeCount}";
        _upgradeTimeCount = upgradeTimeCount;

        _icon = icon;        
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
        _autoClick.SetPointsPerAutoClick(_pointsPerAutoClick);
        _currentPointsPerAutoClick = _pointsPerAutoClick;
        _pointsPerAutoClickHUD.text = $"+{_pointsPerAutoClick}";
    }

private void AddPointsPerSecond()
    {
        if (_points.CurrentPoints >= _upgradeCount)
        {
            _pointsPerAutoClick *= 2;
            _pointsPerAutoClickHUD.text = $"+{_pointsPerAutoClick}";
            
            long addPoints = _pointsPerAutoClick - _currentPointsPerAutoClick;

            _autoClick.SetPointsPerAutoClick(addPoints);

            _currentPointsPerAutoClick = _pointsPerAutoClick;

            _points.RefreshPoints(-_upgradeCount);
        }        
    }
}