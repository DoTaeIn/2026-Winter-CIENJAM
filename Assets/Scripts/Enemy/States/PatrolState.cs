using UnityEngine;

public class PatrolState : IState
{
    public PatrolState(Enemy _enemy) : base(_enemy) { }

    public override void Enter()
    {
        //Debug.Log("상태 변경: Patrol");
    }

    public override void Update()
    {
        _enemy.SetVelocity(_enemy.moveSpeed * _enemy.facingDir);
        
        if (_enemy.CheckPlayer())
        {
            _enemy.ChangeState(_enemy.chaseState);
            return;
        }
        
        if (_enemy.CheckEnvironment())
        {
            _enemy.ChangeState(_enemy.idleState);
        }
    }
    
    public override void Exit() { }
}
