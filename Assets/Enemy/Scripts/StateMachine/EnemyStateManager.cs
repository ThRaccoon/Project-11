public class EnemyStateManager
{
    public EnemyBaseStateController currentState { get; private set; }

    public void Initialize(EnemyBaseStateController startingState)
    {
        currentState = startingState;
        currentState.OnEnter();
    }

    public void ChangeState(EnemyBaseStateController newState)
    {
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter();
    }
}