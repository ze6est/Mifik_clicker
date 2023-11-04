using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(TextMeshProUGUI))]
public class AwardsPerClick : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _pointsPerClickHud;
    [SerializeField] private long _pointsPerClick;
    [SerializeField] private YandexAdv _yandexAdv;

    public long PointsPerClick => _pointsPerClick;

    public event Action<long> PointsPerClickReceived;

    private void OnValidate() => 
        _pointsPerClickHud = gameObject.GetComponent<TextMeshProUGUI>();

    public void Construct(YandexAdv yandexAdv)
    {        
        _yandexAdv = yandexAdv;
    }

    private void Awake()
    {
        _pointsPerClick = 1;
        PointsPerClickReceived?.Invoke(_pointsPerClick);
    }

    private void Start()
    {
        _yandexAdv.AdvertisementRewarded += AddPointsPerClick;

        RefreshText();
    }

    private void OnDestroy()
    {
        _yandexAdv.AdvertisementRewarded -= AddPointsPerClick;
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