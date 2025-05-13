using System;
using ToonPeople;
using UnityEngine;

public class ActionServiceController : MonoBehaviour
{
    [SerializeField] private UIController ui;
    [SerializeField] private PlayerStateController playerStateController;
    [SerializeField] private NpcController waitressController;
    
    private bool isActionInProgress;
    
    void Awake()
    {
        ui.OnSetStateIdle += OnSetStateIdle;
        ui.OnSetStateCallWaitressDrink += OnSetStateCallWaitressDrink;
        ui.OnSetStateCallWaitressFood += OnSetStateCallWaitressFood;
    }

    private void OnDestroy()
    {
        ui.OnSetStateIdle -= OnSetStateIdle;
        ui.OnSetStateCallWaitressDrink -= OnSetStateCallWaitressDrink;
        ui.OnSetStateCallWaitressFood -= OnSetStateCallWaitressFood;
    }

    public void OnSetStateIdle()
    {
        playerStateController.setState(PlayerStateEnum.idle, 0.5f);
    }
    
    public void OnSetStateCallWaitressDrink()
    {
        if (isActionInProgress)
        {
            return;
        }
        
        isActionInProgress = true;
        waitressController.moveToPlayer(OnSetStateIdle, OnActionCompleted);
        playerStateController.setState(PlayerStateEnum.callWaitressDrink, 0.8f);
    }
    
    public void OnSetStateCallWaitressFood()
    {
        if (isActionInProgress)
        {
            return;
        }
        
        isActionInProgress = true;
        waitressController.moveToPlayer(OnSetStateIdle, OnActionCompleted);
        playerStateController.setState(PlayerStateEnum.callWaitressFood, 0.8f);
    }

    private void OnActionCompleted()
    {
        isActionInProgress = false;
    }
}
