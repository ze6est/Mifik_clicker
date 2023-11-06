using UnityEngine;
using TMPro;

public class BonusPanel : MonoBehaviour
{
    [SerializeField] private YandexAdv _yandexAdv;
    [SerializeField] private BonusButton _bonusButton;
    [SerializeField] private TextMeshProUGUI _mifikCoinText;
    [SerializeField] private TextMeshProUGUI _mifikCoinPerClickText;

    private bool _isAdShown;

    public void Construct(YandexAdv yandexAdv, BonusButton bonusButton)
    {
        _yandexAdv = yandexAdv;
        _bonusButton = bonusButton;

        _yandexAdv.FullScreenAdvertisementOpened += OnFullScreenAdvertisementOpened;
        _yandexAdv.FullScreenAdvertisementClosed += OnFullScreenAdvertisementClosed;
        _bonusButton.RevardedVideoClosed += OnRevardedVideoClosed;

        gameObject.SetActive(false);
    }    

    private void OnDestroy()
    {
        _yandexAdv.FullScreenAdvertisementOpened -= OnFullScreenAdvertisementOpened;
        _yandexAdv.FullScreenAdvertisementClosed -= OnFullScreenAdvertisementClosed;
        _bonusButton.RevardedVideoClosed -= OnRevardedVideoClosed;
    }

    private void OnFullScreenAdvertisementOpened() => 
        _isAdShown = true;

    private void OnFullScreenAdvertisementClosed()
    {        
        if (_isAdShown)
        {
            gameObject.SetActive(true);
            _mifikCoinText.enabled = false;
            _mifikCoinPerClickText.text = $"+1 мифкоин за клик";

            _isAdShown = false;
        }        
    }

    private void OnRevardedVideoClosed(long count)
    {
        gameObject.SetActive(true);

        _mifikCoinText.enabled = true;
        _mifikCoinText.text = $"{count} мифкоинов";

        _mifikCoinPerClickText.text = $"+1 мифкоин за клик";
    }
}