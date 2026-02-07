using UnityEngine;

public class AttackState : IState
{
    private Enemy enemy;
    private float timer;
    private bool hasAttacked;

    public AttackState(Enemy _enemy) : base(_enemy) { }

    public override void Enter()
    {
        Debug.Log("상태 변경: ATTACK");
        _enemy.SetVelocity(0);
        timer = 0;
        hasAttacked = false;

        // 공격 애니메이션 시작
        // enemy.animator.SetTrigger("Attack"); 
    }

    public override void Update()
    {
        timer += Time.deltaTime;
        
        if (timer >= _enemy.attackWindup && !hasAttacked)
        {
            Debug.Log("Enemy has Attacked!");
            _enemy.PerformAttack();
            hasAttacked = true;
        }
        
        if (timer >= _enemy.attackDuration)
        {
            _enemy.lastAttackTime = Time.time;
            _enemy.ChangeState(_enemy.chaseState);
        }
    }

    public override void Exit()
    {
        // 필요하다면 공격 애니메이션 종료 처리 등
    }
}
