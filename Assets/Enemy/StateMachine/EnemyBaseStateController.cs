public class EnemyBaseStateController
{
    // --- Components ---
    protected Enemy _enemy = null;


    public EnemyBaseStateController(Enemy enemy)
    {
        _enemy = enemy;
    }

    public virtual void OnEnter() { }

    public virtual void LogicUpdate() { }

    public virtual void PhysicsUpdate() { }

    public virtual void OnExit() { }
}