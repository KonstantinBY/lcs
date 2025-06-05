using DefaultNamespace.Events;
using UnityEngine;
using UnityEngine.Serialization;

namespace Events
{
    public class PhoneCallEvent : GameEvent
    {
        public Transform phone;
        public Transform phoneSlot;
        public Transform hand;
        
        private Animation phoneAnimation;
        private AudioSource audioSource;
        
        
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.Stop();
            
            phoneAnimation = phone.GetComponent<Animation>();
            phoneAnimation.Stop();
            phoneAnimation.wrapMode = WrapMode.Loop;
        }

        public override void onStartEvent()
        {
            audioSource.Play();
            phoneAnimation.Play("anim_phone_call");
        }

        public override void onStopEvent()
        {
            phoneAnimation.Stop();
            audioSource.Stop();
        }
        
        public override void onStartAction()
        {
            phone.SetParent(hand, false);
            phone.localPosition = Vector3.zero;
            phone.localRotation = Quaternion.identity;
        }

        public override void onStopAction()
        {
            phone.SetParent(phoneSlot, false);
            phone.localPosition = Vector3.zero;
            phone.localRotation = Quaternion.identity;
        }
    }
}