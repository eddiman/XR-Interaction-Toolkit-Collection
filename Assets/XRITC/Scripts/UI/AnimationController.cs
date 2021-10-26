using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class AnimationController : MonoBehaviour
{
    public bool runOnEnable = true;
    public string runAnimationName;
    public UnityEvent runAnimationHasStarted;
    public bool disableOnWaitAndPlayAnimationEnd;

    public List<AnimationShortClip> animationClipList = new List<AnimationShortClip>();
    private bool _isAnimRunning;

    public void OnEnable()
    {
        if (runOnEnable)
        {
            RunAnimation();
        }
    }

    public void RunAnimation()
    {
        GetComponent<Animator>().Play(Animator.StringToHash("Base Layer" + runAnimationName));
        runAnimationHasStarted.Invoke();
    }
    public void PlayAndWaitForAnim(string waitAndPlayAnimationName)
    {
        foreach (var anim in animationClipList.Where(anim => anim.animName == waitAndPlayAnimationName))
        {
            if (anim.isAnimRunning || !gameObject.activeSelf) return;
            StartCoroutine(PlayAndWaitForAnim(GetComponent<Animator>(), anim));
            return;
        }
    }



    public IEnumerator PlayAndWaitForAnim(Animator targetAnim, AnimationShortClip anim)
    {
        anim.isAnimRunning = true;
        const string animBaseLayer = "Base Layer";
        int CloseHash = Animator.StringToHash(animBaseLayer + anim.animName);
        //Get hash of animation
        int animHash = 0;
        animHash = CloseHash;

        //targetAnim.Play(MenuAnimCloseHash);
        targetAnim.CrossFadeInFixedTime(anim.animName, .2f);

        //Wait until we enter the current state
        /* while (targetAnim.GetCurrentAnimatorStateInfo(0).fullPathHash != animHash)
     {
         Debug.Log("enter Stattae");
         yield return null;
     }*/

        float counter = 0;
        float waitTime = targetAnim.GetCurrentAnimatorStateInfo(0).length;

        //Now, Wait until the current state is done playing
        while (counter < (waitTime))
        {

            counter += Time.deltaTime;
            yield return null;
        }
        anim.isAnimRunning = false;
        if (anim.disableOnWaitAndPlayAnimationEnd)
        {
            gameObject.SetActive(false);
        }
        anim.waitAndPlayAnimationFinished.Invoke();
    }
    [Serializable]
    public struct AnimationShortClip
    {
        public string animName;
        public UnityEvent waitAndPlayAnimationStarted;
        public UnityEvent waitAndPlayAnimationFinished;
        public bool isAnimRunning;
        public bool disableOnWaitAndPlayAnimationEnd;
        public void Init()
        {
            waitAndPlayAnimationStarted.Invoke();
        }
    }
}
