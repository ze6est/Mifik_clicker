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
    [SerializeField] private TextMeshProUGUI _upgradeCountHUD;
    [SerializeField] private TextMeshProUGUI _upgradeTimeCountHUD;
    [Header("Images")]
    [SerializeField] private Image _icon;
    [SerializeField] private Image _timeAutoClickImage;


    [SerializeField] private long _pointsPerAutoClick;
    [SerializeField] private long _timeAutoClick;
    [SerializeField] private long _upgradeCount;
    [SerializeField] private long _upgradeTimeCount;
    
    private Coroutine _getPoints;

    public void Construct(MifiksName nameId, long pointsPerAutoClick, long timeAutoClick, long upgradeCount, long upgradeTimeCount, Image icon, LockedButton lockedButton, Points points)
    {
        _lockedButton = lockedButton;        
        _points = points;

        _name.text = Enum.GetName(typeof(MifiksName), nameId);   
        
        _pointsPerAutoClickHUD.text = $"+{pointsPerAutoClick}";
        _pointsPerAutoClick = pointsPerAutoClick;        

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
        _upgradePointsPerClickButton.onClick.AddListener(AddPointsPerTime);
    }

    private void OnDestroy()
    {
        _lockedButton.CardUnlocked -= OnCardUnlocked;

        if(_getPoints != null)
            StopCoroutine(_getPoints);
    }

    private void OnCardUnlocked()
    {
        _pointsPerAutoClickHUD.text = $"+{_pointsPerAutoClick}";

        _timeAutoClickImage.fillAmount = 0 ;

        _getPoints = StartCoroutine(GetPoints());
    }

    private void AddPointsPerTime()
    {
        if (_points.CurrentPoints >= _upgradeCount)
        {
            _pointsPerAutoClick *= 2;
            _pointsPerAutoClickHUD.text = $"+{_pointsPerAutoClick}";            

            _points.RefreshPoints(-_upgradeCount);
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