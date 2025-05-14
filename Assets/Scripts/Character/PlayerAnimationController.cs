using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToonPeople
{
    public class PlayerAnimationController : MonoBehaviour
    {
        public string anim;
        public bool delayed;
        public bool happy;
        public bool sad;
        public bool angry;
        public bool amazed;
        public bool disgust;
        public bool numb;
        
        private Animator animator;
        private string currentAnim;

        void Start()
        {
            animator = GetComponent<Animator>();
            animator.Play(anim);
            
            if (happy) GetComponent<Animator>().SetLayerWeight(1, 1f);
            else if (sad) GetComponent<Animator>().SetLayerWeight(2, 1f);
            else if (angry) GetComponent<Animator>().SetLayerWeight(3, 1f);
            else if (amazed) GetComponent<Animator>().SetLayerWeight(4, 1f);
            else if (disgust) GetComponent<Animator>().SetLayerWeight(5, 1f);
            else if (numb) GetComponent<Animator>().SetLayerWeight(6, 1f);
            if (delayed)
            {
                StartCoroutine("playanim", anim);
            }
        }

        IEnumerator playanim(string anim)
        {
            animator.speed = 0.65f;
            yield return new WaitForSeconds(Random.Range(0f, 2f));
            animator.speed = 1f;
            animator.Play(anim);
        }

        public void playtheanimation(string newanim)
        {
            anim = newanim;
            StartCoroutine("playanim", anim);
        }

        public void playAnimation(string animName)
        {
            if (currentAnim == animName)
            {
                return;
            }

            currentAnim = animName;
            animator.Play(animName);
        }
    }
}