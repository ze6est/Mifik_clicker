using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Assets.Scripts.Infrastructure.States;

public class Card : MonoBehaviour, ISavedProgress
{    
    [Header("Id")]
    [SerializeField] MifiksName _nameId;
    [Header("Butons")]
    [SerializeField] private Button _upgradePointsPerClickButton;
    [SerializeField] private Button _upgradeTimeButton;
    [SerializeField] private LockedButton _lockedButton;
    [Header("Points")]
    [SerializeField] private Points _points;
    [Header("TMP")]
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _pointsPerAutoClickHUD;
    [SerializeField] private TextMeshProUGUI _timeAutoClickHUD;
    [SerializeField] private TextMeshProUGUI _upgradeAutoClickCostHUD;
    [SerializeField] private TextMeshProUGUI _upgradeTimeCostHUD;
    [Header("Images")]
    [SerializeField] private Image _icon;
    [SerializeField] private Image _timeAutoClickImage;
    [Header("AutoClick")]
    [SerializeField] private long _pointsPerAutoClick;
    [SerializeField] private long _upgradeAutoClickCost;
    [SerializeField] private int _upgradeCountAutoClick;
    [Header("Time")]
    [SerializeField] private long _timeAutoClick;
    [SerializeField] private long _upgradeTimeCost;
    [SerializeField] private int _upgradeCountTime;
    [Header("Locked")]
    [SerializeField] private bool _isLocked;

    private Coroutine _getPoints;
    private int _upgradeCountAutoClickCurrent;
    private int _upgradeCountTimeCurrent;

    public void Construct(MifiksName nameId, long pointsPerAutoClick, long timeAutoClick, long upgradeAutoClickCost, int upgradeCountAutoClick,
        long upgradeTimeCost, int upgradeCountTime, Image icon, LockedButton lockedButton, Points points)
    {
        _lockedButton = lockedButton;        
        _points = points;

        _nameId = nameId;
        _name.text = Enum.GetName(typeof(MifiksName), nameId);        
        
        _pointsPerAutoClick = pointsPerAutoClick;
        
        _timeAutoClick = timeAutoClick;
        
        _upgradeAutoClickCost = upgradeAutoClickCost;
        _upgradeCountAutoClick = upgradeCountAutoClick;
        
        _upgradeTimeCost = upgradeTimeCost;
        _upgradeCountTime = upgradeCountTime;

        _icon = icon;

        _isLocked = true;

        RefreshText();
    }    

    private void Awake()
    {
        _lockedButton.CardUnlocked += OnCardUnlocked;
        _upgradePointsPerClickButton.onClick.AddListener(UpgradeAutoClick);
        _upgradeTimeButton.onClick.AddListener(UpgradeTime);
    }

    private void OnDestroy()
    {
        _lockedButton.CardUnlocked -= OnCardUnlocked;
        _upgradePointsPerClickButton.onClick.RemoveListener(UpgradeAutoClick);
        _upgradeTimeButton.onClick.RemoveListener(UpgradeTime);

        if (_getPoints != null)
            StopCoroutine(_getPoints);
    }

    public void UpdateProgress(PlayerProgress progress)
    {
        Debug.Log($"{(int)_nameId}");
        Debug.Log($"{progress.Cards[(int)_nameId].PointsPerAutoClick}");

        progress.Cards[(int)_nameId].PointsPerAutoClick = _pointsPerAutoClick;
        progress.Cards[(int)_nameId].UpgradeAutoClickCost = _upgradeAutoClickCost;
        progress.Cards[(int)_nameId].UpgradeCountAutoClickCurrent = _upgradeCountAutoClickCurrent;

        progress.Cards[(int)_nameId].TimeSecondsAutoClick = _timeAutoClick;
        progress.Cards[(int)_nameId].UpgradeTimeCost = _upgradeTimeCost;
        progress.Cards[(int)_nameId].UpgradeCountTimeCurrent = _upgradeCountTimeCurrent;

        progress.Cards[(int)_nameId].IsLocked = _isLocked;
    }

