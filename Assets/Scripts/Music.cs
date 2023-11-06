using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private YandexAdv _yandexAdv;

    [SerializeField] private bool _isPlay;

    private void Start()
    {
        _yandexAdv.FullScreenAdvertisementOpened += OnFullScreenAdvertisementOpened;
        _yandexAdv.FullScreenAdvertisementClosed += OnFullScreenAdvertisementClosed;
        _yandexAdv.ShowRevardedAdvOpened += OnShowRevardedAdvOpened;
        _yandexAdv.ShowRevardedAdvClosed += OnShowRevardedAdvClosed;

        _audioSource.clip = _audioClip;
        _audioSource.Play();

        _isPlay = true;
    }

    private void OnDestroy()
    {
        _yandexAdv.FullScreenAdvertisementOpened -= OnFullScreenAdvertisementOpened;
        _yandexAdv.FullScreenAdvertisementClosed -= OnFullScreenAdvertisementClosed;
        _yandexAdv.ShowRevardedAdvOpened -= OnShowRevardedAdvOpened;
        _yandexAdv.ShowRevardedAdvClosed -= OnShowRevardedAdvClosed;
    }

    private void OnFullScreenAdvertisementOpened() => 
        StopMusic();

    private void OnFullScreenAdvertisementClosed()
    {
        if(!_isPlay)
            StartMusic();
    }

    private void OnShowRevardedAdvOpened() =>
        StopMusic();

    private void OnShowRevardedAdvClosed() =>
        StartMusic();

    private void StartMusic()
    {
        _audioSource.Play();
        _isPlay = true;
    }

    private void StopMusic()
    {
        _audioSource.Pause();
        _isPlay = false;
    }
}