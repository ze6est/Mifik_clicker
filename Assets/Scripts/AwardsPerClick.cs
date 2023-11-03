using UnityEngine;
using TMPro;
using System;
using System.Runtime.InteropServices;

[RequireComponent(typeof(TextMeshProUGUI))]
public class AwardsPerClick : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _pointsPerClickHud;
    [SerializeField] private long _pointsPerClick;

    public long PointsPerClick => _pointsPerClick;

    public event Action<long> PointsPerClickReceived;

    private void OnValidate() => 
        _pointsPerClickHud = gameObject.GetComponent<TextMeshProUGUI>();

    private void Awake()
    {
        _pointsPerClick = 1;
        PointsPerClickReceived?.Invoke(_pointsPerClick);
    }

    private void Start() => 
        RefreshText();

    public void AddPointsPerClick()
    {
        _pointsPerClick++;
        RefreshText();
        PointsPerClickReceived?.Invoke(_pointsPerClick);
    }

    private void RefreshText() => 
        _pointsPerClickHud.text = $"+{_pointsPerClick}";
}