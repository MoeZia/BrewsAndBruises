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
        return animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    }

    public bool IsInAnimationState(string stateName)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(stateName);
    }
}
