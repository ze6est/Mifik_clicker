using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(TextMeshProUGUI))]
public class AwardsPerClick : MonoBehaviour, ISavedProgress
{
    [SerializeField] private TextMeshProUGUI _pointsPerClickHud;
    [SerializeField] private long _pointsPerClick;
    [SerializeField] private YandexAdv _yandexAdv;

    private int _advCount = 0;

    public long PointsPerClick => _pointsPerClick;

    public event Action<long> PointsPerClickReceived;

    private void OnValidate() => 
        _pointsPerClickHud = gameObject.GetComponent<TextMeshProUGUI>();

    public void Construct(YandexAdv yandexAdv) => 
        _yandexAdv = yandexAdv;

    private void Awake() => 
        PointsPerClickReceived?.Invoke(_pointsPerClick);

    private void Start()
    {
        _yandexAdv.ShowRevardedAdvRewarded += AddPointsPerClickAdv;
        _yandexAdv.FullScreenAdvertisementOpened += AddPointsPerClickAdv;

        RefreshText();
    }

    private void OnDestroy()
    {
        _yandexAdv.ShowRevardedAdvRewarded -= AddPointsPerClickAdv;
        _yandexAdv.FullScreenAdvertisementOpened -= AddPointsPerClickAdv;
    }

    public void UpdateProgress(PlayerProgress progress)
    {
        progress.PointsPerClick = _pointsPerClick;
        progress.AdvCount = _advCount;
    }

    public void LoadProgress(PlayerProgress progress)
    {
        _pointsPerClick = progress.PointsPerClick;
        _advCount = progress.AdvCount;

        if (_pointsPerClick == 0)
            _pointsPerClick = 1;
    }

    public void AddPointsPerClick()
    {
        _pointsPerClick++;
        RefreshText();
        PointsPerClickReceived?.Invoke(_pointsPerClick);
    }

    private void AddPointsPerClickAdv()
    {
        _advCount++;
        _pointsPerClick += _advCount;
        RefreshText();
        PointsPerClickReceived?.Invoke(_pointsPerClick);
    }

    private void RefreshText() => 
        _pointsPerClickHud.text = $"+{_pointsPerClick}";
}