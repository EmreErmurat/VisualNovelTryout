using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VisualNovelTryout.Animation
{
    public class TextBoxAnimationController : MonoBehaviour
    {
        Animator animator;



        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            this.gameObject.SetActive(false);
        }

        public void TextBoxSetAnimation(bool status)
        {
            if (status)
            {
                animator.SetTrigger("SetTrue");
            }
            else
            {
                animator.SetTrigger("SetFalse");
            }
        }
    }

}
