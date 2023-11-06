using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BonusButton : MonoBehaviour
{
    [SerializeField] private Button _bonusButton;
    [SerializeField] private Points _points;
    [SerializeField] private YandexAdv _yandexAdv;

    public event Action<long> RevardedVideoClosed;

    private void OnValidate() => 
        _bonusButton = gameObject.GetComponent<Button>();

    public void Construct(YandexAdv yandexAdv)
    {
        _yandexAdv = yandexAdv;
    }

    private void Start()
    {
        _bonusButton.onClick.AddListener(ShowRevarded);
        _yandexAdv.ShowRevardedAdvRewarded += OnRevardedVideoClosed;
    }

    private void OnDestroy()
    {
        _bonusButton.onClick.RemoveListener(ShowRevarded);
        _yandexAdv.ShowRevardedAdvRewarded -= OnRevardedVideoClosed;
    }

    private void ShowRevarded()
    {
        _yandexAdv.ShowRevarded();        
    }

    private void OnRevardedVideoClosed()
    {
        float procent = UnityEngine.Random.Range(1, 11);

        long bonus = (long)(_points.CurrentPoints * procent / 100);

        RevardedVideoClosed?.Invoke(bonus);
        _points.RefreshPoints(bonus);
    }
}