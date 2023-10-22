using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Card : MonoBehaviour
{
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

    private Coroutine _getPoints;
    private int _upgradeCountAutoClickCurrent;
    private int _upgradeCountTimeCurrent;

    public void Construct(MifiksName nameId, long pointsPerAutoClick, long timeAutoClick, long upgradeAutoClickCost, int upgradeCountAutoClick,
        long upgradeTimeCost, int upgradeCountTime, Image icon, LockedButton lockedButton, Points points)
    {
        _lockedButton = lockedButton;        
        _points = points;

        _name.text = Enum.GetName(typeof(MifiksName), nameId);   
        
        _pointsPerAutoClickHUD.text = $"+{pointsPerAutoClick}";
        _pointsPerAutoClick = pointsPerAutoClick;        

        _timeAutoClickHUD.text = $"{timeAutoClick} сек";
        _timeAutoClick = timeAutoClick;

        _upgradeAutoClickCostHUD.text = $"{upgradeAutoClickCost}";
        _upgradeAutoClickCost = upgradeAutoClickCost;
        _upgradeCountAutoClick = upgradeCountAutoClick;

        _upgradeTimeCostHUD.text = $"{upgradeTimeCost}";
        _upgradeTimeCost = upgradeTimeCost;
        _upgradeCountTime = upgradeCountTime;

        _icon = icon;        
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

    private void OnCardUnlocked()
    {
        _pointsPerAutoClickHUD.text = $"+{_pointsPerAutoClick}";

        _timeAutoClickImage.fillAmount = 0 ;

        _getPoints = StartCoroutine(GetPoints());
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