    public void LoadProgress(PlayerProgress progress)
    {
        if (!LoadProgressState.IsNewProgress)
        {
            _pointsPerAutoClick = progress.Cards[(int)_nameId].PointsPerAutoClick;
            _upgradeAutoClickCost = progress.Cards[(int)_nameId].UpgradeAutoClickCost;
            _upgradeCountAutoClickCurrent = progress.Cards[(int)_nameId].UpgradeCountAutoClickCurrent;

            _timeAutoClick = progress.Cards[(int)_nameId].TimeSecondsAutoClick;
            _upgradeTimeCost = progress.Cards[(int)_nameId].UpgradeTimeCost;
            _upgradeCountTimeCurrent = progress.Cards[(int)_nameId].UpgradeCountTimeCurrent;

            _isLocked = progress.Cards[(int)_nameId].IsLocked;

            RefreshText();
        }        
    }

    private void RefreshText()
    {
        _pointsPerAutoClickHUD.text = $"+{_pointsPerAutoClick}";
        _timeAutoClickHUD.text = $"{_timeAutoClick} сек";
        _upgradeAutoClickCostHUD.text = $"{_upgradeAutoClickCost}";
        _upgradeTimeCostHUD.text = $"{_upgradeTimeCost}";

        if (_upgradeCountAutoClick <= _upgradeCountAutoClickCurrent)
            _upgradeAutoClickCostHUD.text = $"max";

        if (_upgradeCountTime <= _upgradeCountTimeCurrent)
            _upgradeTimeCostHUD.text = $"max";
    }

    private void OnCardUnlocked()
    {
        _pointsPerAutoClickHUD.text = $"+{_pointsPerAutoClick}";

        _timeAutoClickImage.fillAmount = 0 ;

        _getPoints = StartCoroutine(GetPoints());

        _isLocked = false;
    }

    private void UpgradeAutoClick()
    {
        if (_points.CurrentPoints >= _upgradeAutoClickCost && _upgradeCountAutoClick > _upgradeCountAutoClickCurrent)
        {
            _upgradeCountAutoClickCurrent += 1;

            _pointsPerAutoClick *= 2;
            _pointsPerAutoClickHUD.text = $"+{_pointsPerAutoClick}";            

            _points.RefreshPoints(-_upgradeAutoClickCost);

            _upgradeAutoClickCost *= 2;

            _upgradeAutoClickCostHUD.text = $"{_upgradeAutoClickCost}";

            if(_upgradeCountAutoClick <= _upgradeCountAutoClickCurrent)
                _upgradeAutoClickCostHUD.text = $"max";
        }        
    }

    private void UpgradeTime()
    {
        if(_points.CurrentPoints >= _upgradeTimeCost && _upgradeCountTime > _upgradeCountTimeCurrent)
        {
            _upgradeCountTimeCurrent += 1;

            _timeAutoClick -= 1;
            _timeAutoClickHUD.text = $"{_timeAutoClick} сек";

            _points.RefreshPoints(-_upgradeTimeCost);

            _upgradeTimeCost *= 2;

            _upgradeTimeCostHUD.text = $"{_upgradeTimeCost}";

            if(_upgradeCountTime <= _upgradeCountTimeCurrent)
                _upgradeTimeCostHUD.text = $"max";
        }
    }

    private IEnumerator GetPoints()
    {
        WaitForSeconds waitingTime = new WaitForSeconds(1f);

        float currentTime = 0;

        while (true)
        {
            yield return waitingTime;

            currentTime += 1f;

            _timeAutoClickImage.fillAmount = currentTime / _timeAutoClick;

            if (currentTime >= _timeAutoClick)
            {
                _points.RefreshPoints(_pointsPerAutoClick);
                currentTime = 0;
            }
        }
    }    
}