using System.Collections;
using DefaultNamespace.Events;
using ToonPeople;
using UnityEngine;
using UnityEngine.Serialization;

public class ActionServiceController : MonoBehaviour
{
    [SerializeField] private UIController ui;
    [SerializeField] private PlayerLimbsController playerLimbsController;
    [SerializeField] private NpcController waitressController;
    
    private bool isActionInProgress;
    private EventsManager eventsManager;
    
    void Awake()
    {
        ui.OnSetStateIdle += OnSetStateIdle;
        ui.OnSetStateCallWaitressDrink += OnSetStateCallWaitressDrink;
        ui.OnSetStateCallWaitressFood += OnSetStateCallWaitressFood;
        ui.OnSetStateFly += OnSetStateFly;
        ui.OnSetStateBirt += OnSetStateBirt;
        ui.OnSetStatePhoneCall += OnSetStatePhoneCall;
    }

    private void OnDestroy()
    {
        ui.OnSetStateIdle -= OnSetStateIdle;
        ui.OnSetStateCallWaitressDrink -= OnSetStateCallWaitressDrink;
        ui.OnSetStateCallWaitressFood -= OnSetStateCallWaitressFood;
        ui.OnSetStateFly += OnSetStateFly;
        ui.OnSetStateBirt -= OnSetStateBirt;
        ui.OnSetStatePhoneCall -= OnSetStatePhoneCall;
    }

    private void Start()
    {
        eventsManager = GetComponent<EventsManager>();
    }

    public void OnSetStateIdle()
    {
        playerLimbsController.setState(PlayerStateEnum.idle, 0.5f);
    }
    
    public void OnSetStateCallWaitressDrink()
    {
        if (isActionInProgress)
        {
            return;
        }
        
        isActionInProgress = true;
        waitressController.moveToPlayer(OnSetStateIdle);
        playerLimbsController.setState(PlayerStateEnum.callWaitressDrink, 0.8f);
        
        eventsManager.processAction(EventEnum.drink);
        
        StartCoroutine(completeActionIn());
    }
    
    public void OnSetStateCallWaitressFood()
    {
        if (isActionInProgress)
        {
            return;
        }
        
        isActionInProgress = true;
        waitressController.moveToPlayer(OnSetStateIdle);
        playerLimbsController.setState(PlayerStateEnum.callWaitressFood, 0.8f);
        
        eventsManager.processAction(EventEnum.food);
        
        StartCoroutine(completeActionIn());
    }
    
    public void OnSetStateFly()
    {
        if (isActionInProgress)
        {
            return;
        }
        
        isActionInProgress = true;
        playerLimbsController.setState(PlayerStateEnum.fly, 1.0f);
        eventsManager.processAction(EventEnum.fly);

        StartCoroutine(completeActionIn());
    }
    
    public void OnSetStateBirt()
    {
        if (isActionInProgress)
        {
            return;
        }
        
        isActionInProgress = true;
        playerLimbsController.setState(PlayerStateEnum.birt, 1.0f);
        eventsManager.processAction(EventEnum.bird);

        StartCoroutine(completeActionIn());
    }    
    
    public void OnSetStatePhoneCall()
    {
        if (isActionInProgress)
        {
            return;
        }

        GameEvent gameEvent = eventsManager.getEventData(EventEnum.phoneCall)?.gameEvent;
        gameEvent?.onStartAction();
        
        isActionInProgress = true;
        playerLimbsController.setState(PlayerStateEnum.phoneCall, 1.0f);
        eventsManager.processAction(EventEnum.phoneCall);

        StartCoroutine(completeActionIn(gameEvent));
    }

    private IEnumerator completeActionIn(GameEvent gameEvent = null, float time = 1f)
    {
        yield return new WaitForSeconds(time);
        
        gameEvent?.onStopAction();
        
        OnSetStateIdle();
        OnActionCompleted();
    }

    private void OnActionCompleted()
    {
        isActionInProgress = false;
    }
}
