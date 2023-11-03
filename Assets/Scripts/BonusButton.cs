using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BonusButton : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ShowRevardedAdv(long count);

    [SerializeField] private Button _bonusButton;
    [SerializeField] private Points _points;
    [SerializeField] private YandexAdv _yandexAdv;

    private void OnValidate()
    {
        _bonusButton = gameObject.GetComponent<Button>();
    }

    public void Construct(YandexAdv yandexAdv)
    {
        _yandexAdv = yandexAdv;
    }

    private void Start()
    {
        _bonusButton.onClick.AddListener(AddBonus);
    }

    private void OnDestroy() =>
        _bonusButton.onClick.RemoveListener(AddBonus);

    private void AddBonus()
    {
        float procent = Random.Range(0.01f, 0.1f);

        long bonus = (long)(_points.CurrentPoints * procent);

        _yandexAdv.ShowRevarded(bonus);
    }
}