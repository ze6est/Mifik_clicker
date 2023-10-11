using UnityEngine;
using TMPro;
using System.Collections;
using System;

[RequireComponent(typeof(TextMeshProUGUI))]
public class AutoClick : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _pointsPerSecondHud;

    private long _pointsPerSecond;
    private Coroutine _getPoints;

    public event Action<long> SecondsPassed;

    private void OnValidate() =>
        _pointsPerSecondHud = gameObject.GetComponent<TextMeshProUGUI>();

    private void Awake()
    {
        _pointsPerSecond = 1;
    }

    private void Start()
    {
        _getPoints = StartCoroutine(GetPoints());
    }

    private void OnDestroy()
    {
        StopCoroutine(_getPoints);
    }

    private IEnumerator GetPoints()
    {
        WaitForSeconds waitingTime = new WaitForSeconds(1f);

        while (true)
        {
            yield return waitingTime;

            SecondsPassed?.Invoke(_pointsPerSecond);
            RefreshText();
        }
    }

    private void RefreshText() =>
        _pointsPerSecondHud.text = $"+{_pointsPerSecond}";
}
