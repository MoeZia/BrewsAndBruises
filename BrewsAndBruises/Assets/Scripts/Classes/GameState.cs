public class GameState
{
    public string CurrentAnimationState { get; private set; }

    public void SetAnimationState(string state)
    {
        CurrentAnimationState = state;
    }
}
