using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class LockedButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _pointsPerUnlockedHud;
    [SerializeField] private Image _icon;
    [SerializeField] private Image _lockedImage;
    [SerializeField] private bool _isLocked;
    [SerializeField] private Points _points;
    [SerializeField] private long _pointsPerUnlocked;
    [SerializeField] private AudioSource _audioSource;

    public event Action CardUnlocked;

    private void OnValidate()
    {
        _button = gameObject.GetComponent<Button>();
        _pointsPerUnlockedHud = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        _icon = gameObject.GetComponent<Image>();
        _audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void Construct(Points points, long pointsPerUnlocked, bool isLocked)
    {
        _points = points;
        _pointsPerUnlocked = pointsPerUnlocked;
        _pointsPerUnlockedHud.text = $"{_pointsPerUnlocked}";
        _isLocked = isLocked;
        _icon.sprite = Resources.Load<Sprite>("Images/LockedButton/LockedButton");
        _lockedImage.sprite = Resources.Load<Sprite>("Images/LockedButton/Locked");        
    }

    private void Awake()
    {
        _button.onClick.AddListener(Disable);

        if (_isLocked)
            gameObject.SetActive(true);
        else
        {
            gameObject.SetActive(false);
            CardUnlocked?.Invoke();
        }
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(Disable);
    }

    private void Disable()
    {
        if (_points.CurrentPoints >= _pointsPerUnlocked)
        {
            CardUnlocked?.Invoke();

            _points.RefreshPoints(-_pointsPerUnlocked);

            gameObject.SetActive(false);
            _isLocked = false;
        }
        else
            _audioSource.Play();
    }
}