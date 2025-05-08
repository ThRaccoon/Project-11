public class EReturnSController : EnemyBaseStateController
{
    public EReturnSController(Enemy enemy) : base(enemy) { }

    public override void OnEnter()
    {
        base.OnEnter();

        _enemy.returnSInstance.DoOnEnter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _enemy.returnSInstance.DoLogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        _enemy.returnSInstance.DoPhysicsUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();

        _enemy.returnSInstance.DoOnExit();
    }
}