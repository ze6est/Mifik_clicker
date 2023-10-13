using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LockedButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _pointsPerUnlockedHud;
    [SerializeField] private Image _icon;
    [SerializeField] private Image _lockedImage;

    private bool _isLocked;
    private Points _points;
    private long _pointsPerUnlocked;

    public void Construct(Points points, long pointsPerUnlocked)
    {
        _points = points;
        _pointsPerUnlocked = pointsPerUnlocked;
        _pointsPerUnlockedHud.text = $"{_pointsPerUnlocked}";
        _isLocked = true;
        _icon.sprite = Resources.Load<Sprite>("Images/LockedButton/Images");
        _lockedImage.sprite = Resources.Load<Sprite>("Images/LockedButton/GUI/Locked");

        if (_isLocked)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }

    private void OnValidate()
    {
        _pointsPerUnlockedHud = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        _icon = gameObject.GetComponent<Image>();
    }
}