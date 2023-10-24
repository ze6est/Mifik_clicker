using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class AwardsPerClick : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _pointsPerClickHud;
    [SerializeField] private long _pointsPerClick;

    public long PointsPerClick => _pointsPerClick;

    private void OnValidate() => 
        _pointsPerClickHud = gameObject.GetComponent<TextMeshProUGUI>();

    private void Awake() => 
        _pointsPerClick = 2L;

    private void Start() => 
        RefreshText();

    private void RefreshText() => 
        _pointsPerClickHud.text = $"+{_pointsPerClick}";
}