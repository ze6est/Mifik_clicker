﻿using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LockedButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _pointsPerUnlockedHud;
    [SerializeField] private Image _icon;
    [SerializeField] private Image _lockedImage;
    [SerializeField] private bool _isLocked;
    [SerializeField] private Points _points;
    [SerializeField] private long _pointsPerUnlocked;

    public event Action CardUnlocked;


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
        _button = gameObject.GetComponent<Button>();
        _pointsPerUnlockedHud = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        _icon = gameObject.GetComponent<Image>();
    }

    private void Awake()
    {
        _button.onClick.AddListener(Disable);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(Disable);
    }

    private void Disable()
    {
        if(_points.CurrentPoints >= _pointsPerUnlocked)
        {
            CardUnlocked?.Invoke();

            _points.RefreshPoints(-_pointsPerUnlocked);

            gameObject.SetActive(false);
            _isLocked = false;            
        }
    }
}