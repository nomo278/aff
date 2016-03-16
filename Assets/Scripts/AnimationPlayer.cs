using System;
using System.Collections;

using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationPlayer : MonoBehaviour {

    public string enterAnimation;
    public string exitAnimation;

    public float exitDelay = 0.35f;

    Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}

    public void EnterAnimation()
    {
        GlobalController.instance.SetCurrentMenu(this);
        StopAllCoroutines();
        if(!gameObject.activeInHierarchy)
            gameObject.SetActive(true);
        if(animator == null)
            animator = GetComponent<Animator>();
        gameObject.SetActive(true);
        animator.Play(Animator.StringToHash(enterAnimation));
    }

    public void ExitAnimation()
    {
        StopAllCoroutines();
        if (!gameObject.activeInHierarchy)
            gameObject.SetActive(true);
        if (animator == null)
            animator = GetComponent<Animator>();
        int animHash = Animator.StringToHash(exitAnimation);
        animator.Play(animHash);
        StartCoroutine(WaitForAnimationExit(
            animHash,
            new Action[] { () => gameObject.SetActive(false)  },
            exitDelay
            )
        );
    }

    public void ExitAnimation(AnimationPlayer animPlayer)
    {
        if (!gameObject.activeInHierarchy)
            gameObject.SetActive(true);
        if (animator == null)
            animator = GetComponent<Animator>();
        int animHash = Animator.StringToHash(exitAnimation);
        animator.Play(animHash);
        StartCoroutine(WaitForAnimationExit(
            animHash,
            new Action[] { () => gameObject.SetActive(false), () => animPlayer.EnterAnimation(), },
            exitDelay
            )
        );
    }

    IEnumerator WaitForAnimationExit(int animHash, Action[] actions, float exitDelay)
    {
        yield return new WaitForSeconds(exitDelay);
        foreach (Action action in actions)
        {
            action();
        }       
    }

    public virtual void PlayAnimation(bool enter)
    {
        if (enter)
            EnterAnimation();
        else
            ExitAnimation();
    }
}
