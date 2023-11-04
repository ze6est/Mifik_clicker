using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class YandexAdv : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ShowFullScreenAdvertisement();

    //[DllImport("__Internal")]
    //private static extern void ShowRevardedAdv(int count);

    public event Action AdvertisementRewarded;

    private void Start()
    {
        transform.parent = null;
    }

    private void OnDestroy()
    {
        ShowFullScreenAdvertisement();
    }

    public void ShowFullScreen()
    {
        ShowFullScreenAdvertisement();
    }

    public void AddPointsPerClick()
    {
        AdvertisementRewarded?.Invoke();
    }

    /*
    public void RefreshPoints(int count)
    {
        _points.RefreshPoints(count);
    }

    public void ShowRevarded(long count)
    {
        int newCount = (int)count;

        ShowRevardedAdv(newCount);
    }
    */
}