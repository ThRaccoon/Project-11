public class EAttackSController : EnemyBaseStateController
{
    public EAttackSController(Enemy enemy) : base(enemy) { }

    public override void OnEnter()
    {
        base.OnEnter();

        _enemy.attackSInstance.DoOnEnter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _enemy.attackSInstance.DoLogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        _enemy.attackSInstance.DoPhysicsUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();

        _enemy.attackSInstance.DoOnExit();
    }
}