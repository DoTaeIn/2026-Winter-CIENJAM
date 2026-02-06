using UnityEngine;

public class IdleState : IState
{
    float timer;

    public IdleState(Enemy _enemy) : base(_enemy) { }

    public override void Enter()
    {
        _enemy.SetVelocity(0);
        Debug.Log("상태 변경: IDLE (대기)");
        timer = _enemy.idleTime;
    }

    public override void Update()
    {
        timer -= Time.deltaTime;

        // 플레이어가 보이면 바로 추격
        if (_enemy.CheckPlayer())
        {
            _enemy.ChangeState(_enemy.chaseState);
            return;
        }

        // 시간 다 되면 다시 순찰
        if (timer <= 0)
        {
            _enemy.Flip();
            _enemy.ChangeState(_enemy.patrolState);
        }
    }
    
    public override void Exit() { }
}
