using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace.Events
{
    public class BirdsEvent : GameEvent
    {
        public bool isButtonsTurnedOn = false;
        
        [FormerlySerializedAs("_landingSpotController")] public LandingSpotController landingSpotController;
        public FlockController _flockController;
        public float hSliderValue = 250.0f;

        public override void onStartEvent()
        {
            Debug.Log($"Birt: LandAll");
            landingSpotController.LandAll();
        }
        
        public override void onStopEvent()
        {
            Debug.Log($"Birt: ScareAll");

            // Check if Init
            if (!landingSpotController._transformCache)
            {
                return;
            }
            
            landingSpotController.ScareAll();
        }

        public override void onStartAction()
        {
            
        }

        public override void onStopAction()
        {
            
        }

        public void OnGUI() {
            if (!isButtonsTurnedOn)
            {
                return;
            }
            
            GUI.Label(new Rect(20.0f,20.0f,125.0f,18.0f),"Landing Spots: " + landingSpotController.transform.childCount);
            if(GUI.Button(new Rect(20.0f,40.0f,125.0f,18.0f),"Scare All"))
                landingSpotController.ScareAll();
            if(GUI.Button(new Rect(20.0f,60.0f,125.0f,18.0f),"Land In Reach"))
                landingSpotController.LandAll();
            if(GUI.Button(new Rect(20.0f,80.0f,125.0f,18.0f),"Land Instant"))
                StartCoroutine(landingSpotController.InstantLand(0.01f));
            if(GUI.Button(new Rect(20.0f,100.0f,125.0f,18.0f),"Destroy")){
                _flockController.destroyBirds();
            }
            GUI.Label(new Rect(20.0f,120.0f,125.0f,18.0f),"Bird Amount: " + _flockController._childAmount);
            _flockController._childAmount = (int)GUI.HorizontalSlider(new Rect (20.0f, 140.0f, 125.0f, 18.0f), (float)_flockController._childAmount, 0.0f , 250.0f);
        }
    }
}