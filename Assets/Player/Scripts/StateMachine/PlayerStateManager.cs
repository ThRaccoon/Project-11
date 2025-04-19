public class PlayerStateManager 
{ 
    public PlayerBaseState currentState {  get; private set; }  


    public void Initialize(PlayerBaseState startingState) 
    {
        currentState = startingState;
        currentState.OnEnter();
    }

    public void ChangeState(PlayerBaseState newState) 
    {
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(); 
    }
}