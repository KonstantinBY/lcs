using UnityEngine;
using UnityEngine.UI;
using System;

public class UIController : MonoBehaviour
{
    [Header("UI Buttons")]
    public Button startButton;
    public Button settingsButton;
    public Button exitButton;
    public Button idleButton;
    public Button callWaitressDrinkButton;
    public Button callWaitressFoodButton;
    public Button flyButton;
    public Button birtButton;
    public Button phoneCallButton;

    // === СОБЫТИЯ ===
    public event Action OnStartClicked;
    public event Action OnSettingsClicked;
    public event Action OnExitClicked;
    public event Action OnSetStateIdle;
    public event Action OnSetStateCallWaitressDrink;
    public event Action OnSetStateCallWaitressFood;
    public event Action OnSetStateFly;
    public event Action OnSetStateBirt;
    public event Action OnSetStatePhoneCall;

    void Start()
    {
        startButton?.onClick.AddListener(() => OnStartClicked?.Invoke());
        settingsButton?.onClick.AddListener(() => OnSettingsClicked?.Invoke());
        exitButton?.onClick.AddListener(() => OnExitClicked?.Invoke());

        idleButton?.onClick.AddListener(() => OnSetStateIdle?.Invoke());
        callWaitressDrinkButton?.onClick.AddListener(() => OnSetStateCallWaitressDrink?.Invoke());
        callWaitressFoodButton?.onClick.AddListener(() => OnSetStateCallWaitressFood?.Invoke());
        flyButton?.onClick.AddListener(() => OnSetStateFly?.Invoke());
        birtButton?.onClick.AddListener(() => OnSetStateBirt?.Invoke());
        phoneCallButton?.onClick.AddListener(() => OnSetStatePhoneCall?.Invoke());
    }
}