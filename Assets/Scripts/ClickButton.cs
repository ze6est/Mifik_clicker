using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ClickButton : MonoBehaviour
{
    [SerializeField] private AwardsPerClick _awardsPerClick;

    private Button _clickButton;

    public event Action<int> ButtonClicked;

    private void OnValidate()
    {
        _clickButton = gameObject.GetComponent<Button>();
        _awardsPerClick = FindObjectOfType<AwardsPerClick>();
    }

    private void Start() => 
        _clickButton.onClick.AddListener(ClickToButton);

    private void OnDestroy() => 
        _clickButton.onClick.RemoveListener(ClickToButton);

    private void ClickToButton() => 
        ButtonClicked?.Invoke(_awardsPerClick.PointsPerClick);
}