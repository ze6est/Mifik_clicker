using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(AudioSource))]
public class ClickButton : MonoBehaviour
{
    [SerializeField] private AwardsPerClick _awardsPerClick;
    [SerializeField] private Button _clickButton;
    [SerializeField] private AudioSource _audioSource;

    public event Action<long> ButtonClicked;

    private void OnValidate()
    {
        _clickButton = gameObject.GetComponent<Button>();
        _audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void Construct(AwardsPerClick awardsPerClick)
    {
        _awardsPerClick = awardsPerClick;
    }

    private void Start() => 
        _clickButton.onClick.AddListener(OnClickButton);

    private void OnDestroy() => 
        _clickButton.onClick.RemoveListener(OnClickButton);

    private void OnClickButton()
    {
        ButtonClicked?.Invoke(_awardsPerClick.PointsPerClick);
        _audioSource.Play();
    }
}