using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(AudioSource))]
public class BonusButton : MonoBehaviour
{
    [SerializeField] private Button _bonusButton;
    [SerializeField] private Points _points;
    [SerializeField] private YandexAdv _yandexAdv;
    [SerializeField] private ExitPanelButton _exitPanelButton;
    [SerializeField] private AudioSource _audioSource;

    public event Action<long> RevardedVideoClosed;

    private void OnValidate()
    {
        _bonusButton = gameObject.GetComponent<Button>();
        _audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void Construct(YandexAdv yandexAdv)
    {
        _yandexAdv = yandexAdv;
    }

    private void Awake()
    {
        _exitPanelButton.PanelClosed += OnPanelClosed;
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

    private void OnPanelClosed() =>
        _audioSource.Play();

    private void ShowRevarded()
    {
        _yandexAdv.ShowRevarded();        
    }

    private void OnRevardedVideoClosed()
    {
        float procent = UnityEngine.Random.Range(5, 16);

        long bonus = (long)(_points.CurrentPoints * procent / 100);

        RevardedVideoClosed?.Invoke(bonus);
        _points.RefreshPoints(bonus);
    }
}