using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Events;
using ToonPeople;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EventsManager : MonoBehaviour
{
    public float delayToFirstEvent = 3;
    public float delayBetweenEvents = 3;
    public float delayBetweenStartAndButtons = 1f;
    
    [SerializeField] private UIButtonRandomizer buttonsRandomizer;
    [SerializeField] private List<EventData> events;
    
    
    // Each level will be increased on this value
    public float complexityIndexBetweenEvents = 0.2f;
    public float complexityIndexReduceComfort = 0.2f;

    // Depends on level
    private float currentDelayBetweenEvents;
    
    private float delayTime;
    private bool isFirstDelayCompleted;

    // Dictionary only for getting of events by name
    private Dictionary<EventEnum, EventData> eventDictionary;
    
    private GameController gc;
    
    private int currentEvent = -1;
    private float currentComfortReduce = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gc = GetComponent<GameController>();
        currentDelayBetweenEvents = delayBetweenEvents - (gc.levelData.level * complexityIndexBetweenEvents);
        
        for (int i = 0; i < events.Count; i++)
        {
            showItems(events.ElementAt(i), false);
            showIcon(events.ElementAt(i), false);
        }
        
        eventDictionary = events.ToDictionary(e => e.name, e => e);

        delayTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        gc.reduceComfort(currentComfortReduce * Time.deltaTime);
        
        runEvent();
    }

    private void runEvent()
    {
        if (currentEvent != -1)
        {
            return;
        }

        if (!isFirstDelayCompleted && Time.time - delayTime < delayToFirstEvent)
        {
            return;
        }

        isFirstDelayCompleted = true;

        currentDelayBetweenEvents = calculateCurrentDelayBetweenEvents();
        if (Time.time - delayTime < currentDelayBetweenEvents)
        {
            return;
        }
        
        int randomEventIndex = Random.Range(0, events.Count);
        currentEvent = randomEventIndex;
        
        EventData eventData = events.ElementAt(randomEventIndex);
        
        // Need to show scene before showing Icon and Buttons
        showItems(eventData, true);

        StartCoroutine(runEvent(eventData));
    }

    private float calculateCurrentDelayBetweenEvents()
    {
        return delayBetweenEvents - (gc.levelData.level * complexityIndexBetweenEvents);
    }
    
    IEnumerator runEvent(EventData eventData)
    {
        //showItems(eventData, true);
        
        yield return new WaitForSeconds(delayBetweenStartAndButtons);
        
        showIcon(eventData, true);
        currentComfortReduce = calculateCurrentComportReduce(eventData);
        buttonsRandomizer.ShowRandomSet(eventData.name);
    }

    private float calculateCurrentComportReduce(EventData eventData)
    {
        float eventDataComfortReduce = 
            eventData.comfortReduce + (eventData.comfortReduce * gc.levelData.level * complexityIndexReduceComfort);
        
        Debug.Log($"eventDataComfortReduce = {eventDataComfortReduce}");
        
        return eventDataComfortReduce;
    }

    public void eventIsCompleted()
    {
        currentEvent = -1;
    }

    public EventData getEventData(EventEnum name)
    {
        return eventDictionary[name];
    }

    public void processAction(EventEnum eventName)
    {
        if (currentEvent < 0)
        {
            return;
        }
        
        EventData eventData = events.ElementAt(currentEvent);
        
        if (eventName == eventData.name)
        {
            showItems(eventData, false);
            showIcon(eventData, false);
            
            gc.addComfort(eventData.comfortBonus);
            
            currentEvent = -1;
            currentComfortReduce = 0;
            delayTime = Time.time;
            
            buttonsRandomizer.HideAllButtons();
        }
        
    }

    private void showItems(EventData eventData, bool isShow)
    {
        if (eventData.item)
        {
            eventData.item?.SetActive(isShow);
        }

        if (eventData.gameEvent != null)
        {
            if (isShow)
            {
                eventData.gameEvent.start();
            }
            else
            {
                eventData.gameEvent.stop();
            }
        }
    }
    
    private void showIcon(EventData eventData, bool isShow)
    {
        eventData.eventIcon?.SetActive(isShow);
    }
    
    [Serializable]
    public class EventData
    {
        [SerializeField] public EventEnum name;
        [SerializeField] internal GameObject eventIcon;
        [SerializeField] internal GameObject item;
        [SerializeField] internal GameEvent gameEvent;
        public int comfortBonus = 10;
        
        // each a 0.2 sec
        public float comfortReduce = 0.8f;
    }
}
