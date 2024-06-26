﻿using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Assets.Scripts.Infrastructure.States;

[RequireComponent(typeof(AudioSource))]
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
    [SerializeField] private Image _image;
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
    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _openCard;
    [SerializeField] private AudioClip _levelUp;
    [SerializeField] private AudioClip _error;
    [SerializeField] private Sprite _icon;

    private Coroutine _getPoints;
    private int _upgradeCountAutoClickCurrent;
    private int _upgradeCountTimeCurrent;

    public event Action<long> PointsReceived;
    public event Action CardUnlocked;

    private void OnValidate() => 
        _audioSource = gameObject.GetComponent<AudioSource>();

    public void Construct(MifiksName nameId, long pointsPerAutoClick, long timeAutoClick, long upgradeAutoClickCost, int upgradeCountAutoClick,
        long upgradeTimeCost, int upgradeCountTime, Sprite icon, LockedButton lockedButton, Points points)
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

    public void Construct(MifiksName nameId, Sprite icon, Points points, int upgradeCountAutoClick, int upgradeCountTime)
    {
        _nameId = nameId;
        _name.text = Enum.GetName(typeof(MifiksName), nameId);
        _points = points;
        _upgradeCountAutoClick = upgradeCountAutoClick;
        _upgradeCountTime = upgradeCountTime;

        _icon = icon;
    }

    private void Awake()
    {
        _lockedButton.CardUnlocked += OnCardUnlocked;
        _upgradePointsPerClickButton.onClick.AddListener(UpgradeAutoClick);
        _upgradeTimeButton.onClick.AddListener(UpgradeTime);

        _image.sprite = _icon;
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
        CardUnlocked?.Invoke();

        _audioSource.PlayOneShot(_openCard);

        _pointsPerAutoClickHUD.text = $"+{_pointsPerAutoClick}";

        _timeAutoClickImage.fillAmount = 0 ;

        _getPoints = StartCoroutine(GetPoints());

        _isLocked = false;
    }

    private void UpgradeAutoClick()
    {
        if (_points.CurrentPoints >= _upgradeAutoClickCost && _upgradeCountAutoClick > _upgradeCountAutoClickCurrent)
        {
            _audioSource.PlayOneShot(_levelUp);

            _upgradeCountAutoClickCurrent += 1;

            _pointsPerAutoClick = _pointsPerAutoClick + 1 + (long)(_pointsPerAutoClick * 0.5);
            _pointsPerAutoClickHUD.text = $"+{_pointsPerAutoClick}";            

            _points.RefreshPoints(-_upgradeAutoClickCost);

            _upgradeAutoClickCost = _upgradeAutoClickCost + 1 + (long)(_upgradeAutoClickCost * 0.75);

            _upgradeAutoClickCostHUD.text = $"{_upgradeAutoClickCost}";

            if(_upgradeCountAutoClick <= _upgradeCountAutoClickCurrent)
                _upgradeAutoClickCostHUD.text = $"max";
        }
        else
            _audioSource.PlayOneShot(_error);
    }

    private void UpgradeTime()
    {
        if(_points.CurrentPoints >= _upgradeTimeCost && _upgradeCountTime > _upgradeCountTimeCurrent)
        {
            _audioSource.PlayOneShot(_levelUp);

            _upgradeCountTimeCurrent += 1;

            _timeAutoClick -= 1;
            _timeAutoClickHUD.text = $"{_timeAutoClick} сек";

            _points.RefreshPoints(-_upgradeTimeCost);

            _upgradeTimeCost = _upgradeTimeCost + 1 + (long)(_upgradeTimeCost * 0.2);

            _upgradeTimeCostHUD.text = $"{_upgradeTimeCost}";

            if(_upgradeCountTime <= _upgradeCountTimeCurrent)
                _upgradeTimeCostHUD.text = $"max";
        }
        else
            _audioSource.PlayOneShot(_error);
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
                PointsReceived?.Invoke(_pointsPerAutoClick);
                currentTime = 0;
            }
        }
    }    
}