using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private GameState gameState;
    private AnimationView animationView;

    void Start()
    {
        gameState = new GameState();
        animationView = GetComponent<AnimationView>();
    }

    public void TriggerAnimation(string stateName)
    {
        gameState.SetAnimationState(stateName);
        animationView.PlayAnimation(stateName);
    }

    public string GetCurrentAnimationState()
    {
        return animationView.GetCurrentAnimationState();
    }

    public bool IsInAnimationState(string stateName)
    {
        return animationView.IsInAnimationState(stateName);
    }
}
