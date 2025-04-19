public class EIdleSController : EnemyBaseStateController
{
    public EIdleSController(Enemy enemy) : base(enemy) { }

    public override void OnEnter()
    {
        base.OnEnter();

        _enemy.idleSInstance.DoOnEnter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // --- Timers ---
        // ----------------------------------------------------------------------------------------------------------------------------------

        // --- Logic ---
        _enemy.idleSInstance.DoLogicUpdate();
        // ----------------------------------------------------------------------------------------------------------------------------------

        // --- State Transitions ---
        // ----------------------------------------------------------------------------------------------------------------------------------
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        _enemy.idleSInstance.DoPhysicsUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();

        _enemy.idleSInstance.DoOnExit();
    }
}