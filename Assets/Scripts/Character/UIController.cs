using UnityEngine;
using UnityEngine.UI;
using System;

public class UIController : MonoBehaviour
{
    [Header("UI Кнопки")]
    public Button startButton;
    public Button settingsButton;
    public Button exitButton;
    public Button idleButton;
    public Button callWaitressDrinkButton;
    public Button callWaitressFoodButton;
    public Button flyButton;

    // === СОБЫТИЯ ===
    public event Action OnStartClicked;
    public event Action OnSettingsClicked;
    public event Action OnExitClicked;
    public event Action OnSetStateIdle;
    public event Action OnSetStateCallWaitressDrink;
    public event Action OnSetStateCallWaitressFood;
    public event Action OnSetStateFly;

    void Start()
    {
        startButton?.onClick.AddListener(() => OnStartClicked?.Invoke());
        settingsButton?.onClick.AddListener(() => OnSettingsClicked?.Invoke());
        exitButton?.onClick.AddListener(() => OnExitClicked?.Invoke());

        idleButton?.onClick.AddListener(() => OnSetStateIdle?.Invoke());
        callWaitressDrinkButton?.onClick.AddListener(() => OnSetStateCallWaitressDrink?.Invoke());
        callWaitressFoodButton?.onClick.AddListener(() => OnSetStateCallWaitressFood?.Invoke());
        flyButton?.onClick.AddListener(() => OnSetStateFly?.Invoke());
    }
}