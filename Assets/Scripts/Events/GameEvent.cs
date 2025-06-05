using AISystem.Common.Base.Unity;
using UnityEngine;

namespace DefaultNamespace.Events
{
    public abstract class GameEvent : MonoBehaviour
    {
        public abstract void onStartAction();
        public abstract void onStopAction();
        
        public abstract void onStartEvent();
        public abstract void onStopEvent();
    }
}