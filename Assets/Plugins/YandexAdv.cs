using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class YandexAdv : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ShowFullScreenAdvertisement();

    [DllImport("__Internal")]
    private static extern void ShowRevardedAdv();

    public event Action AdvertisementRevarded;
    public event Action RevardedVideoClosed;

    private void OnDestroy() => 
        ShowFullScreenAdvertisement();

    public void ShowFullScreen() => 
        ShowFullScreenAdvertisement();

    public void AddPointsPerClick() => 
        AdvertisementRevarded?.Invoke();    

    public void RefreshPoints() => 
        RevardedVideoClosed?.Invoke();

    public void ShowRevarded() => 
        ShowRevardedAdv();
}