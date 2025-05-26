using System;
using DefaultNamespace.Events;
using UnityEngine;

namespace Events
{
    public class PhoneCallEvent : GameEvent
    {
        private AudioSource audioSource;
        
        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.Stop();
        }

        public override void start()
        {
            audioSource.Play();
        }

        public override void stop()
        {
            audioSource.Stop();
        }
    }
}