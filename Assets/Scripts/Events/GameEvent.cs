using AISystem.Common.Base.Unity;
using UnityEngine;

namespace DefaultNamespace.Events
{
    public abstract class GameEvent : MonoBehaviour
    {
        public abstract void start();
        public abstract void stop();
    }
}