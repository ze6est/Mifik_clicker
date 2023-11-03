using System.Runtime.InteropServices;
using UnityEngine;

public class YandexAdv : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ShowFullScreenAdv();

    [DllImport("__Internal")]
    private static extern void ShowRevardedAdv(long count);

    [SerializeField] private Points _points;
    [SerializeField] private AwardsPerClick _awardsPerClick;

    public void Construct(Points points, AwardsPerClick awardsPerClick)
    {
        _points = points;
        _awardsPerClick = awardsPerClick;
    }

    private void Start()
    {
        ShowFullScreenAdv();
    }

    private void OnDestroy()
    {
        ShowFullScreenAdv();
    }

    public void ShowFullScreen()
    {
        ShowFullScreenAdv();
    }

    public void AddPointsPerClick()
    {
        _awardsPerClick.AddPointsPerClick();
    }

    public void RefreshPoints(long count)
    {
        _points.RefreshPoints(count);
    }

    public void ShowRevarded(long count)
    {
        ShowRevardedAdv(count);
    }
}
