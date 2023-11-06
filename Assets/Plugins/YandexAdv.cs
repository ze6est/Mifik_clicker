using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class YandexAdv : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ShowFullScreenAdvertisement();

    [DllImport("__Internal")]
    private static extern void ShowRevardedAdv();

    public event Action FullScreenAdvertisementOpened;
    public event Action FullScreenAdvertisementClosed;
    public event Action ShowRevardedAdvOpened;
    public event Action ShowRevardedAdvRewarded;
    public event Action ShowRevardedAdvClosed;

    private void OnDestroy() => 
        ShowFullScreenAdvertisement();

    public void OpenFullScreenAdvertisement() => 
        FullScreenAdvertisementOpened?.Invoke();

    public void CloseFullScreenAdvertisement() => 
        FullScreenAdvertisementClosed?.Invoke();

    public void OpenShowRevardedAdv() => 
        ShowRevardedAdvOpened?.Invoke();

    public void RewardedShowRevardedAdv() => 
        ShowRevardedAdvRewarded?.Invoke();

    public void CloseShowRevardedAdv() => 
        ShowRevardedAdvClosed?.Invoke();

    public void ShowFullScreen() => 
        ShowFullScreenAdvertisement();    

    public void ShowRevarded() => 
        ShowRevardedAdv();
}