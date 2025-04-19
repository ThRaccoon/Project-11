public class EChaseSController : EnemyBaseStateController
{
    public EChaseSController(Enemy enemy) : base(enemy) { }

    public override void OnEnter()
    {
        base.OnEnter();

        _enemy.chaseSInstance.DoOnEnter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // --- Timers ---
        // ----------------------------------------------------------------------------------------------------------------------------------

        // --- Logic ---
        // ----------------------------------------------------------------------------------------------------------------------------------

        // --- State Transitions ---
        _enemy.chaseSInstance.DoLogicUpdate();
        // ----------------------------------------------------------------------------------------------------------------------------------
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        _enemy.chaseSInstance.DoPhysicsUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();

        _enemy.chaseSInstance.DoOnExit();
    }
}