using UnityEngine;
using System.Collections.Generic;

public class AnimationController : MonoBehaviour
{
    private GameState gameState;
    private AnimationView animationView;

    // Dictionary to store whether an animation is blocking or not
    private Dictionary<string, bool> animationBlockingStates = new Dictionary<string, bool>();

    void Start()
    {
        gameState = new GameState();
        animationView = GetComponent<AnimationView>();
    }

    // Method to register an animation as blocking or non-blocking
    public void RegisterAnimation(string stateName, bool isBlocking)
    {
        animationBlockingStates[stateName] = isBlocking;
    }

    public void TriggerAnimation(string stateName)
    {
        if (!IsAnimationBlocking() || animationView.IsAnimationFinished())
        {
            gameState.SetAnimationState(stateName);
            animationView.PlayAnimation(stateName);
        }
    }

    public string GetCurrentAnimationState()
    {
        return animationView.GetCurrentAnimationState();
    }

    public bool IsInAnimationState(string stateName)
    {
        return animationView.IsInAnimationState(stateName);
    }

    public bool IsAnimationFinished()
    {
        return animationView.IsAnimationFinished();
    }

    // Check if the current animation is blocking
    public bool IsAnimationBlocking()
    {
        string currentAnimation = GetCurrentAnimationState();
        if (animationBlockingStates.TryGetValue(currentAnimation, out bool isBlocking))
        {
            return isBlocking;
        }
        return false;
    }
}
