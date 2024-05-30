using UnityEngine;

public class AnimationView : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAnimation(string stateName)
    {
        animator.Play(stateName);
    }

    public string GetCurrentAnimationState()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        var currentClips = animator.GetCurrentAnimatorClipInfo(0);
        if (currentClips.Length > 0)
        {
            return currentClips[0].clip.name;
        }
        return string.Empty; // Return an empty string if no animation clip is found
    }

    public bool IsInAnimationState(string stateName)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(stateName);
    }

    public bool IsAnimationFinished()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.normalizedTime >= 1f && !animator.IsInTransition(0);
    }
}
