using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _pointsPerSecondHUD;
    [SerializeField] private TextMeshProUGUI _upgradeCountHUD;
    [SerializeField] private Image _icon;

    public void Construct(MifiksName nameId, long pointsPerSecond, int upgradeCount, Image icon)
    {
        _name.text = Enum.GetName(typeof(MifiksName), nameId);        
        _pointsPerSecondHUD.text = $"{pointsPerSecond}";
        _upgradeCountHUD.text = $"{upgradeCount}";
        _icon = icon;
    }
}