using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(TextMeshProUGUI))]
public class AwardsPerClick : MonoBehaviour, ISavedProgress
{
    [SerializeField] private TextMeshProUGUI _pointsPerClickHud;
    [SerializeField] private long _pointsPerClick;
    [SerializeField] private YandexAdv _yandexAdv;

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
        _yandexAdv.ShowRevardedAdvRewarded += AddPointsPerClick;
        _yandexAdv.FullScreenAdvertisementOpened += AddPointsPerClick;

        RefreshText();
    }

    private void OnDestroy()
    {
        _yandexAdv.ShowRevardedAdvRewarded -= AddPointsPerClick;
        _yandexAdv.FullScreenAdvertisementOpened -= AddPointsPerClick;
    }

    public void UpdateProgress(PlayerProgress progress)
    {
        progress.PointsPerClick = _pointsPerClick;
    }

    public void LoadProgress(PlayerProgress progress)
    {
        _pointsPerClick = progress.PointsPerClick;

        if (_pointsPerClick == 0)
            _pointsPerClick = 1;
    }

    public void AddPointsPerClick()
    {
        _pointsPerClick++;
        RefreshText();
        PointsPerClickReceived?.Invoke(_pointsPerClick);
    }

    private void RefreshText() => 
        _pointsPerClickHud.text = $"+{_pointsPerClick}";
}