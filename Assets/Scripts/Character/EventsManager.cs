using System.Collections.Generic;
using System.Linq;
using ToonPeople;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public float delayToFirstEvent = 3;
    public float delayBetweenEvents = 3;
    
    // Each level will be increased on this value
    public float complexityIndex = 0.2f;

    // Depends on level
    private float currentDelayBetweenEvents;
    
    private float delayTime;
    private bool isFirstDelayCompleted;

    [SerializeField] private List<EventData> events;
    // Dictionary only for getting of events by name
    private Dictionary<string, EventData> eventDictionary;
    
    private GameController gc;
    
    private int currentEvent = -1;
    private float currentComfortReduce = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gc = GetComponent<GameController>();
        currentDelayBetweenEvents = delayBetweenEvents - (gc.levelData.level * complexityIndex);
        
        for (int i = 0; i < events.Count; i++)
        {
            events.ElementAt(i).Item.SetActive(false);
        }
        
        eventDictionary = events.ToDictionary(e => e.name, e => e);

        delayTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        gc.reduceComfort(currentComfortReduce);
        
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
        
        runEvent(randomEventIndex);
    }

    private float calculateCurrentDelayBetweenEvents()
    {
        return delayBetweenEvents - (gc.levelData.level * complexityIndex);
    }

    private void runEvent(int eventIndex)
    {
        EventData eventData = events.ElementAt(eventIndex);
        eventData.Item.SetActive(true);
        currentComfortReduce = eventData.comfortReduce;
    }

    public void eventIsCompleted()
    {
        currentEvent = -1;
    }

    public EventData getEventData(string name)
    {
        return eventDictionary[name];
    }

    [System.Serializable]
    public class EventData
    {
        public string name;
        public GameObject Item;
        public int comfortBonus = 10;
        
        // each a 0.2 sec
        public float comfortReduce = 0.8f;
    }

    public void processAction(PlayerStateEnum playerAction)
    {
        if (currentEvent < 0)
        {
            return;
        }
        
        EventData eventData = events.ElementAt(currentEvent);
        
        if (playerAction.ToString() == eventData.name)
        {
            eventData.Item.SetActive(false);
            gc.addComfort(eventData.comfortBonus);
            
            currentEvent = -1;
            currentComfortReduce = 0;
            delayTime = Time.time;
        }
        
    }
}
