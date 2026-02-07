using UnityEngine;

public class ChaseState : IState
{
    public ChaseState(Enemy _enemy) : base(_enemy) { }

    public override void Enter() 
    { 
        //Debug.Log("���� ����: CHASE");
    }

    public override void Update()
    {
        if (_enemy.target == null) return;
        
        float direction = (_enemy.target.position.x > _enemy.transform.position.x) ? 1f : -1f;
        
        if (direction * _enemy.facingDir < 0) _enemy.Flip();
        
        if (!_enemy.CheckEnvironment())
        {
            _enemy.SetVelocity(_enemy.moveSpeed * 1.5f * direction);
        }
        else
        {
            _enemy.SetVelocity(0);
        }
        
        float distanceX = Mathf.Abs(_enemy.target.position.x - _enemy.transform.position.x);
        float distanceY = Mathf.Abs(_enemy.target.position.y - _enemy.transform.position.y);

        if (distanceX > _enemy.detectRange || distanceY > 2f)
        {
            _enemy.target = null;
            _enemy.ChangeState(_enemy.patrolState);
        }

        if(distanceX < _enemy.attackRange)
        {
            _enemy.ChangeState(_enemy.attackState);
        }

    }
    
    public override void Exit() { }
}
